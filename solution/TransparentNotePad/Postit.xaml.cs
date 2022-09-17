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
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace TransparentNotePad
{
    /// <summary>
    /// Logique d'interaction pour Postit.xaml
    /// </summary>
    public partial class Postit : Window
    {
        private bool inOption;

        private Theme? currentTheme;

        DispatcherTimer dispt_anim_ToOption;
        DispatcherTimer dispt_anim_ToHeader;

        private RowDefinition rowAnim1;

        private bool fileSaved = false;
        private string currentTextDocPath;
        private bool isTop = true;

        public Postit()
        {
            InitializeComponent();
            Init();
        }
        public void SetTheme(Theme theme)
        {
            this.currentTheme = theme;
        }

        private void Init()
        {
            this.Topmost = true;
            rowAnim1 = baseGrid.RowDefinitions[0];

            dispt_anim_ToOption = new DispatcherTimer();
            dispt_anim_ToHeader = new DispatcherTimer();

            dispt_anim_ToOption.Interval = TimeSpan.FromMilliseconds(5);
            dispt_anim_ToHeader.Interval = TimeSpan.FromMilliseconds(5);

            dispt_anim_ToOption.Tick += Anim_ToOptions;
            dispt_anim_ToHeader.Tick += Anim_ToHeader;

            SetTheme(Manager.CurrentTheme);
        }

        private void OpenOption(bool value)
        {
            if (value)
            {
                Options.Opacity = 100;
                Header.Opacity = 0;
                Options.IsHitTestVisible = true;
                Header.IsHitTestVisible = false;
                GridLength gridLength = new GridLength(300, GridUnitType.Star);
                rowAnim1.Height = gridLength;
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
                //StartAnimateOpenHeader();
            }
            
        }

        private void StartAnimateOpenOption()
        {
            dispt_anim_ToHeader.Stop();
            dispt_anim_ToOption.Start();
        }
        private void StopAnimateOpenOption()
        {
            dispt_anim_ToOption.Stop();
        }
        private void StartAnimateOpenHeader()
        {
            dispt_anim_ToOption.Stop();
            dispt_anim_ToHeader.Start();
        }
        private void StopAnimateOpenHeader()
        {
            dispt_anim_ToHeader.Stop();
        }

        private void Anim_ToOptions(object? sender, EventArgs args)
        {
            GridLength gridLength = new GridLength(300, GridUnitType.Star);

            if (rowAnim1.Height.Value >= gridLength.Value)
            {
                rowAnim1.Height = gridLength;
                Options.Opacity = 100;
                Header.Opacity = 0;
                Options.IsHitTestVisible = true;
                Header.IsHitTestVisible = false;
                StopAnimateOpenOption();
            }
            else
            {
                GridLength g = new GridLength(rowAnim1.Height.Value + 45, GridUnitType.Star);
                rowAnim1.Height = g;
                //Options.Opacity += 5;
                //Header.Opacity -= 5;
            }
        }
        private void Anim_ToHeader(object? sender, EventArgs args)
        {
            GridLength gridLength = new GridLength(37, GridUnitType.Star);

            if (rowAnim1.Height.Value <= gridLength.Value)
            {
                Options.Opacity = 0;
                Header.Opacity = 100;
                rowAnim1.Height = gridLength;
                Options.IsHitTestVisible = false;
                Header.IsHitTestVisible = true;
                StopAnimateOpenOption();
            }
            else
            {
                GridLength g = new GridLength(rowAnim1.Height.Value - 10, GridUnitType.Star);
                rowAnim1.Height = g;
                //Options.Opacity -= 5;
                //Header.Opacity += 5;
            }
        }

        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
        private void Options_Btn_Back_Click(object sender, RoutedEventArgs e)
        {
            OpenOption(false);
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
            this.Close();
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

        private void Options_Btn_Top_Click(object sender, RoutedEventArgs e)
        {
            if (isTop)
            {
                this.Topmost = false;
                this.isTop = false;
                Options_Btn_Top.Content = "Top: X";
            }
            else
            {
                this.Topmost = true;
                this.isTop = true;
                Options_Btn_Top.Content = "Top: ✓";
            }
        }

        private void Options_slider_winOpacity_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (border_main != null && currentTheme != null)
            {
                Color color = Manager.GetColorFromThemeFileString(currentTheme.Value.Color_TextArea);

                border_main.Background = new SolidColorBrush(Color.FromArgb(
                    Convert.ToByte(Options_slider_winOpacity.Value),
                    color.R,
                    color.G,
                    color.B));
            }
        }
    }
}
