using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace PracticalNotePad
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Brush Brush_Transparent
        {
            get
            {
                return new SolidColorBrush(Color.FromArgb(1, 255, 255, 255));
            }
        }
        private Brush Brush_SemiTransparent
        {
            get
            {
                return new SolidColorBrush(Color.FromArgb(80, 255, 255, 255));
            }
        }
        private Brush Brush_Opaque
        {
            get
            {
                return new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            }
        }


        private Color win_bg_color;
        private bool win_transparent = true;
        private bool canFullTransparent = true;
        private bool panel_displaying = true;
        private bool isTop = true;
        private int current_zoom = 20;

        private byte lastWinOppacity = 0x4a;


        public MainWindow()
        {
            InitializeComponent();
            Init();
            this.Topmost = true;
        }

        void NewWindowAsDialog(object sender, RoutedEventArgs e)
        {
            Window win = new Window();
            win.Owner = this;
            win.ShowDialog();
        }
        void NormalNewWindow(object sender, RoutedEventArgs e)
        {
            Window win = new Window();
            win.Owner = this;
            win.Show();
        }

        private void Init()
        {
            Init_Field();
            Init_Event();
        }
        private void Init_Event()
        {
            
        }
        private void Init_Field()
        {
            win_bg_color = Color.FromArgb(0xff, 0xff, 0xff, 0xff);
            slider_winOpacity.Value = lastWinOppacity;
            SetWindowOpacity(lastWinOppacity);
        }
       
        private void EnableWindowTransparent(bool value)
        {
            if (value) brd_main.Background = Brush_Transparent;
            else SetWindowOpacity(lastWinOppacity);

            win_transparent = value;
        }
        private void SetWindowOpacity(byte alpha)
        {
            brd_main.Background = new SolidColorBrush(Color.FromArgb(Math.Clamp((byte)alpha, (byte)(canFullTransparent ? 0x00: 0x01), (byte)0xff), win_bg_color.R, win_bg_color.G, win_bg_color.B));
            win_transparent = false;
        }

        private void DisplayPanel(bool value)
        {
            Console.WriteLine($"DISPLAY PANEL --> {value}");
            if (value)
            {
                panel.IsEnabled = true;
                panel_border.Background = new SolidColorBrush(Color.FromArgb(0xff, 0xbf, 0xbf, 0xbf));
                panel.Margin = new Thickness(93, 0, 0, 0);
                display_panel.Margin = new Thickness(86,126, 0, 190);
                display_panel.HorizontalAlignment = HorizontalAlignment.Left;
            }
            else
            {
                panel.IsEnabled = false;
                panel_border.Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
                panel.Margin = new Thickness(236, 0, 0, 0);
                display_panel.Margin = new Thickness(0, 126, -7, 190);
                display_panel.HorizontalAlignment = HorizontalAlignment.Right;
            }

            
            panel_displaying = value;
        }

        private void Zoom(bool up)
        {
            if (up) current_zoom++;
            else current_zoom--;
            
            tbox_mainText.FontSize = current_zoom;
        }
        private void OnResize()
        {
            //display_panel.Margin = new Thickness(panel.Margin.Left - 7, display_panel.Margin.Top, display_panel.Margin.Bottom, display_panel.Margin.Right);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }
        private void btn_transparent_Click(object sender, RoutedEventArgs e)
        {
            EnableWindowTransparent(!win_transparent);
        }
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            byte value = Convert.ToByte(e.NewValue);
            SetWindowOpacity(value);
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DisplayPanel(!panel_displaying);
        }
        private void btn_quit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
        private void display_panel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DisplayPanel(!panel_displaying);
        }
        private void btn_minimise_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        private void Window_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl)) Zoom(e.Delta > 0);
        }

        private void btn_top_Click(object sender, RoutedEventArgs e)
        {
            if (!isTop)
            {
                btn_top.Content = "Top  ✓";
                this.Topmost = true;
                isTop = true;
            }
            else
            {
                btn_top.Content = "Top  X";
                this.Topmost = false;
                isTop = false;
            }
            
        }

        private void btn_fullscreen_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
            }
            else
            {
                WindowState = WindowState.Maximized;
            }
        }
    }
}
