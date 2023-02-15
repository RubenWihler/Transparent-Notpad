using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using TransparentNotePad.CustomControls;
using TransparentNotePad.Commands;
using Brush = System.Windows.Media.Brush;
using Color = System.Windows.Media.Color;
using Point = System.Drawing.Point;
using CommandManager = TransparentNotePad.Commands.CommandManager;

namespace TransparentNotePad
{
    public class PaintCanvas : Canvas
    {
        public const int DRAW_ITERATION = 5;
        public const int MAX_UNDO_REDO = 50;

        static Random random = new Random();

        public enum PaintBrush
        {
            None,
            Pen,
            Eraser,
            Arrow,
            Line,
            Rectangle_Filled,
            Rectangle_Outline,
            Circle_Filled,
            Circle_Outline,
            Text,
            ImageBox
        }

        //undo/redo system
        private CommandManager _commandManager = new CommandManager();
        private List<UIElement> _pendingDisplayCommandElements = new List<UIElement>();
        private List<UIElement> _pendingHideCommandElements = new List<UIElement>();

        private bool _canPaint;
        private double radius;
        private double eraser_radius;
        private bool rectangle_Rounded = false;
        private bool tbox_outline = true;
        private bool tbox_fill = true;
        private string tbox_defaultValue = "type text...";
        private Ellipse? erase_cursor_preview = null;
        private bool isMouseDown = false;
        private Point? currentPoint = null;
        private PaintBrush selectedBrush;
        private System.Windows.Point? alt_resize_basePoint;
        private bool showEraserPreview = true;

        private Arrow? currentDrawingArrow;
        private Line? currentDrawingLine;
        private System.Windows.Shapes.Rectangle? currentDrawingRectangle;
        private System.Windows.Shapes.Ellipse? currentDrawingCircle;
        private TextBox? currentDrawingTbox;
        private ImageBox? currentDrawingImagebox;

        private System.Windows.Point? initalPointCurrentDrawingRectangle;
        private System.Windows.Point? initalPointCurrentDrawingCircle;
        private System.Windows.Point? initalPointCurrentDrawingTbox;
        private System.Windows.Point? initalPointCurrentDrawingImagebox;

        private List<DependencyObject> foundControls = new List<DependencyObject>();

        public bool CanPaint
        {
            get
            {
                return this._canPaint && CanPaintFunc.Invoke();
            }
            set
            {
                this._canPaint = value;
            }
        }
        public Func<bool> CanPaintFunc { get; set; } = () => true;
        public double Radius
        {
            get
            {
                return radius;
            }
            set
            {
                radius = value;
            }
        }
        public double EraseRadius
        {
            get
            {
                return eraser_radius;
            }
            set
            {
                if (value < 0) value = 0;

                eraser_radius = value;

                if (erase_cursor_preview != null)
                {
                    erase_cursor_preview!.Width = eraser_radius;
                    erase_cursor_preview!.Height = eraser_radius;
                }
            }
        }
        public uint Elements { get; private set; }
        public bool ShowEraserPreview
        {
            get
            {
                return this.showEraserPreview;
            }
            set
            {
                this.showEraserPreview = value;

                if (erase_cursor_preview != null)
                {
                    erase_cursor_preview!.Visibility =
                            (selectedBrush == PaintBrush.Eraser)
                            && value ?
                            Visibility.Visible : Visibility.Hidden;
                }

            }
        }
        public bool RectangleRounded
        {
            get
            {
                return this.rectangle_Rounded;
            }
            set
            {
                this.rectangle_Rounded = value;
            }
        }
        public bool TextBoxOutline
        {
            get
            {
                return this.tbox_outline;
            }
            set
            {
                this.tbox_outline = value;
            }
        }
        public bool TextBoxFill
        {
            get
            {
                return this.tbox_fill;
            }
            set
            {
                this.tbox_fill = value;
            }
        }

        public PaintBrush SelectedBrush
        {
            get
            {
                return selectedBrush;
            }
            set
            {
                selectedBrush = value;

                if (erase_cursor_preview != null)
                {
                    erase_cursor_preview!.Visibility =
                        selectedBrush == PaintBrush.Eraser
                        && showEraserPreview ?
                        Visibility.Visible : Visibility.Hidden;
                }

            }
        }
        public Color CurrentColor { get; set; }

