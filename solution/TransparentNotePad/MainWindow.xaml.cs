using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using System.Xml;

namespace TransparentNotePad
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public enum AppMode
        {
            Text,
            Draw
        }

        /*---------- Fields ----------*/
        private Color text_area_color;
        private bool win_transparent = true;
        private bool canFullTransparent = true;
        private bool panel_displaying = true;
        private bool isTop = true;
        private int current_zoom = 20;
        private bool fontBox_initalized = false;
        private List<Task> zoomTimers = new List<Task>();
        private CancellationTokenSource? zoomTimerToken;
        private bool crtl_pressed = false;
        private PaintCanvas.PaintBrush currentBrush;
        private bool inDisplayMoving = false;

        private byte lastWinOppacity = 0x4a;
        private AppMode currentMode = AppMode.Text;

        private bool fileSaved = false;
        private string currentTextDocPath;
        private Color panel_color;

        DispatcherTimer dispatcherTimer;

        #region /*------------- Proprety --------------*/

        public Style Panel_Buttons_Style
        {
            get
            {
                object o = Resources["PanelButton"];
                if (o.GetType() == typeof(Style)) return o as Style;
                return null;
            }
        }
        public Style Panel_ButtonsTextBloc_Style
        {
            get
            {
                object o = Resources["PanelButtonText"];
                if (o.GetType() == typeof(Style)) return o as Style;
                return null;
            }
        }
        
        public Color PanelColor
        {
            get
            {
                return this.panel_color;
            }
            set
            {
                this.panel_color = value;
            }
        }
        public Color TextAreaColor
        {
            get
            {
                return text_area_color;
            }
            set
            {
                text_area_color = value;
                SetWindowOpacity(lastWinOppacity);
            }
        }
        public AppMode CurrentMode
        {
            get
            {
                return currentMode;
            }
            set
            {
                SetMode(value);
            }
        }

        private Brush Brush_Transparent
        {
            get
            {
                return new SolidColorBrush(Color.FromArgb(1, 255, 255, 255));
            }
        }

        private Brush Brush_Button_Active
        {
            get
            {
                return Manager.GetBrushFromString(Manager.CurrentTheme.Color_Btn_Brush_Active);
            }
        }
        private Brush Brush_Button_Disable
        {
            get
            {
                return Manager.GetBrushFromString(Manager.CurrentTheme.Color_Btn_Brush_Disable);
            }
        }

        #endregion

        #region /*---------- Initialization -----------*/

        public MainWindow()
        {
            InitializeComponent();
            Init();
            this.Topmost = true;

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromSeconds(0.5);
            dispatcherTimer.Tick += RetardedCall;
            dispatcherTimer.Start();
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
            _ = new Manager(this);
            Manager.SetAssociationWithExtension(
                ".tntxt",
                ".tntxt", 
                System.Reflection.Assembly.GetEntryAssembly().Location,
                "TNTXTX (same as .txt files, but opens by default with Transparent Notpad)");
            Init_Field();
            Init_Event();
            //temp
            SetMode(AppMode.Text);

            string[] args = Environment.GetCommandLineArgs();

            try
            {
                if (args.Length != 0)
                {
                    tbox_mainText.Text = File.ReadAllText(args[1]);
                    currentTextDocPath = args[1];
                    fileSaved = true;
                }
            }
            catch (Exception) { }
        }
        private void Init_Event()
        {
            
        }
        private void Init_Field()
        {
            //text_area_color = Color.FromArgb(0xff, 0xff, 0xff, 0xff);
            slider_winOpacity.Value = lastWinOppacity;
            SetWindowOpacity(lastWinOppacity);
        }
        private void RetardedCall(object? sender, EventArgs args)
        {
            Manager.TryGetStoredFile(out StoredDataFile data_file);
            Console.WriteLine($"RetardedCall");
            Manager.SetTheme(Manager.CurrentTheme);
            Manager.SetDefaultTextZoom(data_file.LastTextZoom);
            Init_FontBox();

            slider_brush_size.Value = 7;
            dispatcherTimer.Tick -= RetardedCall;
            dispatcherTimer.Stop();
            dispatcherTimer = null;
        }

        private void Init_FontBox()
        {
            foreach (var v in cmbbox_Panels_FontSelector.Items)
            {
                FontFamily f = v as FontFamily;

                Manager.TryGetStoredFile(out StoredDataFile file);
                string storedDefaultFont = file.Font;

                if (f != null)
                {
                    if (f.Source == storedDefaultFont)
                    {
                        cmbbox_Panels_FontSelector.SelectedItem = f;
                        Manager.SetDefaultTypeFont(f, false);
                    }
                }
            }

            fontBox_initalized = true;
        }

        #endregion

        #region /*---------- Private Methods ----------*/

        private void EnableWindowTransparent(bool value)
        {
            if (value) brd_main.Background = Brush_Transparent;
            else SetWindowOpacity(lastWinOppacity);

            win_transparent = value;
        }
        private void SetWindowOpacity(byte alpha, bool setSliderValue = true)
        {
            brd_main.Background = new SolidColorBrush(Color.FromArgb(
                Math.Clamp((byte)alpha,(byte)(canFullTransparent ? 0x00 : 0x01),(byte)0xff),
                text_area_color.R,
                text_area_color.G,
                text_area_color.B));

            win_transparent = false;
            lastWinOppacity = alpha;

            if (setSliderValue) slider_winOpacity.Value = alpha;
        }

        private void DisplayPanel(bool value)
        {
            Console.WriteLine($"DISPLAY PANEL --> {value}");
            if (value)
            {
                btn_DisplayPanel.Content = "Hide Panel";
                panel.IsEnabled = true;
                panel_border.Background = new SolidColorBrush(
                    Color.FromArgb(0xff, PanelColor.R, PanelColor.G, PanelColor.B));
                panel.Opacity = 100;
                panel.IsHitTestVisible = true;
                display_panel.IsHitTestVisible = true;
                display_panel.Opacity = 100;
            }
            else
            {
                btn_DisplayPanel.Content = "Show Panel";
                panel.IsEnabled = false;
                panel.IsHitTestVisible = false;
                display_panel.IsHitTestVisible = false;
                panel_border.Background = new SolidColorBrush(
                    Color.FromArgb(0, PanelColor.R, PanelColor.G, PanelColor.B));
                
                panel.Opacity = 0;
                display_panel.Opacity = 0;
            }


            panel_displaying = value;
        }

        private void Zoom(bool up)
        {
            if (up) tbox_mainText.FontSize++;
            else tbox_mainText.FontSize--;

            current_zoom = Convert.ToInt32(tbox_mainText.FontSize);
            Manager.StartZoomTimer();
        }
        
        private void OpenOptions()
        {
            if (!Manager.isOpened_OptionWin)
            {
                if (Manager.TryOpenWindow<OptionWindow>(Manager.OptionWin))
                    Manager.OptionWin.OnOpen();
            }
        }

        private void SetMode(AppMode mode)
        {
            currentMode = mode;

            switch (mode)
            {
                case AppMode.Text:
                    DisableElementsOfDrawMode();
                    EnableElementsOfTextMode();
                    break;
                case AppMode.Draw:
                    DisableElementsOfTextMode();
                    EnableElementsOfDrawMode();
                    UpdateBrushButtons();
                    break;
                default:
                    break;
            }

            Console.WriteLine($"Switch To {mode} Mode!");
        }

        private void DisableElementsOfTextMode()
        {
            Panel_Items_textMode.IsEnabled = false;
            Panel_Items_textMode.Opacity = 0;
            Panel_Items_textMode.IsHitTestVisible = false;

            tbox_mainText.IsEnabled = false;
            tbox_mainText.IsHitTestVisible = false;
            tbox_mainText.Opacity = 0;
        }
        private void DisableElementsOfDrawMode()
        {
            Panel_Items_drawMode.IsEnabled = false;
            Panel_Items_drawMode.Opacity = 0;
            Panel_Items_drawMode.IsHitTestVisible = false;

            paint_area.IsEnabled = false;
            paint_area.IsHitTestVisible = false;
            paint_area.Opacity = 0;
            paint_area.CanPaint = false;
        }
        private void EnableElementsOfTextMode()
        {
            Panel_Items_textMode.IsEnabled = true;
            Panel_Items_textMode.Opacity = 100;
            Panel_Items_textMode.IsHitTestVisible = true;

            tbox_mainText.IsEnabled = true;
            tbox_mainText.IsHitTestVisible = true;
            tbox_mainText.Opacity = 100;
        }
        private void EnableElementsOfDrawMode()
        {
            Panel_Items_drawMode.IsEnabled = true;
            Panel_Items_drawMode.Opacity = 100;
            Panel_Items_drawMode.IsHitTestVisible = true;

            paint_area.IsEnabled = true;
            paint_area.IsHitTestVisible = true;
            paint_area.Opacity = 100;
            paint_area.CanPaint = true;
        }

        

        #endregion

        #region /*---------- Public Methods -----------*/



        #endregion

        #region /*--------- WPF Event Methods ---------*/

        private void PreviewKeyDow(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Tab)
            //{
            //    tbox_mainText.Text += "    ";
            //    return;
            //}
            if (e.Key == Key.E && currentMode == AppMode.Draw)
            {
                OnEraserBtn_Click(this, e);
            }
            if (e.Key == Key.D && currentMode == AppMode.Draw)
            {
                OnPenButtonClick(this, e);
            }
            if (e.Key == Key.Z 
                && Keyboard.IsKeyDown(Key.LeftCtrl)
                && currentMode == AppMode.Draw)
            {
                if (Keyboard.IsKeyDown(Key.LeftShift)) paint_area.Redo();
                else paint_area.Undo();
            }
            if (e.Key == Key.S
                && Keyboard.IsKeyDown(Key.LeftCtrl)
                && currentMode == AppMode.Text)
            {
                if (Keyboard.IsKeyDown(Key.LeftShift)) btn_TextSaveAs_Click(this, null);
                else btn_TextSave_Click(this, null);
            }
            if (e.Key == Key.P
                && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                btn_DisplayPanel_Click(this, null);
            }
            if (e.Key == Key.T
                && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                btn_top_Click(this, null);
            }
            if (e.Key == Key.N
                && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                btn_note_Click_1(this, null);
            }
            if (e.Key == Key.Add
                && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                byte new_value = Convert.ToByte(Math.Clamp(slider_winOpacity.Value + 10, 0, 255));
                SetWindowOpacity(new_value, true);
            }
            if (e.Key == Key.Subtract
                && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                byte new_value = Convert.ToByte(Math.Clamp(slider_winOpacity.Value - 10, 0, 255));
                SetWindowOpacity(new_value, true);
            }
            if (e.Key == Key.D0
                && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                byte new_value = 0;
                SetWindowOpacity(new_value, true);
            }
            if (e.Key == Key.D1
                && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                byte new_value = 1;
                SetWindowOpacity(new_value, true);
            }
            if (e.Key == Key.D2
                && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                byte new_value = 44;
                SetWindowOpacity(new_value, true);
            }
            if (e.Key == Key.D3
                && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                byte new_value = 60;
                SetWindowOpacity(new_value, true);
            }
            if (e.Key == Key.D4
                && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                byte new_value = 80;
                SetWindowOpacity(new_value, true);
            }
            if (e.Key == Key.D5
                && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                byte new_value = 100;
                SetWindowOpacity(new_value, true);
            }
            if (e.Key == Key.D6
                && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                byte new_value = 130;
                SetWindowOpacity(new_value, true);
            }
            if (e.Key == Key.D7
                && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                byte new_value = 160;
                SetWindowOpacity(new_value, true);
            }
            if (e.Key == Key.D8
                && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                byte new_value = 200;
                SetWindowOpacity(new_value, true);
            }
            if (e.Key == Key.D9
                && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                byte new_value = 0xff;
                SetWindowOpacity(new_value, true);
            }
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl)
            {
                if (!crtl_pressed)
                {
                    tbox_mainText.PreviewMouseLeftButtonDown += ShiftMove;
                    Console.WriteLine("ACTIVE");
                    crtl_pressed = true;
                }
            }
        }
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl)
            {
                if (crtl_pressed)
                {
                    tbox_mainText.PreviewMouseLeftButtonDown -= ShiftMove;
                    Console.WriteLine("DESACTIVE");
                    crtl_pressed = false;
                }
            }
        }
        private void ShiftMove(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                if (!Keyboard.IsKeyDown(Key.LeftCtrl))
                {
                    tbox_mainText.PreviewMouseLeftButtonDown -= ShiftMove;
                    Console.WriteLine("DESACTIVE");
                    crtl_pressed = false;
                }
                this.DragMove();
            }
        }
        private void On_WinDrop(object sender, DragEventArgs e)
        {
            var addedSize = Width - 800;
            Point drop_pos = e.GetPosition(this);

            if (drop_pos.X > 618/*670 - 460*/ + addedSize)
            {
                drop_pos.X = 618 + addedSize;
            }

            //if (drop_pos.X < 460 + addedSize)
            //{
            //    drop_pos.X = 460 + addedSize;
            //}

            display_panel.Margin = new Thickness(
                drop_pos.X - 7,
                display_panel.Margin.Top,
                display_panel.Margin.Right,
                display_panel.Margin.Bottom);

            panel.Margin = new Thickness(
                drop_pos.X,
                panel.Margin.Top,
                panel.Margin.Right,
                panel.Margin.Bottom);

            tbox_mainText.Margin = new Thickness(
                tbox_mainText.Margin.Left,
                tbox_mainText.Margin.Top,
                -drop_pos.X,
                tbox_mainText.Margin.Bottom);
        }
        private void OnWinResize(object sender, SizeChangedEventArgs e)
        {
            var addedSize = Width - 800;
            
            if (panel.Margin.Left > 670 + addedSize)
            {
                panel.Margin = new Thickness(
                    670 + addedSize,
                    panel.Margin.Top,
                    panel.Margin.Right,
                    panel.Margin.Bottom);
            }
            if (display_panel.Margin.Left > 665 + addedSize)
            {
                display_panel.Margin = new Thickness(
                    665 + addedSize, 
                    display_panel.Margin.Top,
                    display_panel.Margin.Right,
                    display_panel.Margin.Bottom);
            }

            //if (panel.Margin.Left < 480 + addedSize)
            //{
            //    panel.Margin = new Thickness(
            //        460 + addedSize,
            //        panel.Margin.Top,
            //        panel.Margin.Right,
            //        panel.Margin.Bottom);
            //}
            //if (display_panel.Margin.Left < 473 + addedSize)
            //{
            //    display_panel.Margin = new Thickness(
            //        455 + addedSize,
            //        display_panel.Margin.Top,
            //        display_panel.Margin.Right,
            //        display_panel.Margin.Bottom);
            //}

            if (display_panel.Margin.Left > this.Width - 25)
            {
                display_panel.Margin = new Thickness(
                    this.Width - 7,
                    display_panel.Margin.Top,
                    display_panel.Margin.Right,
                    display_panel.Margin.Bottom);

                panel.Margin = new Thickness(
                    this.Width - 20,
                    panel.Margin.Top,
                    panel.Margin.Right,
                    panel.Margin.Bottom);
            }
        }
        private void Resizer_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(panel, panel, DragDropEffects.None);
            }
        }
        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
        private void btn_transparent_Click(object sender, RoutedEventArgs e)
        {
            EnableWindowTransparent(!win_transparent);
        }
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            byte value = Convert.ToByte(e.NewValue);
            SetWindowOpacity(value, false);
        }
        private void Border_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(panel, panel, DragDropEffects.None);
            }

        }
        private void btn_quit_Click(object sender, RoutedEventArgs e)
        {
            Manager.QuitApp();
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
        private void btn_DisplayPanel_Click(object sender, RoutedEventArgs e)
        {
            DisplayPanel(!panel_displaying);

        }

        private void btn_option_Click(object sender, RoutedEventArgs e)
        {
            OpenOptions();
        }


        private void btn_option_Click_1(object sender, RoutedEventArgs e)
        {
            OpenOptions();
        }

        private void btn_switchMode_Click(object sender, RoutedEventArgs e)
        {
            SetMode(CurrentMode == AppMode.Text ? AppMode.Draw : AppMode.Text);
        }

        private void OnPanelTextInit(object sender, EventArgs e)
        {
            
        }
        private void cmbbox_Panels_FontSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (fontBox_initalized)
            {
                Manager.SetDefaultTypeFont((FontFamily)((sender as ComboBox).SelectedItem), true);
            }
            
        }








        #endregion

        private void BrushSizeChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            paint_area.Radius = e.NewValue;
        }

        private void EraserSizeChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            paint_area.EraseRadius = e.NewValue;
        }

        private void ColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            paint_area.CurrentColor = e.NewValue.Value;
        }

        private void OnPenButtonClick(object sender, RoutedEventArgs e)
        {
            currentBrush = PaintCanvas.PaintBrush.Defautl;
            paint_area.SelectedBrush = currentBrush;
            UpdateBrushButtons();
        }

        private void OnEraserBtn_Click(object sender, RoutedEventArgs e)
        {
            currentBrush = PaintCanvas.PaintBrush.Eraser;
            paint_area.SelectedBrush = currentBrush;
            UpdateBrushButtons();
        }

        public void UpdateBrushButtons()
        {
            switch (currentBrush)
            {
                case PaintCanvas.PaintBrush.Defautl:
                    SetBrushButtonActive(btn_panel_draw_NormalBrush, true);
                    SetBrushButtonActive(btn_panel_draw_EraseBrush, false);
                    break;
                case PaintCanvas.PaintBrush.Eraser:
                    SetBrushButtonActive(btn_panel_draw_NormalBrush, false);
                    SetBrushButtonActive(btn_panel_draw_EraseBrush, true);
                    break;
                default:
                    break;
            }
        }
        private void SetBrushButtonActive(Button btn, bool value)
        {
            if (value)
            {
                btn.BorderBrush = Brush_Button_Active;
                var icon = Manager.FindVisualChilds<FontAwesome.WPF.ImageAwesome>(btn);
                icon.FirstOrDefault().Foreground = Brush_Button_Active;
            }
            else
            {
                btn.BorderBrush = Brush_Button_Disable;
                var icon = Manager.FindVisualChilds<FontAwesome.WPF.ImageAwesome>(btn);
                icon.FirstOrDefault().Foreground = Brush_Button_Disable;
            }
        }

        private void On_BtnClear_Click(object sender, RoutedEventArgs e)
        {
            paint_area.Clear();
        }

        private void OnTextColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            tbox_mainText.Foreground = new SolidColorBrush(e.NewValue.Value);
        }

        private void btn_ViewInExplorerClick(object sender, RoutedEventArgs e)
        {
            if (fileSaved)
            {
                string path = System.IO.Path.GetDirectoryName(currentTextDocPath);
                
                if (Directory.Exists(path))
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        Arguments = path,
                        FileName = "explorer.exe"
                    };

                    Process.Start(startInfo);
                }
            }
        }

        private void btn_TextSaveAs_Click(object sender, RoutedEventArgs e)
        {
            //SaveFileDialog dialog = new SaveFileDialog();
            SaveFileDialog dialog = new SaveFileDialog
            {
                InitialDirectory = Manager.LastTextFileSaveDirectory,
                Title = "Save text to file",

                CheckFileExists = false,
                CheckPathExists = true,

                DefaultExt = "txt",
                Filter = "Text Files(*.txt)|*.txt|All(*.*)|*.tntxt|transparent notpad file(*.tntxt*)|*",
                FilterIndex = 2,
                RestoreDirectory = true
            };

            if (dialog.ShowDialog() == true)
            {
                File.WriteAllText(dialog.FileName, tbox_mainText.Text);

                currentTextDocPath = dialog.FileName;
                Manager.SetLastSaveEmplacement(currentTextDocPath);
                fileSaved = true;
            }            
        }

        private void btn_TextSave_Click(object sender, RoutedEventArgs e)
        {
            if (!fileSaved)
            {
                btn_TextSaveAs_Click(this, e);
            }
            else
            {
                if (File.Exists(currentTextDocPath))
                {
                    File.WriteAllText(currentTextDocPath, tbox_mainText.Text);
                }
                else
                {
                    btn_TextSaveAs_Click(this, e);
                }
            }
        }

        private void btn_OpenTextDoc_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                InitialDirectory = Manager.LastTextFileSaveDirectory,
                Title = "Open Text File",

                CheckFileExists = true,
                CheckPathExists = true,

                RestoreDirectory = true
            };

            if (dialog.ShowDialog() == true)
            {
                Manager.SaveCurrentTextInTemp();
                tbox_mainText.Text = File.ReadAllText(dialog.FileName);

                currentTextDocPath = dialog.FileName;
                fileSaved = true;
            }
        }

        public void btn_note_Click_1(object sender, RoutedEventArgs e)
        {
            Postit p = new Postit();
            p.Show();
        }

        private void display_panel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void tbox_mainText_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void display_panel_MouseUp(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void panel_DragLeave(object sender, DragEventArgs e)
        {
            
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void On_DropOnWindow(object sender, DragEventArgs e)
        {
            On_WinDrop(sender, e);
        }
    }
}
