using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace MaterialDesign2.Controls
{
    public partial class MaterialWindow : Window
    {
        public Brush LabelBrush
        {
            get { return (Brush)GetValue(LabelBrushProperty); }
            set { SetValue(LabelBrushProperty, value); }
        }
        public Brush GetLabelBrush() => LabelBrush;
        public void SetLabelBrush(Brush value) => LabelBrush = value;
        public static readonly DependencyProperty LabelBrushProperty =
            DependencyProperty.RegisterAttached("LabelBrush", typeof(Brush), typeof(MaterialWindow), new FrameworkPropertyMetadata(Brushes.Gray, FrameworkPropertyMetadataOptions.Inherits));

        public bool MaximizeEnabled
        {
            get { return (bool)GetValue(MaximizeEnabledProperty); }
            set { SetValue(MaximizeEnabledProperty, value); }
        }
        public bool GetMaximizeEnabled() => MaximizeEnabled;
        public void SetMaximizeEnabled(bool value) => MaximizeEnabled = value;
        public static readonly DependencyProperty MaximizeEnabledProperty =
            DependencyProperty.RegisterAttached("MaximizeEnabled", typeof(bool), typeof(MaterialWindow), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.Inherits, (s, e) => { ((MaterialWindow)s).ResizeMode = ((bool)e.NewValue) ? ResizeMode.CanResize : ResizeMode.NoResize; }));

        public bool MinimizeEnabled
        {
            get { return (bool)GetValue(MinimizeEnabledProperty); }
            set { SetValue(MinimizeEnabledProperty, value); }
        }
        public bool GetMinimizeEnabled() => MinimizeEnabled;
        public void SetMinimizeEnabled(bool value) => MinimizeEnabled = value;
        public static readonly DependencyProperty MinimizeEnabledProperty =
            DependencyProperty.RegisterAttached("MinimizeEnabled", typeof(bool), typeof(MaterialWindow), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.Inherits));

        public bool CloseEnabled
        {
            get { return (bool)GetValue(CloseEnabledProperty); }
            set { SetValue(CloseEnabledProperty, value); }
        }
        public bool GetCloseEnabled() => CloseEnabled;
        public void SetCloseEnabled(bool value) => CloseEnabled = value;
        public static readonly DependencyProperty CloseEnabledProperty =
            DependencyProperty.RegisterAttached("CloseEnabled", typeof(bool), typeof(MaterialWindow), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.Inherits));
        /*
        public Thickness FrameMargin
        {
            get { return (Thickness)GetValue(FrameMarginProperty); }
            set { SetValue(FrameMarginProperty, value); }
        }
        public static readonly DependencyProperty FrameMarginProperty =
            DependencyProperty.Register("FrameMargin", typeof(Thickness), typeof(MaterialWindow), new PropertyMetadata(new Thickness(0)));
        */
        public Effect FrameEffect
        {
            get { return (Effect)GetValue(FrameEffectProperty); }
            set { SetValue(FrameEffectProperty, value); }
        }
        public Effect GetFrameEffect() => FrameEffect;
        public void SetFrameEffect(Effect value) => FrameEffect = value;
        public static readonly DependencyProperty FrameEffectProperty =
            DependencyProperty.RegisterAttached("FrameEffect", typeof(Effect), typeof(MaterialWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

        public Effect TopBarEffect
        {
            get { return (Effect)GetValue(TopBarEffectProperty); }
            set { SetValue(TopBarEffectProperty, value); }
        }
        public Effect GetTobBarEffect() => TopBarEffect;
        public void SetTobBarEffect(Effect value) => TopBarEffect = value;
        public static readonly DependencyProperty TopBarEffectProperty =
            DependencyProperty.RegisterAttached("TopBarEffect", typeof(Effect), typeof(MaterialWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

        static MaterialWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MaterialWindow), new FrameworkPropertyMetadata(typeof(MaterialWindow)));
        }

        public MaterialWindow() : base()
        {
            //PreviewMouseMove += OnPreviewMouseMove;
            OnStateChanged(null);
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                if (GetTemplateChild("backboard") is Border backboard) backboard.Margin = new Thickness(7);
                //FrameMargin = new Thickness(7);
                if (GetTemplateChild("restoreButton") is MaterialButton restoreButton && restoreButton.Content is Grid grid)
                {
                    if (grid.GetChildOfType<Rectangle>() is Rectangle maximizeIconNormal) maximizeIconNormal.Visibility = Visibility.Collapsed;
                    if (grid.GetChildOfType<Grid>() is Grid maximizeIconMaximized) maximizeIconMaximized.Visibility = Visibility.Visible;
                }
            }
            else
            {
                if (GetTemplateChild("backboard") is Border backboard) backboard.Margin = new Thickness(0);
                //FrameMargin = new Thickness(15, 20, 15, 15);
                if (GetTemplateChild("restoreButton") is MaterialButton restoreButton && restoreButton.Content is Grid grid)
                {
                    if (grid.GetChildOfType<Rectangle>() is Rectangle maximizeIconNormal) maximizeIconNormal.Visibility = Visibility.Visible;
                    if (grid.GetChildOfType<Grid>() is Grid maximizeIconMaximized) maximizeIconMaximized.Visibility = Visibility.Collapsed;
                }
            }
            if (e != null) base.OnStateChanged(e);
        }

        public override void OnApplyTemplate()
        {
            if (GetTemplateChild("moveRectangle") is Border moveRectangle)
            {
                moveRectangle.PreviewMouseDown += MoveRectangle_PreviewMouseDown;
                moveRectangle.PreviewMouseMove += MoveRectangle_PreviewMouseMove;
            }
            if (GetTemplateChild("minimizeButton") is Button minimizeButton) minimizeButton.Click += MinimizeClick;
            if (GetTemplateChild("restoreButton") is Button restoreButton) restoreButton.Click += RestoreClick;
            if (GetTemplateChild("closeButton") is Button closeButton) closeButton.Click += CloseClick;
            /*if (GetTemplateChild("resizeGrid") is Grid resizeGrid)
            {
                foreach (UIElement element in resizeGrid.Children)
                {
                    if (element is Rectangle resizeRectangle)
                    {
                        resizeRectangle.PreviewMouseDown += ResizeRectangle_PreviewMouseDown;
                        resizeRectangle.MouseMove += ResizeRectangle_MouseMove;
                    }
                }
            }*/
            /*Left -= FrameMargin.Left;
            Top -= FrameMargin.Top;
            Width += FrameMargin.Left + FrameMargin.Right;
            Height += FrameMargin.Top + FrameMargin.Bottom;*/
            base.OnApplyTemplate();
        }

        private void MoveRectangle_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed && WindowState == WindowState.Maximized)
            {
                double FullWidth = ActualWidth;
                double lambda = e.GetPosition(this).X / FullWidth;

                WindowState = WindowState.Normal;

                Left = (FullWidth - Width) * lambda;
                Top = e.GetPosition(this).Y - 25;
                MoveRectangle_PreviewMouseDown(sender, null);
            }
        }

        private void MoveRectangle_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed) DragMove();
        }
        /*
        protected void ResizeRectangle_MouseMove(object sender, MouseEventArgs e)
        {
            if (WindowState == WindowState.Maximized) return;
            Rectangle rectangle = sender as Rectangle;
            switch (rectangle.Name)
            {
                case "top":
                    Cursor = Cursors.SizeNS;
                    break;
                case "bottom":
                    Cursor = Cursors.SizeNS;
                    break;
                case "left":
                    Cursor = Cursors.SizeWE;
                    break;
                case "right":
                    Cursor = Cursors.SizeWE;
                    break;
                case "topLeft":
                    Cursor = Cursors.SizeNWSE;
                    break;
                case "topRight":
                    Cursor = Cursors.SizeNESW;
                    break;
                case "bottomLeft":
                    Cursor = Cursors.SizeNESW;
                    break;
                case "bottomRight":
                    Cursor = Cursors.SizeNWSE;
                    break;
                default:
                    break;
            }
        }
        
        protected void OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton != MouseButtonState.Pressed) Cursor = Cursors.Arrow;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, UInt32 msg, IntPtr wParam, IntPtr lParam);

        protected void ResizeRectangle_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (WindowState == WindowState.Maximized) return;
            Rectangle rectangle = sender as Rectangle;
            switch (rectangle.Name)
            {
                case "top":
                    Cursor = Cursors.SizeNS;
                    ResizeWindow(ResizeDirection.Top);
                    break;
                case "bottom":
                    Cursor = Cursors.SizeNS;
                    ResizeWindow(ResizeDirection.Bottom);
                    break;
                case "left":
                    Cursor = Cursors.SizeWE;
                    ResizeWindow(ResizeDirection.Left);
                    break;
                case "right":
                    Cursor = Cursors.SizeWE;
                    ResizeWindow(ResizeDirection.Right);
                    break;
                case "topLeft":
                    Cursor = Cursors.SizeNWSE;
                    ResizeWindow(ResizeDirection.TopLeft);
                    break;
                case "topRight":
                    Cursor = Cursors.SizeNESW;
                    ResizeWindow(ResizeDirection.TopRight);
                    break;
                case "bottomLeft":
                    Cursor = Cursors.SizeNESW;
                    ResizeWindow(ResizeDirection.BottomLeft);
                    break;
                case "bottomRight":
                    Cursor = Cursors.SizeNWSE;
                    ResizeWindow(ResizeDirection.BottomRight);
                    break;
                default:
                    break;
            }
        }
        
        private HwndSource _hwndSource;

        protected override void OnInitialized(EventArgs e)
        {
            SourceInitialized += OnSourceInitialized;
            base.OnInitialized(e);
        }

        private void OnSourceInitialized(object sender, EventArgs e)
        {
            _hwndSource = (HwndSource)PresentationSource.FromVisual(this);
        }

        private void ResizeWindow(ResizeDirection direction)
        {
            SendMessage(_hwndSource.Handle, 0x112, (IntPtr)(61440 + direction), IntPtr.Zero);
        }*/

        #region Click events
        protected void MinimizeClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        protected void RestoreClick(object sender, RoutedEventArgs e)
        {
            WindowState = (WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;
        }

        protected void CloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion
    }
}
