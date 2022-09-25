using System;
using System.Collections.Generic;
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
            Eraser
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
        private Ellipse? erase_cursor_preview = null;
        private bool isMouseDown = false;
        private Point? currentPoint = null;
        private PaintBrush selectedBrush;
        private System.Windows.Point? alt_resize_basePoint;
        private bool showEraserPreview = true;

        private List<DependencyObject> foundControls = new List<DependencyObject>();

        //bool -> has been created
        //      true:  created
        //      false: deleted

        private List<Dictionary<UIElement, bool>> undo_Elements = new List<Dictionary<UIElement, bool>>();
        private List<Dictionary<UIElement, bool>> redo_Elements = new List<Dictionary<UIElement, bool>>();
        private Dictionary<UIElement, bool> dic_futureUndoElements = new Dictionary<UIElement, bool>();
        

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

        public void EraseCircle(System.Windows.Point posistion)
        {
            HitTestResult result = VisualTreeHelper.HitTest(this, posistion);

            if (result != null)
            {
                DependencyObject obj = result.VisualHit;
                this.Children.Remove(obj as UIElement);
            }
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

                if (SelectedBrush == PaintBrush.Eraser)
                {
                    Erase(new System.Windows.Point(e.GetPosition(this).X, e.GetPosition(this).Y));
                }
                else
                {
                    Draw(e.GetPosition(this).X, e.GetPosition(this).Y);
                }

                //if (Keyboard.IsKeyDown(Key.LeftAlt))
                //{
                //    if (alt_resize_basePoint == null)
                //    {
                //        alt_resize_basePoint = e.GetPosition(this);
                //    }
                //    else
                //    {
                //        System.Windows.Point mpos = e.GetPosition(this);
                //        double distance = GetDistance(alt_resize_basePoint.Value, mpos);
                        
                //        if (alt_resize_basePoint.Value.Y < mpos.Y)
                //        {
                //            if (SelectedBrush == PaintBrush.Defautl) Radius -= distance * 0.001;
                //            else if (SelectedBrush == PaintBrush.Eraser) EraseRadius -= distance * 0.001;
                //        }
                //        else
                //        {
                //            if (SelectedBrush == PaintBrush.Defautl) Radius += distance * 0.001;
                //            else if (SelectedBrush == PaintBrush.Eraser) EraseRadius += distance * 0.001;
                //        }
                //    }

                //    return;
                //}
                //else
                //{
                //    if (alt_resize_basePoint != null) alt_resize_basePoint = null;

                //    if (SelectedBrush == PaintBrush.Eraser)
                //    {
                //        Erase(new System.Windows.Point(e.GetPosition(this).X, e.GetPosition(this).Y));
                //    }
                //    else
                //    {
                //        Draw(e.GetPosition(this).X, e.GetPosition(this).Y);
                //    }
                //}
            }
            //else
            //{
            //    if (alt_resize_basePoint != null) alt_resize_basePoint = null;
            //}
        }
        private void Draw(double x, double y)
        {
            //x -= (Radius / 2);
            //y -= (Radius / 2);

            //PaintCircle(new Point(Convert.ToInt32(x), Convert.ToInt32(y)));
            PaintLine(new Point(Convert.ToInt32(x), Convert.ToInt32(y)));
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
        }

        private void SaveToUndo()
        {
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
}

