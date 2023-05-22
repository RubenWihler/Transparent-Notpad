using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TransparentNotePad.CustomControls
{
    /// <summary>
    /// Taken from github : 
    /// https://github.com/TacticDevGit/WPF-Resizing-Adorner
    /// </summary>
    class ResizeAdorner : Adorner, IThemable
    {
        public event Action onResized;

        private VisualCollection _adornerVisuals;
        private Thumb _thumb1;
        private Thumb _thumb2;
        private Rectangle _rectangle;

        protected override int VisualChildrenCount
        {
            get
            {
                return _adornerVisuals.Count;
            }
        }

        public ResizeAdorner(UIElement adornedElement) : base(adornedElement)
        {
            const int thumbs_width = 10;
            const int thumbs_height = 10;

            var current_theme_tcolor = ThemeManager.CurrentTheme.PrimaryColor;
            var bg_brush = current_theme_tcolor.ToBrush();
            var stroke_brush = new SolidColorBrush(Color.FromArgb(200,
                current_theme_tcolor.R, current_theme_tcolor.G, current_theme_tcolor.B));

            _adornerVisuals = new VisualCollection(this);
            _thumb2 = new Thumb() 
            { 
                Background = bg_brush,
                Height = thumbs_height,
                Width = thumbs_width,
                Cursor = Cursors.SizeNWSE
            };

            _rectangle = new Rectangle() 
            { 
                Stroke = stroke_brush, 
                StrokeThickness = 2,
                //StrokeDashArray = { 3, 2 }
                StrokeLineJoin = new PenLineJoin()
            };

            //_thumb1.DragDelta += Thumb1_DragDelta;
            _thumb2.DragDelta += Thumb2_DragDelta;

            _adornerVisuals.Add(_rectangle);
            _adornerVisuals.Add(_thumb2);

            if (!((FrameworkElement)AdornedElement).Parent.GetType().IsAssignableFrom(typeof(PaintCanvas)))
            {
                _thumb1 = new Thumb()
                {
                    Background = bg_brush,
                    Height = thumbs_height,
                    Width = thumbs_width,
                    Cursor = Cursors.SizeNWSE
                };

                _thumb1.DragDelta += Thumb1_DragDelta;
                _adornerVisuals.Add(_thumb1);
            }

            ApplyTheme(ThemeManager.CurrentTheme);
        }

        public void ApplyTheme(Theme theme)
        {
            var current_theme_tcolor = theme.PrimaryColor;
            var bg_brush = current_theme_tcolor.ToBrush();
            var stroke_brush = new SolidColorBrush(Color.FromArgb(200,
                current_theme_tcolor.R, current_theme_tcolor.G, current_theme_tcolor.B));

            if (_thumb1 != null) this._thumb1.Background = bg_brush;

            this._thumb2.Background = bg_brush;
            this._rectangle.Stroke = stroke_brush;
        }

        protected override Visual GetVisualChild(int index)
        {
            return _adornerVisuals[index];
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            _rectangle.Arrange(new Rect(-2.5, -2.5, AdornedElement.DesiredSize.Width + 5, AdornedElement.DesiredSize.Height + 5));
            _thumb1?.Arrange(new Rect(-5, -5, 10, 10));
            _thumb2.Arrange(new Rect(AdornedElement.DesiredSize.Width - 5, AdornedElement.DesiredSize.Height - 5, 10, 10));

            return base.ArrangeOverride(finalSize);
        }

        private void Thumb2_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Resize(e.VerticalChange, e.HorizontalChange, false, Keyboard.IsKeyDown(Key.LeftShift));
        }
        private void Thumb1_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Resize(e.VerticalChange, e.HorizontalChange, true, Keyboard.IsKeyDown(Key.LeftShift));
        }

        public void Resize(double verticalChange, double horizontalChange, bool fromUp = false, bool keepAspectRatio = false)
        {
            var target = (FrameworkElement)AdornedElement;
            double result_height;
            double result_width;

            if (fromUp)
            {
                result_height = target.Height - verticalChange < 0 ?
                    0 : target.Height - verticalChange;

                result_width = target.Width - horizontalChange < 0 ?
                    0 : target.Width - horizontalChange;
            }
            else
            {
                result_height = target.Height + verticalChange < 0 ?
                    0 : target.Height + verticalChange;

                result_width = target.Width + horizontalChange < 0 ?
                    0 : target.Width + horizontalChange;
            }

            if (keepAspectRatio)
            {
                var scaleHeight = (float)result_height / (float)target.Height;
                var scaleWidth = (float)result_width / (float)target.Width;
                float scale = Math.Min(scaleHeight, scaleWidth);

                if (scale <= 0) return;
                target.Height *= scale;
                target.Width *= scale;

                onResized?.Invoke();
                return;
            }

            target.Height = result_height;
            target.Width = result_width;

            onResized?.Invoke();
        }
    }
}
