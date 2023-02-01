using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Brush = System.Windows.Media.Brush;
using Color = System.Windows.Media.Color;
using Point = System.Drawing.Point;

namespace TransparentNotePad
{
    public class PaintCanvas : Canvas
    {
        public const int DRAW_ITERATION = 5;
        public const int MAX_UNDO_REDO = 50;

        static Random random = new Random();

        public enum PaintBrush
        {
            Defautl,
            Eraser,
            Arrow,
            Line,
            Rectangle_Filled,
            Rectangle_Outline,
            Circle_Filled,
            Circle_Outline,
            Text
        }

        public bool CanPaint { get; set; }
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

        private System.Windows.Point? initalPointCurrentDrawingRectangle;
        private System.Windows.Point? initalPointCurrentDrawingCircle;
        private System.Windows.Point? initalPointCurrentDrawingTbox;

        private List<DependencyObject> foundControls = new List<DependencyObject>();

        //bool -> has been created
        //      true:  created
        //      false: deleted

        private List<Dictionary<UIElement, bool>> undo_Elements = new List<Dictionary<UIElement, bool>>();
        private List<Dictionary<UIElement, bool>> redo_Elements = new List<Dictionary<UIElement, bool>>();
        private Dictionary<UIElement, bool> dic_futureUndoElements = new Dictionary<UIElement, bool>();

        public void Undo()
        {
            if (undo_Elements.Count <= 0) return;

            Dictionary<UIElement, bool> dic = undo_Elements[undo_Elements.Count - 1];
            Dictionary<UIElement, bool> dic_for_redo = new Dictionary<UIElement, bool>();

            foreach (var item in dic)
            {
                if (item.Value) this.Children.Remove(item.Key);
                else
                {
                    this.Children.Add(item.Key);
                }

                dic_for_redo.Add(item.Key, !item.Value);
            }

            if (redo_Elements.Count > MAX_UNDO_REDO - 1) redo_Elements.RemoveAt(0);
            redo_Elements.Add(dic_for_redo);
            undo_Elements.Remove(dic);
        }
        public void Redo()
        {
            if (redo_Elements.Count <= 0) return;
            Dictionary<UIElement, bool> dic = redo_Elements[redo_Elements.Count - 1];
            Dictionary<UIElement, bool> dic_for_undo = new Dictionary<UIElement, bool>();

            foreach (var item in dic)
            {
                if (item.Value) this.Children.Remove(item.Key);
                else this.Children.Add(item.Key);

                dic_for_undo.Add(item.Key, !item.Value);
            }

            if (undo_Elements.Count > MAX_UNDO_REDO - 1) undo_Elements.RemoveAt(0);
            undo_Elements.Add(dic_for_undo);
            redo_Elements.Remove(dic);
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
            if (TryGetBrush(out Brush brush))
            {
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
                dic_futureUndoElements.Add(line, true);
            }
        }
        public void PaintCircle(Point posistion)
        {
            Ellipse elipse = new Ellipse();

            if (TryGetBrush(out Brush brush))
            {
                elipse.Fill = brush;
                elipse.Width = Radius;
                elipse.Height = Radius;
                SetTop(elipse, posistion.Y);
                SetLeft(elipse, posistion.X);
                this.Children.Add(elipse);
            }
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
                dic_futureUndoElements.Add(currentDrawingArrow, true);
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
                dic_futureUndoElements.Add(currentDrawingLine, true);
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
                dic_futureUndoElements.Add(currentDrawingRectangle, true);
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
                dic_futureUndoElements.Add(currentDrawingCircle, true);
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
                dic_futureUndoElements.Add(currentDrawingTbox, true);
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
            SelectedBrush = PaintBrush.Defautl;
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

                if (SelectedBrush == PaintBrush.Eraser)
                {
                    Erase(new System.Windows.Point(e.GetPosition(this).X, e.GetPosition(this).Y));
                }
                else if (SelectedBrush == PaintBrush.Defautl)
                {
                    Draw(e.GetPosition(this).X, e.GetPosition(this).Y);
                }
                else if (SelectedBrush == PaintBrush.Arrow)
                {
                    DrawArrow(new System.Windows.Point(e.GetPosition(this).X, e.GetPosition(this).Y));
                }
                else if (SelectedBrush == PaintBrush.Line)
                {
                    DrawStraightLine(new System.Windows.Point(e.GetPosition(this).X, e.GetPosition(this).Y));
                }
                else if (SelectedBrush == PaintBrush.Rectangle_Filled)
                {
                    DrawRectangle(new System.Windows.Point(e.GetPosition(this).X, e.GetPosition(this).Y), true);
                }
                else if (SelectedBrush == PaintBrush.Rectangle_Outline)
                {
                    DrawRectangle(new System.Windows.Point(e.GetPosition(this).X, e.GetPosition(this).Y), false);
                }
                else if (SelectedBrush == PaintBrush.Circle_Filled)
                {
                    DrawCircle(new System.Windows.Point(e.GetPosition(this).X, e.GetPosition(this).Y), true);
                }
                else if (SelectedBrush == PaintBrush.Circle_Outline)
                {
                    DrawCircle(new System.Windows.Point(e.GetPosition(this).X, e.GetPosition(this).Y), false);
                }
                else if (SelectedBrush == PaintBrush.Text)
                {
                    DrawTextBox(new System.Windows.Point(e.GetPosition(this).X, e.GetPosition(this).Y), tbox_outline, tbox_fill);
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
                        && !((UIElement)foundControls[i]).Uid.Contains("DO_NOT_REMOVE"))
                    {
                        dic_futureUndoElements.Add((UIElement)foundControls[i], false);
                        this.Children.Remove((UIElement)foundControls[i]);
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

            if (SelectedBrush == PaintBrush.Defautl)
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
        }

        private void SaveToUndo()
        {
            if (dic_futureUndoElements.Count <= 0) return;
            undo_Elements.Add(new Dictionary<UIElement, bool>(dic_futureUndoElements));
            if (undo_Elements.Count > MAX_UNDO_REDO) undo_Elements.RemoveAt(0);

            redo_Elements.Clear();
            dic_futureUndoElements.Clear();
        }

        private bool TryGetBrush(out Brush brush)
        {
            if (SelectedBrush == PaintBrush.Eraser)
            {
                brush = new SolidColorBrush(Colors.Bisque);
                return true;
            }
            if (SelectedBrush == PaintBrush.Defautl)
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

