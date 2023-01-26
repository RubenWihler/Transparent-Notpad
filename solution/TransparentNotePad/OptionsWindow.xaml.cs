using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace TransparentNotePad
{
    /// <summary>
    /// Logique d'interaction pour Window1.xaml
    /// </summary>
    public partial class OptionWindow : Window
    {
        private bool theme_initialized = false;

        private Style ThemeButtonStyle
        {
            get
            {
                return Application.Current.Resources["Theme_Button"] as Style;
            }
        }
        public OptionWindow()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            Init_Theme();
        }
        private void Init_Theme()
        {
            theme_initialized = false;
            cmbbox_Theme.Items.Clear();

            if (Manager.TryGetThemeFromXMLFile(out List<ThemeOLD> tlist))
            {
                foreach (ThemeOLD t in tlist)
                {
                    CreateThemeButton(t);
                }
            }

            theme_initialized = true;
        }
        private void CreateThemeButton(ThemeOLD theme)
        {
            ThemeBtn btn = new ThemeBtn();
            btn.theme = theme;
            btn.Style = ThemeButtonStyle;
            btn.Content = theme.Theme_Name;

            cmbbox_Theme.Items.Add(btn);

            if (theme.Theme_Name == Manager.CurrentThemeName)
            {
                Console.WriteLine($"CURRENT THEME FOUNDED: {theme.Theme_Name}");
                cmbbox_Theme.SelectedItem = btn;
            }
                
            theme_initialized = true;
        }

        public void OnOpen()
        {
            Init_Theme();
            Topmost = Manager.MainWindow.Topmost;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            Manager.TryCloseWindow<OptionWindow>(this);
        }

        private void cmbbox_Theme_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            
        }

        private void cmbbox_Theme_ContextMenuClosing(object sender, ContextMenuEventArgs e)
        {
            
        }

        private void cmbbox_Theme_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (theme_initialized)
            {
                Manager.SetTheme((cmbbox_Theme.SelectedItem as ThemeBtn).theme, true);
            }
        }
    }
}
