using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

            foreach (Theme theme in ThemeManager.AllThemesFiles)
            {
                CreateThemeButton(theme);
            }

            theme_initialized = true;
        }
        private void CreateThemeButton(Theme theme)
        {
            ThemeBtn theme_button = new ThemeBtn();
            theme_button.theme = theme;
            theme_button.Style = ThemeButtonStyle;
            theme_button.Content = theme.ThemeName;

            cmbbox_Theme.Items.Add(theme_button);

            if (theme.ThemeName == ThemeManager.CurrentTheme.ThemeName)
            {
                cmbbox_Theme.SelectedItem = theme_button;
            }
                
            theme_initialized = true;
        }

        public void OnOpen()
        {
            Init_Theme();
            Topmost = Manager.InstanceOfMainWindow.Topmost;
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
                var theme = ((ThemeBtn)cmbbox_Theme.SelectedItem).theme;
                ThemeManager.SetSelectedTheme(theme, true);
            }
        }
    }
}