        public void Undo()
        {
            _commandManager.Undo();
        }
        public void Redo()
        {
            _commandManager.Redo();
        }
        public void Clear()
        {
            List<UIElement> elements = Children.Cast<UIElement>().Where((child) => !child.Uid.Contains("DO_NOT_REMOVE")).ToList();

            for (int i = 0; i < elements.Count; i++)
            {
                Children.Remove(elements[i]);
            }
        }
        public void StopPaint()
        {
            MouseDisable();
        }
        public void PaintLine(Point pos)
        {
            if (!TryGetBrush(out Brush brush)) return;

            Line line = new Line();
            line.Stroke = brush;
            line.StrokeThickness = Radius;

            if (currentPoint == null) currentPoint = pos;

            line.X1 = currentPoint.Value.X;
            line.Y1 = currentPoint.Value.Y;
            line.X2 = pos.X;
            line.Y2 = pos.Y;
            line.StrokeDashCap = PenLineCap.Round;
            line.StrokeStartLineCap = PenLineCap.Round;
            line.StrokeEndLineCap = PenLineCap.Round;
            line.IsHitTestVisible = false;

            currentPoint = pos;

            this.Children.Add(line);
            _pendingDisplayCommandElements.Add(line);
            //_commandManager.AddCommand(new DisplayUiElementCommand(line));
        }
        
