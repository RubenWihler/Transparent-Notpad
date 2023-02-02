using FontAwesome.WPF;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using TransparentNotePad.SaveSystem;

using Brush = System.Windows.Media.Brush;
using Color = System.Windows.Media.Color;
using FontFamily = System.Windows.Media.FontFamily;
using Point = System.Windows.Point;

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
            Draw,
            DesktopMode
        }
        public enum DMTools
        {
            Pen,
            Eraser,
            Cursor,
            Text,
            RectBorder,
            RectFill,
            CircleBorder,
            CircleFill,
            Arrow,
            Line
        }

        /*---------- Fields ----------*/
        public static event Action onWindowResize;
        private static CancellationTokenSource? _zoomTimerToken;

        private AppMode currentMode = AppMode.Text;
        private PaintCanvas.PaintBrush currentBrush;

        private bool win_transparent = true;
        private bool canFullTransparent = true;
        private bool panel_displaying = true;
        private bool isTop = true;
        private bool fontBox_initalized = false;
        private bool crtl_pressed = false;
        private bool isResizePanel = false;

        private byte lastWinOpacityBeforeDM = 0xff;
        private byte lastWinOppacity = 0x4a;

        private int current_zoom = 20;

        private bool fileSaved = false;
        private string currentTextDocPath;
        
        DispatcherTimer dispatcherTimer;
        DispatcherTimer screenShotTimer;
        DispatcherTimer dmp_snappingTimer;
        private List<Task> zoomTimers = new List<Task>();

        private DataObject dmp_drag_data;
        private DataObject panel_drag_data;

        //Desktop Mode (DM)
        private AppMode lastAppModeBeforeCurrent;
        private DMTools currentDMTools = DMTools.Cursor;
        private WindowState lastWinStateBeforeDM = WindowState.Normal;
        private ImageAwesome currentDMTool_Icon;

        private bool dmp_extended = false;
        
        

        #region /*------------- Proprety --------------*/

        public Style Panel_Buttons_Style
        {
            get
            {
                object o = Resources["PanelButton"];
                if (o.GetType() == typeof(Style)) return o as Style;
                return null!;
            }
        }
        public Style Panel_ButtonsTextBloc_Style
        {
            get
            {
                object o = Resources["PanelButtonText"];
                if (o.GetType() == typeof(Style)) return (Style)o;
                return null!;
            }
        }
        
        public Color PanelColor
        {
            get
            {
                return ThemeManager.CurrentTheme.PanelBackgroundColor.ToColor();
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
        public bool FileSaved
        {
            get
            {
                return this.fileSaved;
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
                return ThemeManager.CurrentTheme.PanelBrushButtonIcon.Variants[1].ToBrush();
            }
        }
        private Brush Brush_Button_Disable
        {
            get
            {
                return ThemeManager.CurrentTheme.PanelBrushButtonIcon.Variants[0].ToBrush();
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
                System.Reflection.Assembly.GetEntryAssembly()!.Location,
                "TNTXTX (same as .txt files, but opens by default with Transparent Notpad)");

            Init_Field();
            Init_DesktopMode();
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
        private void Init_Field()
        {
            //text_area_color = Color.FromArgb(0xff, 0xff, 0xff, 0xff);
            slider_winOpacity.Value = lastWinOppacity;
            SetWindowOpacity(lastWinOppacity);
        }
        private void Init_DesktopMode()
        {
            brd_DesktopModePanel.onStartMoving += () =>
            {
                SetWindowOpacity(0x01, false);
            };
            brd_DesktopModePanel.onEndMoving += () =>
            {
                SetWindowOpacity(lastWinOppacity, false);
            };
        }
        private void RetardedCall(object? sender, EventArgs args)
        {
            //Console.WriteLine($"RetardedCall");
            Init_FontBox();

            slider_brush_size.Value = 7;
            dispatcherTimer.Tick -= RetardedCall;
            dispatcherTimer.Stop();
            dispatcherTimer = null!;
            DMP_slider_brushSize.Value = 7;
            DMP_slider_eraserSize.Value = 50;
            SetCurrentDMTool(DMTools.Pen);
            dm_PaintCanvas.SelectedBrush = PaintCanvas.PaintBrush.Pen;
            dm_PaintCanvas.CanPaint = true;

            Canvas.SetLeft(brd_DesktopModePanel, SystemParameters.PrimaryScreenWidth - 130);
            Canvas.SetTop(brd_DesktopModePanel, (SystemParameters.PrimaryScreenHeight / 2) - (brd_DesktopModePanel.Height / 2));
        }

        private void Init_FontBox()
        {
            var selected_font_name = OptionsManager.CurrentOptionFile.EditorFont;

            foreach (var font in cmbbox_Panels_FontSelector.Items)
            {
                var font_family = (FontFamily)font;

                if (font_family != null)
                {
                    if (font_family.Source == selected_font_name)
                    {
                        cmbbox_Panels_FontSelector.SelectedItem = font;
                        break;
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
            var theme_editor_bg_color = ThemeManager.CurrentTheme.EditorBackgroundColor.ToColor();
            
            brd_main.Background = new SolidColorBrush(Color.FromArgb(
                Math.Clamp((byte)alpha,(byte)(canFullTransparent ? 0x00 : 0x01),(byte)0xff),
                theme_editor_bg_color.R,
                theme_editor_bg_color.G,
                theme_editor_bg_color.B));

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
            if (up)
            {
                if (tbox_mainText.FontSize < 150) tbox_mainText.FontSize++;
            }
            else
            {
                if (tbox_mainText.FontSize > 1) tbox_mainText.FontSize--;
            }

            current_zoom = Convert.ToInt32(tbox_mainText.FontSize);
            StartZoomTimer();
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
            lastAppModeBeforeCurrent = currentMode;
            currentMode = mode;

            switch (mode)
            {
                case AppMode.Text:
                    DisableElementsOfDrawMode();
                    DisableElementsOfDesktopMode();
                    EnableElementsOfTextMode();
                    break;
                case AppMode.Draw:
                    DisableElementsOfTextMode();
                    DisableElementsOfDesktopMode();
                    EnableElementsOfDrawMode();
                    UpdateBrushButtons();
                    break;
                case AppMode.DesktopMode:
                    DisableElementsOfTextMode();
                    DisableElementsOfDrawMode();
                    EnableElementsOfDesktopMode();
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
        private void DisableElementsOfDesktopMode()
        {
            dm_PaintCanvas.IsHitTestVisible = false;
            dm_PaintCanvas.Opacity = 0;
            brd_DesktopModePanel.IsHitTestVisible = false;

            upBar.IsHitTestVisible = true;
            upBar.Opacity = 1;

            this.WindowState = lastWinStateBeforeDM;
            SetWindowOpacity(lastWinOpacityBeforeDM, true);
            DisplayPanel(true);
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
        private void EnableElementsOfDesktopMode()
        {
            brd_DesktopModePanel.IsHitTestVisible = true;
            dm_PaintCanvas.IsHitTestVisible = true;
            dm_PaintCanvas.Opacity = 1;

            upBar.IsHitTestVisible = false;
            upBar.Opacity = 0;

            lastWinStateBeforeDM = this.WindowState;
            lastWinOpacityBeforeDM = lastWinOppacity;

            this.WindowState = WindowState.Maximized;

            var middle_y = this.ActualHeight / 3.25;
            brd_DesktopModePanel.AddSnapPoint(new Point(50, middle_y), Key.Left);
            brd_DesktopModePanel.AddSnapPoint(new Point(this.ActualWidth - 120, middle_y), Key.Right);

            SetWindowOpacity(0x01, true);
            DisplayPanel(false);
            ResetAllDMToolsIconColor();
        }
        
        private void ResetAllDMToolsIconColor()
        {
            var theme = ThemeManager.CurrentTheme;
            var unselected_brush = theme.DesktopModePanelToolButtonIcon.Variants[0].ToBrush();
            var tools_icon = GetAllDMToolIcons();

            for (int i = 0; i < tools_icon.Length; i++)
            {
                tools_icon[i].Foreground = unselected_brush;
            }

            SetCurrentDMTool(currentDMTools);
        }
        private void ExportToPng(Canvas surface)
        {
            Rect bounds = VisualTreeHelper.GetDescendantBounds(surface);
            double dpi = 96d;

            RenderTargetBitmap rtb = new RenderTargetBitmap((int)bounds.Width, (int)bounds.Height, dpi, dpi, System.Windows.Media.PixelFormats.Default);

            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext dc = dv.RenderOpen())
            {
                VisualBrush vb = new VisualBrush(surface);
                dc.DrawRectangle(vb, null, new Rect(new Point(), bounds.Size));
            }

            rtb.Render(dv);

            /* Save image to file */
            SaveFileDialog dialog = new SaveFileDialog
            {
                InitialDirectory = OptionsManager.CurrentOptionFile.FileSavePath,
                Title = "Save to Image",

                CheckFileExists = false,
                CheckPathExists = true,

                DefaultExt = "txt",
                Filter = "Image file(*.png)|*.png",
                FilterIndex = 2,
                RestoreDirectory = true
            };

            if (dialog.ShowDialog() == true)
            {
                PngBitmapEncoder enc = new PngBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(rtb));

                using (FileStream stm = System.IO.File.Create(dialog.FileName))
                {
                    enc.Save(stm);
                }
            }
        }

        private void ExportScreenToPng()
        {
            brd_DesktopModePanel.Opacity = 0;

            screenShotTimer = new DispatcherTimer();
            screenShotTimer.Interval = TimeSpan.FromSeconds(0.2);
            screenShotTimer.Tick += ScreenTask;
            screenShotTimer.Start();
        }
        private void ScreenTask(object sender, EventArgs args)
        {
            screenShotTimer.Stop();
            Bitmap bitmap = new Bitmap((int)SystemParameters.PrimaryScreenWidth, (int)SystemParameters.PrimaryScreenHeight);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size);

            /* Save image to file */
            SaveFileDialog dialog = new SaveFileDialog
            {
                InitialDirectory = OptionsManager.CurrentOptionFile.FileSavePath,
                Title = "Save to Image",

                CheckFileExists = false,
                CheckPathExists = true,

                DefaultExt = "txt",
                //"Text Files(*.txt)|*.txt|All(*.*)|*.tntxt|transparent notpad file(*.tntxt*)|*"
                Filter = "PNG(*.png)|*.png|JPG(*.jpg)|*.jpg|BITMAP(*.bmp)|*.bmp|GIF(*.gif)|*.gif|EXIF(*.exf)|*.exf|TIFF(*.tif)|*.tif|EXIF(*.exf)|*.exf|WMF(*.wmf)|*.wmf",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            if (dialog.ShowDialog() == true)
            {
                System.Drawing.Imaging.ImageFormat format = System.Drawing.Imaging.ImageFormat.Png;

                switch (System.IO.Path.GetExtension(dialog.FileName))
                {
                    case "png":
                        format = System.Drawing.Imaging.ImageFormat.Png;
                        break;
                    case "jpeg":
                        format = System.Drawing.Imaging.ImageFormat.Jpeg;
                        break;
                    case "bpm":
                        format = System.Drawing.Imaging.ImageFormat.Bmp;
                        break;
                    case "gif":
                        format = System.Drawing.Imaging.ImageFormat.Gif;
                        break;
                    case "exf":
                        format = System.Drawing.Imaging.ImageFormat.Exif;
                        break;
                    case "tif":
                        format = System.Drawing.Imaging.ImageFormat.Tiff;
                        break;
                    case "wmf":
                        format = System.Drawing.Imaging.ImageFormat.Wmf;
                        break;
                    default:
                        break;
                }

                bitmap.Save(dialog.FileName, format);
            }

            if (currentMode == AppMode.DesktopMode)
                brd_DesktopModePanel.Opacity = 1;
        }

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
            else if (e.Key == Key.E && currentMode == AppMode.DesktopMode)
            {
                DMP_btn_Tools_Click(DMP_btn_EraseTool, e);
            }
            if (e.Key == Key.D && currentMode == AppMode.Draw)
            {
                OnPenButtonClick(this, e);
            }
            else if (e.Key == Key.D && currentMode == AppMode.DesktopMode)
            {
                DMP_btn_Tools_Click(DMP_btn_DrawTool, e);
            }
            if (e.Key == Key.Z 
                && Keyboard.IsKeyDown(Key.LeftCtrl)
                && currentMode == AppMode.Draw)
            {
                if (Keyboard.IsKeyDown(Key.LeftShift)) paint_area.Redo();
                else paint_area.Undo();
            }
            else if (e.Key == Key.Z
                && Keyboard.IsKeyDown(Key.LeftCtrl)
                && currentMode == AppMode.DesktopMode)
            {
                if (Keyboard.IsKeyDown(Key.LeftShift)) dm_PaintCanvas.Redo();
                else dm_PaintCanvas.Undo();
            }
            if (e.Key == Key.S
                && Keyboard.IsKeyDown(Key.LeftCtrl)
                && currentMode == AppMode.Text)
            {
                if (Keyboard.IsKeyDown(Key.LeftShift)) btn_TextSaveAs_Click(this, null);
                else btn_TextSave_Click(this, null);
            }
            else if (e.Key == Key.S
                && Keyboard.IsKeyDown(Key.LeftCtrl)
                && currentMode == AppMode.Draw)
            {
                ExportToPng(paint_area);
            }
            else if (e.Key == Key.S
                && Keyboard.IsKeyDown(Key.LeftCtrl)
                && currentMode == AppMode.DesktopMode)
            {
                ExportScreenToPng();
            }

            if (e.Key == Key.P
                && Keyboard.IsKeyDown(Key.LeftCtrl)
                && currentMode != AppMode.DesktopMode)
            {
                btn_DisplayPanel_Click(this, null);
            }
            if (e.Key == Key.T
                && Keyboard.IsKeyDown(Key.LeftCtrl)
                && currentMode != AppMode.DesktopMode)
            {
                btn_top_Click(this, null);
            }
            if (e.Key == Key.N
                && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                btn_note_Click_1(this, null);
            }
            if (e.Key == Key.Add
                && Keyboard.IsKeyDown(Key.LeftCtrl)
                && currentMode != AppMode.DesktopMode)
            {
                byte new_value = Convert.ToByte(Math.Clamp(slider_winOpacity.Value + 10, 0, 255));
                SetWindowOpacity(new_value, true);
            }
            if (e.Key == Key.Subtract
                && Keyboard.IsKeyDown(Key.LeftCtrl)
                && currentMode != AppMode.DesktopMode)
            {
                byte new_value = Convert.ToByte(Math.Clamp(slider_winOpacity.Value - 10, 0, 255));
                SetWindowOpacity(new_value, true);
            }
            if (e.Key == Key.D0
                && Keyboard.IsKeyDown(Key.LeftCtrl)
                && currentMode != AppMode.DesktopMode)
            {
                byte new_value = 0;
                SetWindowOpacity(new_value, true);
            }
            if (e.Key == Key.D1
                && Keyboard.IsKeyDown(Key.LeftCtrl)
                && currentMode != AppMode.DesktopMode)
            {
                byte new_value = 1;
                SetWindowOpacity(new_value, true);
            }
            if (e.Key == Key.D2
                && Keyboard.IsKeyDown(Key.LeftCtrl)
                && currentMode != AppMode.DesktopMode)
            {
                byte new_value = 44;
                SetWindowOpacity(new_value, true);
            }
            if (e.Key == Key.D3
                && Keyboard.IsKeyDown(Key.LeftCtrl)
                && currentMode != AppMode.DesktopMode)
            {
                byte new_value = 60;
                SetWindowOpacity(new_value, true);
            }
            if (e.Key == Key.D4
                && Keyboard.IsKeyDown(Key.LeftCtrl)
                && currentMode != AppMode.DesktopMode)
            {
                byte new_value = 80;
                SetWindowOpacity(new_value, true);
            }
            if (e.Key == Key.D5
                && Keyboard.IsKeyDown(Key.LeftCtrl)
                && currentMode != AppMode.DesktopMode)
            {
                byte new_value = 100;
                SetWindowOpacity(new_value, true);
            }
            if (e.Key == Key.D6
                && Keyboard.IsKeyDown(Key.LeftCtrl)
                && currentMode != AppMode.DesktopMode)
            {
                byte new_value = 130;
                SetWindowOpacity(new_value, true);
            }
            if (e.Key == Key.D7
                && Keyboard.IsKeyDown(Key.LeftCtrl)
                && currentMode != AppMode.DesktopMode)
            {
                byte new_value = 160;
                SetWindowOpacity(new_value, true);
            }
            if (e.Key == Key.D8
                && Keyboard.IsKeyDown(Key.LeftCtrl)
                && currentMode != AppMode.DesktopMode)
            {
                byte new_value = 200;
                SetWindowOpacity(new_value, true);
            }
            if (e.Key == Key.D9
                && Keyboard.IsKeyDown(Key.LeftCtrl)
                && currentMode != AppMode.DesktopMode)
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
            e.Handled = true;
            if (e.Data == panel_drag_data)
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

            if (e.Data == dmp_drag_data)
            {
                //if (!lastWinOpacityBeforeDMPFrag_Saved)
                //{
                //    lastWinOpacityBeforeDMPDrag = dm_PaintCanvas.Background;
                //    dm_PaintCanvas.Background = new SolidColorBrush(Color.FromArgb(1, 0, 0, 0));
                //    lastWinOpacityBeforeDMPFrag_Saved = true;
                //}

                Point drop_pos = e.GetPosition(this);
                Canvas.SetTop(brd_DesktopModePanel, drop_pos.Y - ((brd_DesktopModePanel.Height / 2)));
                Canvas.SetLeft(brd_DesktopModePanel, drop_pos.X - ((brd_DesktopModePanel.Width / 2)));
            }
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

            onWindowResize?.Invoke();
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

                display_panel.Margin = new Thickness(
                    1500 - 7,
                    display_panel.Margin.Top,
                    display_panel.Margin.Right,
                    display_panel.Margin.Bottom);

                panel.Margin = new Thickness(
                    1500,
                    panel.Margin.Top,
                    panel.Margin.Right,
                    panel.Margin.Bottom);
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
                var font = ((FontFamily)((ComboBox)sender).SelectedItem);
                var font_name = font.Source;
                OptionsManager.SetDefaultFont(font_name);
                tbox_mainText.FontFamily = font;
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
            paint_area.CurrentColor = e.NewValue!.Value;
        }

        private void OnPenButtonClick(object sender, RoutedEventArgs e)
        {
            currentBrush = PaintCanvas.PaintBrush.Pen;
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
                case PaintCanvas.PaintBrush.Pen:
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
                icon.FirstOrDefault()!.Foreground = Brush_Button_Active;
            }
            else
            {
                btn.BorderBrush = Brush_Button_Disable;
                var icon = Manager.FindVisualChilds<FontAwesome.WPF.ImageAwesome>(btn);
                icon.FirstOrDefault()!.Foreground = Brush_Button_Disable;
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
                string path = System.IO.Path.GetDirectoryName(currentTextDocPath)!;
                
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
            SaveAs();
        }

        private void btn_TextSave_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private void btn_OpenTextDoc_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                InitialDirectory = OptionsManager.CurrentOptionFile.FileSavePath,
                Title = "Open Text File",

                CheckFileExists = true,
                CheckPathExists = true,

                RestoreDirectory = true
            };

            if (dialog.ShowDialog() == true)
            {
                if (fileSaved && File.Exists(currentTextDocPath))
                {
                    File.WriteAllText(currentTextDocPath, tbox_mainText.Text);
                }
                else
                {
                    SaveManager.SaveTextToTemporaryFile(tbox_mainText.Text);
                }

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
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                isResizePanel = true;
                tbox_mainText.IsHitTestVisible = false;
                panel_drag_data = new DataObject(panel);
                DragDrop.DoDragDrop(panel, panel_drag_data, DragDropEffects.None);
            }
        }
        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (isResizePanel)
            {
                tbox_mainText.IsHitTestVisible = true;
                isResizePanel = false;
            }
        }

        private void On_DropOnWindow(object sender, DragEventArgs e)
        {
            On_WinDrop(sender, e);
        }

        #region Desktop Mode (DOM)

        public ImageAwesome GetDMToolIcon(DMTools tool)
        {
            switch (tool)
            {
                case DMTools.Pen: return DM_ToolIcon_Pencil;
                case DMTools.Eraser: return DM_ToolIcon_Eraser;
                case DMTools.Cursor: return DM_ToolIcon_Cursor;
                case DMTools.Text: return DM_ToolIcon_Text;
                case DMTools.RectBorder: return DM_ToolIcon_RectBorder;
                case DMTools.RectFill: return DM_ToolIcon_RectFill;
                case DMTools.CircleBorder: return DM_ToolIcon_CircleBorder;
                case DMTools.CircleFill: return DM_ToolIcon_CircleFill;
                case DMTools.Arrow: return DM_ToolIcon_Arrow;
                case DMTools.Line: return DM_ToolIcon_Line;
            }

            throw new Exception($"Tool: {tool} hasn't icon !");
        }
        public ImageAwesome[] GetAllDMToolIcons()
        {
            return new ImageAwesome[]
            {
                DM_ToolIcon_Pencil,
                DM_ToolIcon_Eraser,
                DM_ToolIcon_Cursor,
                DM_ToolIcon_Text,
                DM_ToolIcon_RectBorder,
                DM_ToolIcon_RectFill,
                DM_ToolIcon_CircleBorder,
                DM_ToolIcon_CircleFill,
                DM_ToolIcon_Arrow,
                DM_ToolIcon_Line
            };
        }
        public bool TryGetDMToolIcon(DMTools tool, out ImageAwesome icon)
        {
            switch (tool)
            {
                case DMTools.Pen:
                    icon = DM_ToolIcon_Pencil;
                    return true;
                case DMTools.Eraser:
                    icon = DM_ToolIcon_Eraser;
                    return true;
                case DMTools.Cursor:
                    icon = DM_ToolIcon_Cursor;
                    return true;
                case DMTools.Text:
                    icon = DM_ToolIcon_Text;
                    return true;
                case DMTools.RectBorder:
                    icon = DM_ToolIcon_RectBorder;
                    return true;
                case DMTools.RectFill:
                    icon = DM_ToolIcon_RectFill;
                    return true;
                case DMTools.CircleBorder:
                    icon = DM_ToolIcon_CircleBorder;
                    return true;
                case DMTools.CircleFill:
                    icon = DM_ToolIcon_CircleFill;
                    return true;
                case DMTools.Arrow:
                    icon = DM_ToolIcon_Arrow;
                    return true;
                case DMTools.Line:
                    icon = DM_ToolIcon_Line;
                    return true;
            }

            icon = null!;
            return false;
        }

        private void brd_DesktopModePanel_MouseLeave(object sender, MouseEventArgs e)
        {
            dm_PaintCanvas.ShowEraserPreview = true;
        }
        private void brd_DesktopModePanel_MouseEnter(object sender, MouseEventArgs e)
        {
            dm_PaintCanvas.ShowEraserPreview = false;
            dm_PaintCanvas.StopPaint();
        }
        private void SetCurrentDMTool(DMTools tool)
        {
            var theme = ThemeManager.CurrentTheme;
            Brush selected_brush = theme.DesktopModePanelToolButtonIcon.Variants[1].ToBrush();
            Brush unselected_brush = theme.DesktopModePanelToolButtonIcon.Variants[0].ToBrush();

            if (currentDMTool_Icon != null)
                currentDMTool_Icon.Foreground = unselected_brush;

            if (TryGetDMToolIcon(tool, out ImageAwesome icon))
            {
                icon.Foreground = selected_brush;
                currentDMTool_Icon = icon;
            }

            currentDMTools = tool;
        }
        private void DMP_Snaping(object sender, EventArgs args)
        {
            //dmp_snappingTimer.Stop();
            //const int SNAP_FORCE = 150;

            //double top = Canvas.GetTop(brd_DesktopModePanel);
            //double left = Canvas.GetLeft(brd_DesktopModePanel);

            ////right middle snap
            //double right_middle_x = SystemParameters.PrimaryScreenWidth - 130;
            //double right_middle_y = (SystemParameters.PrimaryScreenHeight / 2) - (brd_DesktopModePanel.Height / 2);

            ////left middle snap
            //double left_middle_x = 30;


            //if (left >= right_middle_x - SNAP_FORCE
            //    && left <= right_middle_x + SNAP_FORCE
            //    && top >= right_middle_y - SNAP_FORCE
            //    && top <= right_middle_y + SNAP_FORCE)
            //{
            //    Canvas.SetLeft(brd_DesktopModePanel, right_middle_x);
            //    Canvas.SetTop(brd_DesktopModePanel, right_middle_y);
            //}
            //else if (left >= left_middle_x - SNAP_FORCE
            //    && left <= left_middle_x + SNAP_FORCE
            //    && top >= right_middle_y - SNAP_FORCE
            //    && top <= right_middle_y + SNAP_FORCE)
            //{
            //    Canvas.SetLeft(brd_DesktopModePanel, left_middle_x);
            //    Canvas.SetTop(brd_DesktopModePanel, right_middle_y);
            //}
        }
        private void SetDMPExtended(bool value)
        {
            const int MOVE_X_PANEL = 244;

            double new_pos_delta;
            GridLength extendGridDef;

            if (value)
            {
                brd_DesktopModePanel.Resize(339);
                DMP_grid_Extended.IsHitTestVisible = true;
                DMP_grid_Extended.Opacity = 1;
                DMP_icon_Extend.Icon = FontAwesomeIcon.ArrowCircleLeft;
                DMP_btn_Extend.ToolTip = "Reduce panel";

                new_pos_delta = -MOVE_X_PANEL;
                extendGridDef = new GridLength(81, GridUnitType.Star);
            }
            else
            {
                brd_DesktopModePanel.Resize(95);
                DMP_grid_Extended.IsHitTestVisible = true;
                DMP_grid_Extended.Opacity = 0;
                DMP_icon_Extend.Icon = FontAwesomeIcon.ArrowCircleRight;
                DMP_btn_Extend.ToolTip = "Expand panel";

                new_pos_delta = MOVE_X_PANEL;
                extendGridDef = new GridLength(0, GridUnitType.Pixel);
            }

            dmp_extended = value;
            DMP_gridDef_Extend.Width = extendGridDef;
            DMP_gridDef_Base.Width = new GridLength(32, GridUnitType.Star);

            if (brd_DesktopModePanel.Position.X > 300)
            {
                brd_DesktopModePanel.Translate(new_pos_delta, 0);
                //Console.WriteLine($"Oui brd_DesktopModePanel > 300\r\n   {brd_DesktopModePanel.Position}");
            }
        }
        private void DM_canvas_Drop(object sender, DragEventArgs e)
        {
            dmp_snappingTimer = new DispatcherTimer();
            dmp_snappingTimer.Interval = TimeSpan.FromMilliseconds(10);
            dmp_snappingTimer.Tick += DMP_Snaping;
            dmp_snappingTimer.Start();

            //dm_PaintCanvas.Background = lastWinOpacityBeforeDMPDrag;
            //lastWinOpacityBeforeDMPFrag_Saved = false;
        }

        private void btn_desktopMode_Click(object sender, RoutedEventArgs e)
        {
            SetMode(AppMode.DesktopMode);
        }

        private void DMP_Close_Click(object sender, RoutedEventArgs e)
        {
            AppMode toOpen = AppMode.Text;
            
            if (lastAppModeBeforeCurrent != AppMode.DesktopMode)
            {
                toOpen = lastAppModeBeforeCurrent;
            }

            SetMode(toOpen);
        }

        private void DMP_btn_Extend_Click(object sender, RoutedEventArgs e)
        {
            SetDMPExtended(!dmp_extended);
        }

        private void DMP_btn_Tools_Click(object sender, RoutedEventArgs e)
        {
            SetWindowOpacity(0x01);
            dm_PaintCanvas.Background = new SolidColorBrush(Color.FromArgb(1, 0, 0, 0));

            if ((Button)sender == DMP_btn_DrawTool)
            {
                SetCurrentDMTool(DMTools.Pen);
                dm_PaintCanvas.SelectedBrush = PaintCanvas.PaintBrush.Pen;
                dm_PaintCanvas.CanPaint = true;
            }

            else if ((Button)sender == DMP_btn_EraseTool)
            {
                dm_PaintCanvas.SelectedBrush = PaintCanvas.PaintBrush.Eraser;
                dm_PaintCanvas.CanPaint = true;
                SetCurrentDMTool(DMTools.Eraser);
            }

            else if ((Button)sender == DMP_btn_PointerTool)
            {
                SetWindowOpacity(0x00);
                dm_PaintCanvas.CanPaint = false;
                dm_PaintCanvas.SelectedBrush = PaintCanvas.PaintBrush.None;
                dm_PaintCanvas.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                SetCurrentDMTool(DMTools.Cursor);
            }

            else if ((Button)sender == DMP_btn_TextTool)
            {
                dm_PaintCanvas.SelectedBrush = PaintCanvas.PaintBrush.Text;
                SetCurrentDMTool(DMTools.Text);
            }

            else if ((Button)sender == DMP_btn_RectBorderTool)
            {
                dm_PaintCanvas.SelectedBrush = PaintCanvas.PaintBrush.Rectangle_Outline;
                SetCurrentDMTool(DMTools.RectBorder);
            }

            else if ((Button)sender == DMP_btn_RectFillTool)
            {
                dm_PaintCanvas.SelectedBrush = PaintCanvas.PaintBrush.Rectangle_Filled;
                SetCurrentDMTool(DMTools.RectFill);
            }

            else if ((Button)sender == DMP_btn_CircleBorderTool)
            {
                dm_PaintCanvas.SelectedBrush = PaintCanvas.PaintBrush.Circle_Outline;
                SetCurrentDMTool(DMTools.CircleBorder);
            }

            else if ((Button)sender == DMP_btn_CircleFillTool)
            {
                dm_PaintCanvas.SelectedBrush = PaintCanvas.PaintBrush.Circle_Filled;
                SetCurrentDMTool(DMTools.CircleFill);
            }

            else if ((Button)sender == DMP_btn_ArrowTool)
            {
                dm_PaintCanvas.SelectedBrush = PaintCanvas.PaintBrush.Arrow;
                SetCurrentDMTool(DMTools.Arrow);
            }

            else if ((Button)sender == DMP_btn_LineTool)
            {
                dm_PaintCanvas.SelectedBrush = PaintCanvas.PaintBrush.Line;
                SetCurrentDMTool(DMTools.Line);
            }
        }

        private void DMP_slider_brushSize_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (dm_PaintCanvas != null)
                dm_PaintCanvas.Radius = (double)e.NewValue;
        }

        private void DMP_slider_eraserSize_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (dm_PaintCanvas != null)
                dm_PaintCanvas.EraseRadius = (double)e.NewValue;
        }

        private void DMP_btn_Clear_Click(object sender, RoutedEventArgs e)
        {
            dm_PaintCanvas.Clear();
        }

        
        private void DMP_colorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (dm_PaintCanvas != null)
                dm_PaintCanvas.CurrentColor = e.NewValue!.Value;
        }

        #endregion

        #region Save

        public void SaveAs()
        {
            if (SaveManager.TrySaveTextFileAs(tbox_mainText.Text, out SaveFileDialog dialog))
            {
                currentTextDocPath = dialog.FileName;
                OptionsManager.SetFileSaveEmplacement(currentTextDocPath);
                fileSaved = true;
            }
        }
        public void Save()
        {
            if (!fileSaved || !File.Exists(currentTextDocPath))
            {
                SaveAs();
                return;
            }

            File.WriteAllText(currentTextDocPath, tbox_mainText.Text);
        }

        #endregion

        #region Text Zoom Save Timer

        public void StartZoomTimer()
        {
            if (_zoomTimerToken != null)
            {
                _zoomTimerToken?.Cancel();
            }

            var font_size = tbox_mainText.FontSize;

            _zoomTimerToken = new CancellationTokenSource();
            Task.Run(() => ZoomTimer(_zoomTimerToken.Token, font_size));
        }
        private async Task ZoomTimer(CancellationToken cancellationToken, double zoomValue)
        {
            //duration: 1000 (20 * 50);
            for (int i = 0; i < 20; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await Task.Delay(50);
            }

            OptionsManager.SetDefaultZoom(zoomValue);
        }




        #endregion

    }
}
