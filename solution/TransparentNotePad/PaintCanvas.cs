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
        public double Radius { get; set; }
        public uint Elements { get; private set; }
        

        public PaintBrush SelectedBrush { get; set; }
        public Color CurrentColor { get; set; }

        private bool isMouseDown = false;
        private Point? currentPoint = null;

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

                if (currentPoint == null)
                {
                    currentPoint = pos;
                }

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
            this.Children.Clear();
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
            Console.WriteLine("PAINT CANVAS -> INIT ENDED!");
        }
        private void Init_Events()
        {
            this.MouseMove += OnMouseMove;
            this.MouseLeftButtonDown += OnMouseLefButtonDown;
            this.MouseLeftButtonUp += OnMouseLefButtonUp;
            this.MouseLeave += OnMouseExitCanvas;
        }
        private void Init_DefaultValues()
        {
            this.Cursor = Cursors.Cross;
            SelectedBrush = PaintBrush.Defautl;
            CurrentColor = Color.FromArgb(0xff, 0xff, 0xff, 0xff);
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                if (SelectedBrush == PaintBrush.Eraser)
                {
                    Erase( new System.Windows.Point(e.GetPosition(this).X, e.GetPosition(this).Y));
                }
                else
                {
                    Draw(e.GetPosition(this).X, e.GetPosition(this).Y);
                }
                
            }
                
        }
        private void Draw(double x, double y)
        {
            x -= Radius / 2;
            y -= Radius / 2;

            //PaintCircle(new Point(Convert.ToInt32(x), Convert.ToInt32(y)));
            PaintLine(new Point(Convert.ToInt32(x), Convert.ToInt32(y)));
        }
        //private void Erase(double x, double y)
        //{
        //    double divised_radius = Radius / 2;
        //    int x_randomised = 0;
        //    int y_randomised = 0;

        //    x -= divised_radius;
        //    y -= divised_radius;

        //    int point_to_place = 100;
        //    while (point_to_place > 0)
        //    {
        //        x_randomised = (int)x + random.Next((int)-divised_radius, (int)divised_radius);
        //        y_randomised = (int)y + random.Next((int)-divised_radius, (int)divised_radius);
        //        EraseCircle(new System.Windows.Point(x_randomised, y_randomised));

        //        point_to_place--;
        //    }

        //}

        private List<DependencyObject> foundControls = new List<DependencyObject>();

        private void Erase(System.Windows.Point point)
        {
            var hitTestArea = new EllipseGeometry(point, Radius, Radius);
            foundControls.Clear();

            VisualTreeHelper.HitTest( this, null,
                new HitTestResultCallback(SelectionResult), 
                new GeometryHitTestParameters(hitTestArea));

            for (int i = 0; i < foundControls.Count; i++)
            {
                try
                {
                    if (foundControls[i] != this)
                    {
                        dic_futureUndoElements.Add(foundControls[i] as UIElement, false);
                        this.Children.Remove(foundControls[i] as UIElement);
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
        
    }
}

public static class RemoveChildHelper
{
    public static void RemoveChild(this DependencyObject parent, UIElement child)
    {
        var panel = parent as Panel;
        if (panel != null)
        {
            panel.Children.Remove(child);
            return;
        }

        var decorator = parent as Decorator;
        if (decorator != null)
        {
            if (decorator.Child == child)
            {
                decorator.Child = null;
            }
            return;
        }

        var contentPresenter = parent as ContentPresenter;
        if (contentPresenter != null)
        {
            if (contentPresenter.Content == child)
            {
                contentPresenter.Content = null;
            }
            return;
        }

        var contentControl = parent as ContentControl;
        if (contentControl != null)
        {
            if (contentControl.Content == child)
            {
                contentControl.Content = null;
            }
            return;
        }
    }
}