        private void DrawArrow(System.Windows.Point pos)
        {
            if (currentDrawingArrow == null)
            {
                currentDrawingArrow = new Arrow();
                currentDrawingArrow.HeadWidth = 20;
                currentDrawingArrow.HeadHeight = 10;
                currentDrawingArrow.Stroke = new SolidColorBrush(CurrentColor);
                currentDrawingArrow.StrokeThickness = 4;
                currentDrawingArrow.X1 = pos.X;
                currentDrawingArrow.Y1 = pos.Y;
                currentDrawingArrow.X2 = pos.X;
                currentDrawingArrow.Y2 = pos.Y;
                currentDrawingArrow.IsHitTestVisible = false;
                this.Children.Add(currentDrawingArrow);
                _commandManager.AddCommand(new DisplayUiElementCommand(currentDrawingArrow));
            }
            else
            {
                currentDrawingArrow.X2 = pos.X;
                currentDrawingArrow.Y2 = pos.Y;
            }
        }
        private void DrawStraightLine(System.Windows.Point pos)
        {
            if (currentDrawingLine == null)
            {
                currentDrawingLine = new Line();
                currentDrawingLine.Stroke = new SolidColorBrush(CurrentColor);
                currentDrawingLine.StrokeThickness = Radius;
                currentDrawingLine.X1 = pos.X;
                currentDrawingLine.Y1 = pos.Y;
                currentDrawingLine.X2 = pos.X;
                currentDrawingLine.Y2 = pos.Y;
                currentDrawingLine.IsHitTestVisible = false;
                this.Children.Add(currentDrawingLine);
                _commandManager.AddCommand(new DisplayUiElementCommand(currentDrawingLine));
            }
            else
            {
                currentDrawingLine.X2 = pos.X;
                currentDrawingLine.Y2 = pos.Y;
            }
        }
        private void DrawRectangle(System.Windows.Point pos, bool fill)
        {
            if (currentDrawingRectangle == null)
            {
                initalPointCurrentDrawingRectangle = pos;
                currentDrawingRectangle = new System.Windows.Shapes.Rectangle();
                
                if (fill)
                {
                    currentDrawingRectangle.Fill = new SolidColorBrush(CurrentColor);
                }
                else
                {
                    currentDrawingRectangle.Stroke = new SolidColorBrush(CurrentColor);
                    currentDrawingRectangle.StrokeThickness = Radius;
                }

                if (rectangle_Rounded)
                {
                    currentDrawingRectangle.RadiusX = 10;
                    currentDrawingRectangle.RadiusY = 10;
                }
                
                currentDrawingRectangle.IsHitTestVisible = false;
                Children.Add(currentDrawingRectangle);
                SetLeft(currentDrawingRectangle, pos.X);
                SetTop(currentDrawingRectangle, pos.Y);
                _commandManager.AddCommand(new DisplayUiElementCommand(currentDrawingRectangle));
            }
            else
            {
                double newWidth = pos.X - initalPointCurrentDrawingRectangle!.Value.X;
                double newHeight = pos.Y - initalPointCurrentDrawingRectangle!.Value.Y;

                if (newWidth >= 0) currentDrawingRectangle.Width = newWidth;
                else
                {
                    SetLeft(currentDrawingRectangle, pos.X);
                    
                    currentDrawingRectangle.Width = initalPointCurrentDrawingRectangle!.Value.X - pos.X;
                }

                if (newHeight >= 0) currentDrawingRectangle.Height = newHeight;
                else
                {
                    SetTop(currentDrawingRectangle, pos.Y);
                    currentDrawingRectangle.Height = initalPointCurrentDrawingRectangle!.Value.Y - pos.Y;
                }
            }
        }
        private void DrawCircle(System.Windows.Point pos, bool fill)
        {
            if (currentDrawingCircle == null)
            {
                initalPointCurrentDrawingCircle = pos;
                currentDrawingCircle = new System.Windows.Shapes.Ellipse();

                if (fill)
                {
                    currentDrawingCircle.Fill = new SolidColorBrush(CurrentColor);
                }
                else
                {
                    currentDrawingCircle.Stroke = new SolidColorBrush(CurrentColor);
                    currentDrawingCircle.StrokeThickness = Radius;
                }

                currentDrawingCircle.IsHitTestVisible = false;
                Children.Add(currentDrawingCircle);
                SetLeft(currentDrawingCircle, pos.X);
                SetTop(currentDrawingCircle, pos.Y);
                _commandManager.AddCommand(new DisplayUiElementCommand(currentDrawingCircle));
            }
            else
            {
                double newWidth = pos.X - initalPointCurrentDrawingCircle!.Value.X;
                double newHeight = pos.Y - initalPointCurrentDrawingCircle!.Value.Y;

                if (newWidth >= 0) currentDrawingCircle.Width = newWidth;
                else
                {
                    SetLeft(currentDrawingCircle, pos.X);

                    currentDrawingCircle.Width = initalPointCurrentDrawingCircle!.Value.X - pos.X;
                }

                if (newHeight >= 0) currentDrawingCircle.Height = newHeight;
                else
                {
                    SetTop(currentDrawingCircle, pos.Y);
                    currentDrawingCircle.Height = initalPointCurrentDrawingCircle!.Value.Y - pos.Y;
                }
            }
        }
        private void DrawTextBox(System.Windows.Point pos, bool outline, bool fill)
        {
            if (currentDrawingTbox == null)
            {
                initalPointCurrentDrawingTbox = pos;
                currentDrawingTbox = new TextBox();

                currentDrawingTbox.Foreground = new SolidColorBrush(CurrentColor);

                if (outline)
                {
                    currentDrawingTbox.BorderBrush = new SolidColorBrush(CurrentColor);
                    currentDrawingTbox.BorderThickness = new Thickness(Radius / 2);
                }
                else
                {
                    currentDrawingTbox.BorderThickness = new Thickness(0);
                }
                if (fill)
                {
                    currentDrawingTbox.Background = new SolidColorBrush(
                        Color.FromArgb(0xaa, 0x33, 0x33, 0x33));
                }
                else
                {
                    currentDrawingTbox.Background = new SolidColorBrush(
                        Color.FromArgb(0x00, 0x00, 0x00, 0x00));
                }

                currentDrawingTbox.SelectionTextBrush = null;
                currentDrawingTbox.Text = tbox_defaultValue;
                currentDrawingTbox.FontFamily = new System.Windows.Media.FontFamily("Poppins");
                currentDrawingTbox.FontSize = 18;

                currentDrawingTbox.IsHitTestVisible = true;
                Children.Add(currentDrawingTbox);
                SetLeft(currentDrawingTbox, pos.X);
                SetTop(currentDrawingTbox, pos.Y);
                _commandManager.AddCommand(new DisplayUiElementCommand(currentDrawingTbox));
            }
            else
            {
                double newWidth = pos.X - initalPointCurrentDrawingTbox!.Value.X;
                double newHeight = pos.Y - initalPointCurrentDrawingTbox!.Value.Y;

                if (newWidth >= 0) currentDrawingTbox.Width = newWidth;
                else
                {
                    SetLeft(currentDrawingTbox, pos.X);

                    currentDrawingTbox.Width = initalPointCurrentDrawingTbox!.Value.X - pos.X;
                }

                if (newHeight >= 0) currentDrawingTbox.Height = newHeight;
                else
                {
                    SetTop(currentDrawingTbox, pos.Y);
                    currentDrawingTbox.Height = initalPointCurrentDrawingTbox!.Value.Y - pos.Y;
                }
            }
        }
        private void DrawImageBox(System.Windows.Point pos)
        {
            if (currentDrawingImagebox == null)
            {
                initalPointCurrentDrawingImagebox = pos;
                currentDrawingImagebox = new ImageBox();

                currentDrawingImagebox.IsHitTestVisible = true;
                Children.Add(currentDrawingImagebox);
                SetLeft(currentDrawingImagebox, pos.X);
                SetTop(currentDrawingImagebox, pos.Y);
                _commandManager.AddCommand(new DisplayUiElementCommand(currentDrawingImagebox));
            }
            else
            {
                double newWidth = pos.X - initalPointCurrentDrawingImagebox!.Value.X;
                double newHeight = pos.Y - initalPointCurrentDrawingImagebox!.Value.Y;

                if (newWidth >= 0) currentDrawingImagebox.Width = newWidth;
                else
                {
                    SetLeft(currentDrawingImagebox, pos.X);

                    currentDrawingImagebox.Width = initalPointCurrentDrawingImagebox!.Value.X - pos.X;
                }

                if (newHeight >= 0) currentDrawingImagebox.Height = newHeight;
                else
                {
                    SetTop(currentDrawingImagebox, pos.Y);
                    currentDrawingImagebox.Height = initalPointCurrentDrawingImagebox!.Value.Y - pos.Y;
                }

                if (!currentDrawingImagebox.Highlighted) currentDrawingImagebox.Highlight();
            }
        }
        private void Draw(double x, double y)
        {
            PaintLine(new Point(Convert.ToInt32(x), Convert.ToInt32(y)));
        }
        
        
        public override void EndInit()
        {
            base.EndInit();
            Init();
        }
        private void Init()
        {
            Init_Events();
            Init_DefaultValues();
            Init_StaticObjects();
            Console.WriteLine("PAINT CANVAS -> INIT ENDED!");
        }
        private void Init_Events()
        {
            this.MouseMove += OnMouseMove;
            this.MouseLeftButtonDown += OnMouseLefButtonDown;
            this.MouseLeftButtonUp += OnMouseLefButtonUp;
            this.MouseLeave += OnMouseExitCanvas;
        }
        private void Init_StaticObjects()
        {
            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].Uid += "DO_NOT_REMOVE";
            }
        }
        private void Init_DefaultValues()
        {
            this.Cursor = Cursors.Cross;
            SelectedBrush = PaintBrush.None;
            CurrentColor = Color.FromArgb(0xff, 0xff, 0xff, 0xff);
            Init_ErasePreview();
        }
        public void Init_ErasePreview()
        {
            this.erase_cursor_preview = new Ellipse();
            this.erase_cursor_preview.Height = EraseRadius;
            this.erase_cursor_preview.Width  = EraseRadius;
            this.erase_cursor_preview.StrokeThickness = 2;
            this.erase_cursor_preview.Stroke = new SolidColorBrush(Color.FromArgb(230, 200, 200, 200));
            this.erase_cursor_preview.IsHitTestVisible = false;
            this.erase_cursor_preview.Fill = new SolidColorBrush(Color.FromArgb(70, 200, 200, 200));
            this.erase_cursor_preview.Uid += "DO_NOT_REMOVE";
            this.Children.Add(this.erase_cursor_preview);
            SetZIndex(this.erase_cursor_preview, 10);
            this.erase_cursor_preview.Visibility = Visibility.Hidden;
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (SelectedBrush == PaintBrush.Eraser)
            {
                if (erase_cursor_preview != null)
                {
                    SetTop(erase_cursor_preview, e.GetPosition(this).Y - EraseRadius / 2);
                    SetLeft(erase_cursor_preview, e.GetPosition(this).X - EraseRadius / 2);
                }
            }

            if (isMouseDown)
            {
                //if (alt_resize_basePoint != null) alt_resize_basePoint = null;
                if (!CanPaint) return;

                switch (SelectedBrush)
                {
                    case PaintBrush.Eraser:
                        Erase(new System.Windows.Point(e.GetPosition(this).X, e.GetPosition(this).Y));
                        break;

                    case PaintBrush.Pen:
                        Draw(e.GetPosition(this).X, e.GetPosition(this).Y);
                        break;

                    case PaintBrush.Arrow:
                        DrawArrow(new System.Windows.Point(e.GetPosition(this).X, e.GetPosition(this).Y));
                        break;

                    case PaintBrush.Line:
                        DrawStraightLine(new System.Windows.Point(e.GetPosition(this).X, e.GetPosition(this).Y));
                        break;

                    case PaintBrush.Rectangle_Filled:
                        DrawRectangle(new System.Windows.Point(e.GetPosition(this).X, e.GetPosition(this).Y), true);
                        break;

                    case PaintBrush.Rectangle_Outline:
                        DrawRectangle(new System.Windows.Point(e.GetPosition(this).X, e.GetPosition(this).Y), false);
                        break;

                    case PaintBrush.Circle_Filled:
                        DrawCircle(new System.Windows.Point(e.GetPosition(this).X, e.GetPosition(this).Y), true);
                        break;

                    case PaintBrush.Circle_Outline:
                        DrawCircle(new System.Windows.Point(e.GetPosition(this).X, e.GetPosition(this).Y), false);
                        break;

                    case PaintBrush.Text:
                        DrawTextBox(new System.Windows.Point(e.GetPosition(this).X, e.GetPosition(this).Y), tbox_outline, tbox_fill);
                        break;

                    case PaintBrush.ImageBox:
                        DrawImageBox(new System.Windows.Point(e.GetPosition(this).X, e.GetPosition(this).Y));
                        break;
                }
            }

        }


        private void Erase(System.Windows.Point point)
        {
            double divisedRadius = EraseRadius / 2;
            var hitTestArea = new EllipseGeometry(point, divisedRadius, divisedRadius);
            foundControls.Clear();

            VisualTreeHelper.HitTest(this, null,
                new HitTestResultCallback(SelectionResult),
                new GeometryHitTestParameters(hitTestArea));

            for (int i = 0; i < foundControls.Count; i++)
            {
                try
                {
                    if (foundControls[i] != this 
                        && !((UIElement)foundControls[i]).Uid.Contains("DO_NOT_REMOVE")
                        && ((UIElement)foundControls[i]).Visibility != Visibility.Hidden)
                    {
                        //dic_futureUndoElements.Add((UIElement)foundControls[i], false);
                        //this._commandManager.AddCommand(new HideUiElementCommand(foundControls[i]));
                        ((UIElement)foundControls[i]).Visibility = Visibility.Hidden;
                        this._pendingHideCommandElements.Add((UIElement)foundControls[i]);
                        //this.Children.Remove((UIElement)foundControls[i]);
                    }
                }
                catch (Exception) { }
            }
        }
        public HitTestResultBehavior SelectionResult(HitTestResult result)
        {
            IntersectionDetail id = ((GeometryHitTestResult)result).IntersectionDetail;
            switch (id)
            {
                case IntersectionDetail.FullyContains:
                case IntersectionDetail.FullyInside:
                    foundControls.Add(result.VisualHit);
                    return HitTestResultBehavior.Continue;
                case IntersectionDetail.Intersects:
                    foundControls.Add(result.VisualHit);
                    return HitTestResultBehavior.Continue;
                default:
                    return HitTestResultBehavior.Stop;
            }
        }

        private void OnMouseLefButtonDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;

            if (SelectedBrush == PaintBrush.Pen && CanPaint)
                Draw(e.GetPosition(this).X, e.GetPosition(this).Y);
        }
        private void OnMouseLefButtonUp(object sender, MouseEventArgs e)
        {
            MouseDisable();
        }
        private void OnMouseExitCanvas(object sender, MouseEventArgs e)
        {
            MouseDisable();
        }

        private void MouseDisable()
        {
            isMouseDown = false;
            currentPoint = null;
            SaveToUndo();

            if (currentDrawingArrow != null) currentDrawingArrow = null;
            if (currentDrawingLine != null) currentDrawingLine = null;
            if (currentDrawingRectangle != null) currentDrawingRectangle = null;
            if (currentDrawingCircle != null) currentDrawingCircle = null;
            if (currentDrawingTbox != null) currentDrawingTbox = null;
            if (currentDrawingImagebox != null)
            {
                currentDrawingImagebox.UnHighlight();
                currentDrawingImagebox.UpdateSize();
                currentDrawingImagebox.Focus();
                currentDrawingImagebox = null;
            }

        }

        private void SaveToUndo()
        {
            if (_pendingDisplayCommandElements.Count > 0)
            {
                var command = new DisplayMultipleUiElementsCommand(_pendingDisplayCommandElements.ToArray());
                _commandManager.AddCommand(command);
                _pendingDisplayCommandElements.Clear();
            }

            if (_pendingHideCommandElements.Count > 0)
            {
                var command = new HideMultipleUiElementsCommand(_pendingHideCommandElements.ToArray());
                _commandManager.AddCommand(command);
                _pendingHideCommandElements.Clear();
            }
        }

        private bool TryGetBrush(out Brush brush)
        {
            if (SelectedBrush == PaintBrush.Eraser)
            {
                brush = new SolidColorBrush(Colors.Bisque);
                return true;
            }
            if (SelectedBrush == PaintBrush.Pen)
            {
                brush = new SolidColorBrush(CurrentColor);
                return true;
            }
            //...

            brush = null;
            return false;
        }

        private double GetDistance(System.Windows.Point p1, System.Windows.Point p2)
        {
            return ((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }
    }

    /// <summary>
    /// this code is not mine.
    /// here is the link where I found it:
    /// https://www.codeproject.com/Articles/23116/WPF-Arrow-and-Custom-Shapes
    /// </summary>
    public sealed class Arrow : Shape
    {
        #region Dependency Properties

        public static readonly DependencyProperty X1Property = DependencyProperty.Register("X1", typeof(double), typeof(Arrow), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty Y1Property = DependencyProperty.Register("Y1", typeof(double), typeof(Arrow), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty X2Property = DependencyProperty.Register("X2", typeof(double), typeof(Arrow), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty Y2Property = DependencyProperty.Register("Y2", typeof(double), typeof(Arrow), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty HeadWidthProperty = DependencyProperty.Register("HeadWidth", typeof(double), typeof(Arrow), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty HeadHeightProperty = DependencyProperty.Register("HeadHeight", typeof(double), typeof(Arrow), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        #endregion

        #region CLR Properties

        [TypeConverter(typeof(LengthConverter))]
        public double X1
        {
            get { return (double)base.GetValue(X1Property); }
            set { base.SetValue(X1Property, value); }
        }

        [TypeConverter(typeof(LengthConverter))]
        public double Y1
        {
            get { return (double)base.GetValue(Y1Property); }
            set { base.SetValue(Y1Property, value); }
        }

        [TypeConverter(typeof(LengthConverter))]
        public double X2
        {
            get { return (double)base.GetValue(X2Property); }
            set { base.SetValue(X2Property, value); }
        }

        [TypeConverter(typeof(LengthConverter))]
        public double Y2
        {
            get { return (double)base.GetValue(Y2Property); }
            set { base.SetValue(Y2Property, value); }
        }

        [TypeConverter(typeof(LengthConverter))]
        public double HeadWidth
        {
            get { return (double)base.GetValue(HeadWidthProperty); }
            set { base.SetValue(HeadWidthProperty, value); }
        }

        [TypeConverter(typeof(LengthConverter))]
        public double HeadHeight
        {
            get { return (double)base.GetValue(HeadHeightProperty); }
            set { base.SetValue(HeadHeightProperty, value); }
        }

        #endregion

        #region Overrides

        protected override Geometry DefiningGeometry
        {
            get
            {
                // Create a StreamGeometry for describing the shape
                StreamGeometry geometry = new StreamGeometry();
                geometry.FillRule = FillRule.EvenOdd;

                using (StreamGeometryContext context = geometry.Open())
                {
                    InternalDrawArrowGeometry(context);
                }

                // Freeze the geometry for performance benefits
                geometry.Freeze();

                return geometry;
            }
        }

        #endregion

        #region Privates

        private void InternalDrawArrowGeometry(StreamGeometryContext context)
        {
            double theta = Math.Atan2(Y1 - Y2, X1 - X2);
            double sint = Math.Sin(theta);
            double cost = Math.Cos(theta);

            System.Windows.Point pt1 = new System.Windows.Point(X1, this.Y1);
            System.Windows.Point pt2 = new System.Windows.Point(X2, this.Y2);

            System.Windows.Point pt3 = new System.Windows.Point(
                X2 + (HeadWidth * cost - HeadHeight * sint),
                Y2 + (HeadWidth * sint + HeadHeight * cost));

            System.Windows.Point pt4 = new System.Windows.Point(
                X2 + (HeadWidth * cost + HeadHeight * sint),
                Y2 - (HeadHeight * cost - HeadWidth * sint));

            context.BeginFigure(pt1, true, false);
            context.LineTo(pt2, true, true);
            context.LineTo(pt3, true, true);
            context.LineTo(pt2, true, true);
            context.LineTo(pt4, true, true);
        }

        #endregion
    }
}

