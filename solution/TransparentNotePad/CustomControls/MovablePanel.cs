using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;

namespace TransparentNotePad.CustomControls
{
    [NameScopeProperty("themeColorType", typeof(ThemeColorType))]
    public class MovablePanel : CustomBorder
    {
        public event Action onStartMoving;
        public event Action onEndMoving;

        private bool _can_snap = true;
        private Dictionary<Point, Key?> _snap_points = new Dictionary<Point, Key?>();
        private double _snap_bound = 200;

        private bool _isMoving;
        private bool _moveFirstClick = true;
        private double _moveStartX;
        private double _moveStartY;
        private double _moveRelativeX;
        private double _moveRelativeY;

        DispatcherTimer _panel_leave_timer = new DispatcherTimer();
        CancellationTokenSource _panel_leave_timer_cancellation = new CancellationTokenSource();

        public Point Position
        {
            get
            {
                var transform = this.TransformToAncestor(this.Parent as Visual);
                return transform.Transform(new Point(0, 0));
            }
        }
        public bool CanSnap
        {
            get
            {
                return _can_snap;
            }
            set
            {
                _can_snap = value;
            }
        }
        //public List<Point> SnapPoints
        //{
        //    get
        //    {
        //        return _snap_points.AsReadOnly();
        //    }
        //    set
        //    {
        //        this._snap_points = value;

        //        foreach (Point snap_point in _snap_points)
        //        {
        //            if (!_snap_points_shortcut_key.ContainsKey(snap_point))
        //            {
        //                _snap_points_shortcut_key.Add(snap_point, null);
        //            }
        //        }
        //    }
        //}
        //public Dictionary<Point, Key?> SnapPointsShortcutKey
        //{
        //    get
        //    {
        //        return _snap_points_shortcut_key;
        //    }
        //    set
        //    {
        //        _snap_points_shortcut_key = value;

        //        foreach (var snap_point_shortcut in _snap_points_shortcut_key.Keys)
        //        {
        //            if (!_snap_points.Contains(snap_point_shortcut))
        //            {
        //                _snap_points.Add(snap_point_shortcut);
        //            }
        //        }
        //    }
        //}
        public double SnapBound
        {
            get
            {
                return this._snap_bound;
            }
            set
            {
                this._snap_bound = value;
            }
        }
        private bool _IsMoving
        {
            get
            {
                return _isMoving;
            }
            set
            {
                if (_isMoving != value)
                {
                    if (value) OnStartMoving();
                    else OnEndMoving();
                }

                _isMoving = value;
            }
        }

        public override void EndInit()
        {
            base.EndInit();

            this.RenderTransform = new TranslateTransform();

            this.MouseDown += OnMouseDown;
            this.MouseUp += OnMouseUp;
            this.MouseMove += OnMouseMove;
            this.MouseEnter += OnMouseEnter;
            this.MouseLeave += OnMouseLeave;
            this.KeyDown += OnKeyDown;

            _panel_leave_timer = new DispatcherTimer();
            _panel_leave_timer.Interval = TimeSpan.FromSeconds(0.1);
            _panel_leave_timer.Tick += MouseLeaveAttemptCall;
        }

        private void OnStartMoving()
        {
            onStartMoving?.Invoke();
        }
        private void OnEndMoving()
        {
            _ = TrySnap();
            onEndMoving?.Invoke();
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                if (_moveFirstClick)
                {
                    InitStartPoints();
                }

                _IsMoving = true;

                Point RelativeMousePoint = Mouse.GetPosition(this);
                _moveRelativeX = RelativeMousePoint.X;
                _moveRelativeY = RelativeMousePoint.Y;
            }
        }
        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            this._IsMoving = false;
        }
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_IsMoving)
            {
                if (Mouse.LeftButton != MouseButtonState.Pressed)
                {
                    _IsMoving = false;
                    return;
                }
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
            if (_IsMoving)
            {
                _panel_leave_timer.Start();
            }  
        }
        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            if (_IsMoving)
            {
                _panel_leave_timer_cancellation?.Cancel();
                _panel_leave_timer.Stop();
            }
        }
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (!_can_snap || Keyboard.Modifiers != ModifierKeys.Control)
                return;

            foreach (var snap_point in _snap_points)
            {
                if (snap_point.Value == e.Key)
                {
                    SnapTo(snap_point.Key);
                    break;
                }
            }
        }

        private void InitStartPoints()
        {
            GeneralTransform transform = this.TransformToAncestor(this.Parent as Visual);
            Point start_point = transform.Transform(new Point(0, 0));
            _moveStartX = start_point.X;
            _moveStartY = start_point.Y;
            _moveFirstClick = false;
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

            if (_moveFirstClick)
            {
                InitStartPoints();
            }

            var distance_from_start_x = x - _moveStartX;
            var distance_from_start_y = y - _moveStartY;

            var move_transform = base.RenderTransform as TranslateTransform;

            move_transform!.X = distance_from_start_x;
            move_transform!.Y = distance_from_start_y;
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

        public void AddSnapPoint(Point position, Key? shortcutKey = null)
        {
            if (_snap_points.ContainsKey(position))
            {
                _snap_points[position] = shortcutKey;
                return;
            }

            _snap_points.Add(position, shortcutKey);
        }
        private void SnapTo(Point point)
        {
            MoveTo(point.X, point.Y);
        }
        private bool TrySnap()
        {
            if (TryGetSnapMatch(out var snap_point))
            {
                SnapTo(snap_point!.Value);
                return true;
            }

            return false;
        }
        private bool TryGetSnapMatch(out Point? foundedPoint)
        {
            var position = Position;
            double founded_point_distance = double.MaxValue;
            foundedPoint = null;
            
            foreach (Point snap_point in _snap_points.Keys)
            {
                var distance = GetDistanceBetween2Points(position, snap_point);

                if (distance < SnapBound)
                {
                    if (foundedPoint != null) continue;
                    if (founded_point_distance < distance) continue;

                    founded_point_distance = distance;
                    foundedPoint = snap_point;
                }
            }

            return foundedPoint != null;
        }
        private static double GetDistanceBetween2Points(Point point1, Point point2)
        {
            double distance_x = point2.X - point1.X;
            double distance_y = point2.Y - point1.Y;
            return Math.Sqrt(Math.Pow(distance_x, 2) + Math.Pow(distance_y, 2));
        }
        private async void MouseLeaveAttemptCall(object sender, EventArgs args)
        {
            _panel_leave_timer_cancellation?.Cancel();

            _panel_leave_timer_cancellation = new CancellationTokenSource();
            _panel_leave_timer.Stop();
            await MouseLeaveAttempt(_panel_leave_timer_cancellation.Token);
        }
        private async Task MouseLeaveAttempt(CancellationToken cancellationToken)
        {
            var parent = this.Parent as IInputElement;

            for (int i = 0; i < 100; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                var pos = Mouse.GetPosition(parent);
                MoveTo(pos.X, pos.Y);
                await Task.Delay(TimeSpan.FromMilliseconds(0.1));
            }

            this._IsMoving = false;
        }
    }
}
