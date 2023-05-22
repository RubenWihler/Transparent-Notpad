using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using TransparentNotePad.SaveSystem;

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
            tbox_tempTextPath.Text = SaveManager.TemporaryTextDirectoryPath;
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

        private bool SaveOptions(out string error)
        {
            var error_msg_builder = new StringBuilder();
            var selected_theme = ((ThemeBtn)cmbbox_Theme.SelectedItem).theme;
            var selected_temp_path = tbox_tempTextPath.Text;
            
            //saving theme
            if (!ThemeManager.SetSelectedTheme(selected_theme, true))
            {
                error_msg_builder.Append("Unable to save selected theme!\r\n");
            }
            
            //saving temporary text file path
            if (!Directory.Exists(selected_temp_path) || !OptionsManager.SetTemporaryTextFileSaveEmplacement(selected_temp_path))
            {
                error_msg_builder.Append("The location of the temporary files is invalid!\r\n");
            }
            
            error = error_msg_builder.ToString();
            return error.Length == 0;
        }
        
        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            if (!SaveOptions(out var error_msg))
            {
                MessageBox.Show(this,
                    error_msg,
                    "Error while applying settings",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            
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
            //preview selected theme
            if (!theme_initialized) return;
            var theme = ((ThemeBtn)cmbbox_Theme.SelectedItem).theme;
            ThemeManager.LoadTheme(theme);
        }

        private void tbox_tempTextPath_TextChanged(object sender, TextChangedEventArgs e)
        {
            // OptionsManager.SetTemporaryTextFileSaveEmplacement(tbox_tempTextPath.Text);
        }
    }
}
