using System;
using System.Security.Cryptography.Xml;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace TransparentNotePad.CustomControls
{
    [NameScopeProperty("themeColorType", typeof(ThemeColorType))]
    public class MovablePanel : CustomBorder
    {
        private bool _isMoving;
        private bool _moveFirstClick = true;
        private double _moveStartX;
        private double _moveStartY;
        private double _moveRelativeX;
        private double _moveRelativeY;

        public Point Position
        {
            get
            {
                var transform = this.TransformToAncestor(this.Parent as Visual);
                return transform.Transform(new Point(0, 0));
            }
        }

        public override void EndInit()
        {
            base.EndInit();

            this.MouseDown += OnMouseDown;
            this.MouseUp += OnMouseUp;
            this.MouseMove += OnMouseMove;
            this.MouseLeave += OnMouseLeave;
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
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
        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            this._isMoving = false;
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
            }
        }
        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            this._isMoving = false;
        }

        public void UpdateMoveStartPoint()
        {
            var transform = ((TranslateTransform)this.RenderTransform);
            var ref_transform = this.TransformToAncestor(this.Parent as Visual);
            var start_point = ref_transform.Transform(new Point(0, 0));

            var new_pos_x = start_point.X - transform.X;
            var new_pos_y = start_point.Y - transform.Y;

            _moveStartX -= new_pos_x;
            _moveStartY -= new_pos_y;

            Console.WriteLine($"updated start point:\r\n_moveStartX: {_moveStartX}\r\n_moveStartY: {_moveStartY}");
        }
        public void MoveTo(double x, double y)
        {
            if (x < 0) x = 0;
            if (y < 0) y = 0;

            var distance_from_start_x = x - _moveStartX;
            var distance_from_start_y = y - _moveStartY;

            var move_transform = base.RenderTransform as TranslateTransform;

            move_transform!.X = distance_from_start_x;
            move_transform!.Y = distance_from_start_y;

            //Console.Clear();
            //Console.WriteLine($"updated position:");
            //Console.WriteLine($"old : \r\n    x: {old_pos_x}\r\n    y: {old_pos_y}");
            //Console.WriteLine($"new : \r\n    x: {move_transform!.X}\r\n    y: {move_transform!.Y}");
        }
        public void Translate(double x, double y)
        {
            var transform = this.TransformToAncestor(this.Parent as Visual);
            Point old_pos = transform.Transform(new Point(0, 0));

            var distance_x = old_pos.X + x;
            var distance_y = old_pos.Y + y;

            MoveTo(distance_x, distance_y);
        }
        public void Resize(double? width = null, double? height = null)
        {
            width ??= this.Width;
            height ??= this.Height;

            if (width < 0) width = 0;
            if (height < 0) height = 0;

            this.Width = width.Value;
            this.Height = height.Value;

            //UpdateMoveStartPoint();
        }
    }
}
