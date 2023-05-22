using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TransparentNotePad.SaveSystem;

namespace TransparentNotePad
{
    /// <summary>
    /// Logique d'interaction pour Postit.xaml
    /// </summary>
    public partial class Postit : Window
    {
        private bool inOption;

        private RowDefinition rowAnim1;

        private bool fileSaved = false;
        private string currentTextDocPath;
        private bool isTop = true;

        public Postit()
        {
            InitializeComponent();
            Init();
        }
        
        private void Init()
        {
            this.Topmost = true;
            rowAnim1 = baseGrid.RowDefinitions[0];
            ThemeManager.LoadTheme(ThemeManager.CurrentTheme);
            Manager.NotesInstances.Add(this);
            RefreshDefaultValue();
        }

        public void RefreshDefaultValue()
        {
            var option_file = OptionsManager.CurrentOptionFile;
            byte opacity = Convert.ToByte(option_file.NoteWin_Default_WindowOpacity);

            SetTextFont(option_file.NoteWin_Default_Font);
            SetWindowOpacity(opacity);
        }
        public void SetWindowOpacity(byte opacity, bool setValueInSlider = true)
        {
            if (border_main != null)
            {
                Color color = ThemeManager.CurrentTheme.NoteEditorBackgroundColor.ToColor();
                Color new_color = Color.FromArgb(opacity, color.R, color.G, color.B);
                border_main.Background = new SolidColorBrush(new_color);

                if (setValueInSlider) Options_slider_winOpacity.Value = opacity;
            }
        }
        public void SetTextFont(string fontName)
        {
            FontFamily font = new FontFamily(fontName);
            if (font != null) cmbbox_Panels_FontSelector.SelectedItem = font;
        }

        private void OpenOption(bool value)
        {
            if (value)
            {
                GridLength gridLength =
                    new GridLength(300, GridUnitType.Star);

                Options.Opacity = 100;
                Header.Opacity = 0;
                Options.IsHitTestVisible = true;
                Header.IsHitTestVisible = false;
                rowAnim1.Height = gridLength;
                rowAnim1.MinHeight = 150;
                rowAnim1.MaxHeight = 225;
                //StartAnimateOpenOption();
            }
            else
            {
                Options.Opacity = 0;
                Header.Opacity = 100;
                Options.IsHitTestVisible = false;
                Header.IsHitTestVisible = true;
                GridLength gridLength = new GridLength(37, GridUnitType.Star);
                rowAnim1.Height = gridLength;
                rowAnim1.MinHeight = 38;
                rowAnim1.MaxHeight = 42;
                //StartAnimateOpenHeader();
            }

            inOption = value;
        }

        #region Save

        private void SaveAs()
        {
            if (SaveManager.TrySaveTextFileAs(tbox_mainText.Text, out SaveFileDialog dialog))
            {
                currentTextDocPath = dialog.FileName;
                OptionsManager.SetFileSaveEmplacement(currentTextDocPath);
                fileSaved = true;
            }
        }
        private void Save()
        {
            if (!fileSaved || !File.Exists(currentTextDocPath))
            {
                SaveAs();
                return;
            }

            File.WriteAllText(currentTextDocPath, tbox_mainText.Text);
        }

        #endregion

        private void Zoom(bool up)
        {
            if (up)
            {
                if (tbox_mainText.FontSize < 150)
                {
                    tbox_mainText.FontSize++;
                }
            }
            else
            {
                if (tbox_mainText.FontSize > 1)
                {
                    tbox_mainText.FontSize--;
                }
            }

        }

        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void Header_btn_Save_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }
        private void Header_btn_Options_Click(object sender, RoutedEventArgs e)
        {
            OpenOption(true);
        }
        private void Header_btn_Quit_Click(object sender, RoutedEventArgs e)
        {
            Manager.NotesInstances.Remove(this);
            this.Close();
        }
        private void Header_btn_Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Options_Btn_Back_Click(object sender, RoutedEventArgs e)
        {
            OpenOption(false);
        }
        private void option_colorPicker_textColor_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (tbox_mainText != null)
            {
                Color color = option_colorPicker_textColor.SelectedColor != null ?
                option_colorPicker_textColor.SelectedColor.Value
                : Color.FromArgb(0xff, 0xff, 0x00, 0x00);

                tbox_mainText.Foreground = new SolidColorBrush(color);
            }
        }
        private void cmbbox_Panels_FontSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tbox_mainText != null)
            {
                tbox_mainText.FontFamily = cmbbox_Panels_FontSelector.SelectedItem as FontFamily;
            }
        }
        private void Options_Btn_Top_Click(object sender, RoutedEventArgs e)
        {
            if (isTop)
            {
                this.Topmost = false;
                this.isTop = false;
                Options_lblOfBtn_Top.Content = "Top: X";
            }
            else
            {
                this.Topmost = true;
                this.isTop = true;
                Options_lblOfBtn_Top.Content = "Top: ✓";
            }
        }
        private void Options_slider_winOpacity_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SetWindowOpacity(
                Convert.ToByte(Options_slider_winOpacity.Value),
                false);
        }
        private void Options_Btn_SetDefault_Click(object sender, RoutedEventArgs e)
        {
            var option_file = OptionsManager.CurrentOptionFile;
            var font_name = ((FontFamily)cmbbox_Panels_FontSelector.SelectedItem).Source;
            var win_opacity = Convert.ToInt32(Options_slider_winOpacity.Value);

            option_file.NoteWin_Default_Font = font_name;
            option_file.NoteWin_Default_WindowOpacity = win_opacity;

            OptionsManager.SaveOptions(option_file);
        }

        private void Header_Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                return;
            }
        }
        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl)) Zoom(e.Delta > 0);
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                if (e.Key == Key.O) OpenOption(!inOption);
                if (e.Key == Key.T) Options_Btn_Top_Click(this, null);
                if (e.Key == Key.N) Manager.InstanceOfMainWindow.btn_note_Click_1(this, null);

                if (e.Key == Key.Add)
                {
                    byte new_value = Convert.ToByte(Math.Clamp(Options_slider_winOpacity.Value + 10, 0, 255));
                    SetWindowOpacity(new_value, true);
                }
                if (e.Key == Key.Subtract)
                {
                    byte new_value = Convert.ToByte(Math.Clamp(Options_slider_winOpacity.Value - 10, 0, 255));
                    SetWindowOpacity(new_value, true);
                }

                if (e.Key == Key.S)
                {
                    if (Keyboard.IsKeyDown(Key.LeftShift)) SaveAs();
                    else Save();
                }

                e.Handled = true;
            }
        }
    }
}
