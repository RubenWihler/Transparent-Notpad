using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Image = System.Windows.Controls.Image;
using Point = System.Windows.Point;
using Color = System.Windows.Media.Color;

namespace TransparentNotePad.CustomControls
{
    public class ImageBox : Control
    {
        private Border _border = null!;
        private Grid _main_grid = null!;
        private StackPanel _header_stack_panel = null!;
        private CustomButton _button_open_image = null!;
        private CustomButton _button_remove = null!;
        private CustomIcon _icon_button_open_image = null!;
        private CustomIcon _icon_button_remove = null!;
        private Image _image = null!;

        private bool _isBeingDestroy = false;
        private ResizeAdorner _resizeAdorner = null!;
        private Bitmap _bitmap = null!;
        private double _margingAddition = 5;

        //cache for optimisation
        private double _margingLeftRight = 0;
        private double _margingTopBottom = 0;

        private bool _isMoving;
        private bool _moveFirstClick = true;
        private double _moveStartX;
        private double _moveStartY;
        private double _moveRelativeX;
        private double _moveRelativeY;

        static ImageBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageBox),
                new FrameworkPropertyMetadata(typeof(ImageBox)));
        }

        public Bitmap Bitmap
        {
            get
            {
                return _bitmap;
            }
            set
            {
                if (value == null)
                {
                    if (!Highlighted) SetBackgroundOpacity(0x01);
                    else SetBackgroundOpacity(0x32);

                    _image.Source = null;

                    return;
                }

                MemoryStream ms = new MemoryStream();
                value.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                ms.Position = 0;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = ms;
                bitmapImage.EndInit();

                _image.Source = bitmapImage;
                this._bitmap = value;

                if (!Highlighted)
                    SetBackgroundOpacity(0x01);

                UpdateSize();
            }
        }
        public Point Position
        {
            get
            {
                var transform = this.TransformToAncestor(this.Parent as Visual);
                return transform.Transform(new Point(0, 0));
            }
            set
            {
                this.MoveTo(value);
            }
        }
        public Point Size
        {
            get
            {
                return new Point(this.Width, this.Height);
            }
        }
        public bool Highlighted { get; protected set; }
        public double MargingAddition
        {
            get
            {
                return this._margingAddition;
            }
            set
            {
                this._margingAddition = value;
                UpdateMarging();
            }
        }

        private ResizeAdorner _ResizeAdorner
        {
            get
            {
                if (_resizeAdorner == null)
                {
                    _resizeAdorner = new ResizeAdorner(this);
                    _resizeAdorner.onResized += () => { UpdateMoveStartPoint(); };
                }

                _resizeAdorner.ApplyTheme(ThemeManager.CurrentTheme);
                return _resizeAdorner;
            }
        }
        private double _MargingLeftRight
        {
            get
            {
                if (this._margingLeftRight < 1)
                {
                    UpdateMarging();
                }

                return this._margingLeftRight;
            }
        }
        private double _MargingTopBottom
        {
            get
            {
                if (this._margingTopBottom < 1)
                {
                    UpdateMarging();
                }

                return this._margingTopBottom;
            }
        }

        public override void OnApplyTemplate()
        {
            _border = (Border)Template.FindName("border", this);
            _main_grid = (Grid)Template.FindName("grid_main", this);
            _header_stack_panel = (StackPanel)Template.FindName("header_stack_panel", this);
            _button_open_image = (CustomButton)Template.FindName("btn_open_image", this);
            _button_remove = (CustomButton)Template.FindName("btn_remove", this);
            _icon_button_open_image = (CustomIcon)Template.FindName("btn_open_image_icon", this);
            _icon_button_remove = (CustomIcon)Template.FindName("btn_remove_icon", this);
            _image = (Image)Template.FindName("image", this);

            this.RenderTransform = new TranslateTransform();

            MainWindow.onWindowResize += MainWindow_onWindowResize;

            this.Drop += OnImageDrop;
            this.MouseDown += OnMouseDown;
            this.MouseUp += OnMouseUp;
            this.GotFocus += OnGotFocus;
            this.LostFocus += OnLostFocus;
            this.MouseMove += OnMouseMove;
            this.MouseLeave += OnMouseLeave;
            this._button_remove.Click += _button_remove_Click;
            this._button_open_image.Click += _button_open_image_Click;

            var current_theme = ThemeManager.CurrentTheme;
            var bg_color = current_theme.GlobalTextColor;

            _button_open_image.ApplyTheme(current_theme);
            _button_remove.ApplyTheme(current_theme);
            _icon_button_open_image.ApplyTheme(current_theme);
            _icon_button_remove.ApplyTheme(current_theme);

            _border.Background = new SolidColorBrush(
                System.Windows.Media.Color.FromArgb(50, bg_color.R, bg_color.G, bg_color.B));

            base.OnApplyTemplate();
        }

        private void MainWindow_onWindowResize()
        {
            UpdateMoveStartPoint();
        }
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_isMoving)
            {
                //Get the position of the mouse relative to the controls parent              
                Point MousePoint = Mouse.GetPosition(this.Parent as IInputElement);

                if ((MousePoint.X + this.Width - _moveRelativeX > (Parent as FrameworkElement).ActualWidth) ||
                    MousePoint.Y + this.Height - _moveRelativeY > (Parent as FrameworkElement).ActualHeight ||
                    MousePoint.X - _moveRelativeX < 0 ||
                    MousePoint.Y - _moveRelativeY < 0)
                {
                    return;
                }

                //set the distance from the original position
                var distance_from_start_x = MousePoint.X - _moveStartX - _moveRelativeX;
                var distance_from_start_y = MousePoint.Y - _moveStartY - _moveRelativeY;
                //Set the X and Y coordinates of the RenderTransform to be the Distance from original position. This will move the control
                TranslateTransform MoveTransform = base.RenderTransform as TranslateTransform;
                MoveTransform.X = distance_from_start_x;
                MoveTransform.Y = distance_from_start_y;


                var element = this.Parent as Visual;
                AdornerLayer.GetAdornerLayer(element).Remove(_ResizeAdorner);
                AdornerLayer.GetAdornerLayer(element).Add(_ResizeAdorner);
            }
        }
        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            this._isMoving = false;
            //_moveFirstClick = true;
        }
        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            this._isMoving = false;
            //_moveFirstClick = true;
        }
        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!this.IsFocused)
            {
                this.Focus();
            }

            if (e.ButtonState == MouseButtonState.Pressed)
            {
                if (_moveFirstClick)
                {
                    GeneralTransform transform = this.TransformToAncestor(this.Parent as Visual);
                    Point start_point = transform.Transform(new Point(0, 0));
                    _moveStartX = start_point.X;
                    _moveStartY = start_point.Y;
                    _moveFirstClick = false;
                }

                _isMoving = true;

                Point RelativeMousePoint = Mouse.GetPosition(this);
                _moveRelativeX = RelativeMousePoint.X;
                _moveRelativeY = RelativeMousePoint.Y;
            }
        }
        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
            var element = this.Parent as Visual;
            try
            {
                AdornerLayer.GetAdornerLayer(element).Remove(_ResizeAdorner);
            }
            catch (Exception) {}

            AdornerLayer.GetAdornerLayer(element).Add(_ResizeAdorner);
        }
        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (!_isBeingDestroy && this.Visibility == Visibility.Visible && _resizeAdorner != null)
            {
                var element = this.Parent as Visual;
                AdornerLayer.GetAdornerLayer(element)?.Remove(_ResizeAdorner);
            }
        }
        private void OnImageDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var accepted_extensions = new List<string>
                {
                    ".png",
                    ".jpg"
                };

                var files_path = (string[])e.Data.GetData(DataFormats.FileDrop);
                var image_file_path = string.Empty;

                foreach (var path in files_path)
                {
                    if (accepted_extensions.Contains(Path.GetExtension(path)))
                    {
                        image_file_path = path;
                        break;
                    }
                }

                if (image_file_path == string.Empty)
                    return;


                Bitmap = new Bitmap(image_file_path);
            }
        }

        private void UpdateMoveStartPoint()
        {
            if (_isBeingDestroy || this.Parent == null) return;

            var transform = ((TranslateTransform)this.RenderTransform);
            GeneralTransform ref_transform = this.TransformToAncestor(this.Parent as Visual);
            Point start_point = ref_transform.Transform(new Point(0, 0));
            _moveStartX = start_point.X - transform.X;
            _moveStartY = start_point.Y - transform.Y;
        }
        private void SetBackgroundOpacity(byte opacity, Color? color = null)
        {
            color ??= Color.FromArgb(0xFF,0xFF, 0xFF, 0xFF);

            _border.Background = new SolidColorBrush(
                Color.FromArgb(opacity, color.Value.R, color.Value.G, color.Value.B));
        }
        private void _button_remove_Click(object sender, RoutedEventArgs e)
        {
            _isBeingDestroy = true;

            MainWindow.onWindowResize -= MainWindow_onWindowResize;

            DependencyObject parent = VisualTreeHelper.GetParent(this);
            (parent as Panel).Children.Remove(this);
        }
        private void _button_open_image_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                //Uri fileUri = new Uri();
                Bitmap = new Bitmap(openFileDialog.FileName);
            }
        }
        private void UpdateMarging()
        {
            this._margingLeftRight = (this.Width  / 2) + this._margingAddition;
            this._margingTopBottom = (this.Height / 2) + this._margingAddition;
        }

        public void UpdateSize()
        {
            if (_bitmap != null)
            {
                var scaleHeight = (float)this.Width / this._bitmap.Width;
                var scaleWidth = (float)this.Height / this._bitmap.Height;
                var scale = Math.Min(scaleHeight, scaleWidth);

                this.Height = _bitmap.Height * scale;
                this.Width = _bitmap.Width * scale;
            }

            UpdateMoveStartPoint();
        }
        public void Resize(Point dimensions, bool keepAspectRatio = false)
        {
            var new_width  = dimensions.X;
            var new_height = dimensions.Y;

            if (keepAspectRatio)
            {
                var scaleHeight = (float)new_width / this.Width;
                var scaleWidth = (float)new_height / this.Height;
                var scale = Math.Min(scaleHeight, scaleWidth);

                new_height = this.Height * scale;
                new_width  = this.Width * scale;
            }

            this.Width = new_width;
            this.Height = new_height;

            UpdateMoveStartPoint();
        }
        public void Highlight(byte opacity = 0x64)
        {
            SetBackgroundOpacity(opacity, ThemeManager.CurrentTheme.GlobalTextColor.ToColor());
            Highlighted = true;
        }
        public void UnHighlight()
        {
            Highlighted = false;

            if (_bitmap == null)
            {
                SetBackgroundOpacity(0x32, ThemeManager.CurrentTheme.GlobalTextColor.ToColor());
                return;
            }

            SetBackgroundOpacity(0x01);
        }
        public void MoveTo(Point position)
        {
            if (position.X < _MargingLeftRight) position.X = _MargingLeftRight;
            if (position.Y < _MargingTopBottom) position.Y = _MargingTopBottom;

            if (_moveFirstClick)
            {
                UpdateMoveStartPoint();
            }

            var distance_from_start_x = position.X - _moveStartX;
            var distance_from_start_y = position.Y - _moveStartY;

            var move_transform = base.RenderTransform as TranslateTransform;

            move_transform!.X = distance_from_start_x;
            move_transform!.Y = distance_from_start_y;
        }
    }
}
