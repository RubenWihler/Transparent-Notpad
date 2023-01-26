using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Xceed.Wpf.AvalonDock.Themes;
using FontAwesome;

namespace TransparentNotePad
{
    /// <summary>
    /// Logique d'interaction pour Postit.xaml
    /// </summary>
    public partial class Postit : Window
    {
        private bool inOption;

        private ThemeOLD? currentTheme;

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

            Manager.Instance.onThemeChanged += this.SetTheme;

            SetTheme(Manager.CurrentTheme);
            RefreshDefaultValue();
        }

        public void UpdateTheme()
        {
            this.currentTheme = Manager.CurrentTheme;
            
            Manager.TryGetObjectFromResource(this, "Header_Button", out Style? baseBtnStyle);
            Manager.TryGetObjectFromResource(this, "Header_Btn_Text", out Style? baseBtnstextStyle);
            Manager.TryGetObjectFromResource(this, "Options_lbl", out Style? baseLabeltextStyle);
            Manager.TryGetObjectFromResource(this, "Header_Button_Icon", out Style? baseIconStyle);

            SolidColorBrush textColor = new SolidColorBrush(Manager.GetColorFromThemeFileString(currentTheme.Value.Color_Text_Panel_Btns_Text));
            SolidColorBrush panelColor = new SolidColorBrush(Manager.GetColorFromThemeFileString(currentTheme.Value.Color_Panel));

            SetterBase BtnBgColor = new Setter(Button.BackgroundProperty,
                new SolidColorBrush(Manager.GetColorFromThemeFileString(currentTheme.Value.Color_Text_Panel_Btns_Bg)));
            SetterBase SetterTextColor = new Setter(Label.ForegroundProperty, textColor);
            SetterBase iconColor = new Setter(FontAwesome.WPF.ImageAwesome.ForegroundProperty,
                new SolidColorBrush(Manager.GetColorFromThemeFileString(currentTheme.Value.Color_Text_Panel_Btns_Text)));

            Style buttonStyle = new Style(typeof(Button), baseBtnStyle);
            buttonStyle.Setters.Add(BtnBgColor);

            Style btnTextStyle = new Style(typeof(Label), baseBtnstextStyle);
            btnTextStyle.Setters.Add(SetterTextColor);

            Style lblTextStyle = new Style(typeof(Label), baseLabeltextStyle);
            lblTextStyle.Setters.Add(SetterTextColor);

            Style IconStyle = new Style(typeof(FontAwesome.WPF.ImageAwesome), baseIconStyle);
            IconStyle.Setters.Add(iconColor);


            IEnumerable<Button> headerBtns =
                Manager.FindVisualChilds<Button>(Header_Border)
                .Where(x => x.Tag != null && x.Tag.ToString() == "header_btn");

            IEnumerable<Label> btnsTexts =
                Manager.FindVisualChilds<Label>(Header_Border)
                .Where(x => x.Tag != null && x.Tag.ToString() == "btn_text");

            IEnumerable<Label> lblTexts =
                Manager.FindVisualChilds<Label>(Header_Border)
                .Where(x => x.Tag != null && x.Tag.ToString() == "lbl_text");

            IEnumerable<FontAwesome.WPF.ImageAwesome> Icon =
                Manager.FindVisualChilds<FontAwesome.WPF.ImageAwesome>(Header_Border)
                .Where(x => x.Tag != null && x.Tag.ToString() == "header_button_icon");

            foreach (var item in headerBtns)
            {
                Console.WriteLine($"headerBtns : {item.Tag}");
                item.Style = buttonStyle;
            }
            foreach (var item in btnsTexts)
            {
                Console.WriteLine($"btnsTexts : {item.Tag}");
                item.Style = btnTextStyle;
            }
            foreach (var item in lblTexts)
            {
                Console.WriteLine($"lblTexts : {item.Tag}");
                item.Style = lblTextStyle;
            }
            foreach (var item in Icon)
            {
                Console.WriteLine($"Icon : {item.Tag}");
                item.Style = IconStyle;
            }

            Header_Border.Background = panelColor;

            tbox_mainText.Foreground = textColor;
            lbl_Title.Foreground = textColor;
            lbl_Options_Title.Foreground = textColor;

            header_btn_Save_Icon.Foreground = textColor;
            header_btn_Minimize_Icon.Foreground = textColor;
            header_btn_Quit_Icon.Foreground = textColor;

            Header_btn_Option_text.Foreground = textColor;
            Options_Btn_Back_text.Foreground = textColor;
            Options_lblOfBtn_Top.Foreground = textColor;
            Options_Btn_SetDefault_Text.Foreground = textColor;

            SetWindowOpacity(Convert.ToByte(Options_slider_winOpacity.Value));
        }
        public void SetTheme(ThemeOLD theme)
        {
            this.currentTheme = theme;
            UpdateTheme();
        }
        public void RefreshDefaultValue()
        {
            if (Manager.TryGetStoredFile(out StoredDataFile storedFile))
            {
                byte opacity = Convert.ToByte(storedFile.NoteWin_Default_WindowOpacity);

                SetTextFont(storedFile.NoteWin_Default_Font);
                SetWindowOpacity(opacity);
            }
        }
        public void SetWindowOpacity(byte opacity, bool setValueInSlider = true)
        {
            if (border_main != null && currentTheme != null)
            {
                Color color = 
                    Manager.GetColorFromThemeFileString(currentTheme.Value.Color_TextArea);

                border_main.Background = new SolidColorBrush(Color.FromArgb(
                    opacity,
                    color.R,
                    color.G,
                    color.B));

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
        private void SaveAs()
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
        private void Save()
        {
            if (!fileSaved)
            {
                SaveAs();
            }
            else
            {
                if (File.Exists(currentTextDocPath))
                {
                    File.WriteAllText(currentTextDocPath, tbox_mainText.Text);
                }
                else
                {
                    SaveAs();
                }
            }
        }
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
            Manager.Instance.onThemeChanged -= SetTheme;
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
            if (Manager.TryGetStoredFile(out StoredDataFile storedFile))
            {
                FontFamily font = (FontFamily)cmbbox_Panels_FontSelector.SelectedItem;
                int win_opacity = Convert.ToInt32(Options_slider_winOpacity.Value);
                
                storedFile.NoteWin_Default_Font = font.Source;
                storedFile.NoteWin_Default_WindowOpacity = win_opacity;
                
                Manager.SaveStoredData(storedFile);
            }

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
                if (e.Key == Key.N) Manager.MainWindow.btn_note_Click_1(this, null);

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
            }
        }
    }
}
