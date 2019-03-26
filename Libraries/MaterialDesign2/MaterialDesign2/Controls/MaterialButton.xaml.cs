using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MaterialDesign2.Converters;
using System.Globalization;
using System.Windows.Media.Effects;

namespace MaterialDesign2.Controls
{
    public partial class MaterialButton : Button
    {
        #region Properties
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        public CornerRadius GetCornerRadius() => CornerRadius;
        public void SetCornerRadius(CornerRadius value) => CornerRadius = value;
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.RegisterAttached("CornerRadius", typeof(CornerRadius), typeof(MaterialButton), new FrameworkPropertyMetadata(new CornerRadius(0), FrameworkPropertyMetadataOptions.Inherits));
        
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public string GetText() => Text;
        public void SetText(string value) => Text = value;
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.RegisterAttached("Text", typeof(string), typeof(MaterialButton), new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.Inherits));

        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public ImageSource GetIcon() => Icon;
        public void SetIcon(ImageSource value) => Icon = value;
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.RegisterAttached("Icon", typeof(ImageSource), typeof(MaterialButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

        public GridLength LeftWidth
        {
            get { return (GridLength)GetValue(LeftWidthProperty); }
            set { SetValue(LeftWidthProperty, value); }
        }
        public GridLength GetLeftWidth() => LeftWidth;
        public void SetLeftWidth(GridLength value) => LeftWidth = value;
        public static readonly DependencyProperty LeftWidthProperty =
            DependencyProperty.RegisterAttached("LeftWidth", typeof(GridLength), typeof(MaterialButton), new FrameworkPropertyMetadata(new GridLength(0, GridUnitType.Star), FrameworkPropertyMetadataOptions.Inherits));


        public double LeftMinWidth
        {
            get { return (double)GetValue(LeftMinWidthProperty); }
            set { SetValue(LeftMinWidthProperty, value); }
        }
        public double GetLeftMinWidth() => LeftMinWidth;
        public void SetLeftMinWidth(double value) => LeftMinWidth = value;
        public static readonly DependencyProperty LeftMinWidthProperty =
            DependencyProperty.RegisterAttached("LeftMinWidth", typeof(double), typeof(MaterialButton), new FrameworkPropertyMetadata(16d, FrameworkPropertyMetadataOptions.Inherits));

        public double LeftMaxWidth
        {
            get { return (double)GetValue(LeftMaxWidthProperty); }
            set { SetValue(LeftMaxWidthProperty, value); }
        }
        public double GetLeftMaxWidth() => LeftMaxWidth;
        public void SetLeftMaxWidth(double value) => LeftMaxWidth = value;
        public static readonly DependencyProperty LeftMaxWidthProperty =
            DependencyProperty.RegisterAttached("LeftMaxWidth", typeof(double), typeof(MaterialButton), new FrameworkPropertyMetadata(double.PositiveInfinity, FrameworkPropertyMetadataOptions.Inherits));

        
        public GridLength RightWidth
        {
            get { return (GridLength)GetValue(RightWidthProperty); }
            set { SetValue(RightWidthProperty, value); }
        }
        public GridLength GetRightWidth() => RightWidth;
        public void SetRightWidth(GridLength value) => RightWidth = value;
        public static readonly DependencyProperty RightWidthProperty =
            DependencyProperty.RegisterAttached("RightWidth", typeof(GridLength), typeof(MaterialButton), new FrameworkPropertyMetadata(new GridLength(0, GridUnitType.Star), FrameworkPropertyMetadataOptions.Inherits));

        public double RightMinWidth
        {
            get { return (double)GetValue(RightMinWidthProperty); }
            set { SetValue(RightMinWidthProperty, value); }
        }
        public double GetRightMinWidth() => RightMinWidth;
        public void SetRightMinWidth(double value) => RightMinWidth = value;
        public static readonly DependencyProperty RightMinWidthProperty =
            DependencyProperty.RegisterAttached("RightMinWidth", typeof(double), typeof(MaterialButton), new FrameworkPropertyMetadata(16d, FrameworkPropertyMetadataOptions.Inherits));

        public double RightMaxWidth
        {
            get { return (double)GetValue(RightMaxWidthProperty); }
            set { SetValue(RightMaxWidthProperty, value); }
        }
        public double GetRightMaxWidth() => RightMaxWidth;
        public void SetRightMaxWidth(double value) => RightMaxWidth = value;
        public static readonly DependencyProperty RightMaxWidthProperty =
            DependencyProperty.RegisterAttached("RightMaxWidth", typeof(double), typeof(MaterialButton), new FrameworkPropertyMetadata(double.PositiveInfinity, FrameworkPropertyMetadataOptions.Inherits));


        public TextAlignment TextAlignment
        {
            get { return (TextAlignment)GetValue(TextAlignmentProperty); }
            set { SetValue(TextAlignmentProperty, value); }
        }
        public TextAlignment GetTextAlignment() => TextAlignment;
        public void SetTextAlignment(TextAlignment value) => TextAlignment = value;
        public static readonly DependencyProperty TextAlignmentProperty =
            DependencyProperty.RegisterAttached("TextAlignment", typeof(TextAlignment), typeof(MaterialButton), new FrameworkPropertyMetadata(TextAlignment.Center, FrameworkPropertyMetadataOptions.Inherits));

        public Brush TouchOverlay
        {
            get { return (Brush)GetValue(TouchOverlayProperty); }
            set { SetValue(TouchOverlayProperty, value); }
        }
        public Brush GetTouchOverlay() => TouchOverlay;
        public void SetTouchOverlay(Brush value) => TouchOverlay = value;
        public static readonly DependencyProperty TouchOverlayProperty =
            DependencyProperty.RegisterAttached("TouchOverlay", typeof(Brush), typeof(MaterialButton), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.Inherits));

        public Brush HoverOverlay
        {
            get { return (Brush)GetValue(HoverOverlayProperty); }
            set { SetValue(HoverOverlayProperty, value); }
        }
        public Brush GetHoverOverlay() => HoverOverlay;
        public void SetHoverOverlay(Brush value) => HoverOverlay = value;
        public static readonly DependencyProperty HoverOverlayProperty =
            DependencyProperty.RegisterAttached("HoverOverlay", typeof(Brush), typeof(MaterialButton), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.Inherits));
        
        public double HoverOpacity
        {
            get { return (double)GetValue(HoverOpacityProperty); }
            set { SetValue(HoverOpacityProperty, value); }
        }
        public double GetHoverOpacity() => HoverOpacity;
        public void SetHoverOpacity(double value) => HoverOpacity = value;
        public static readonly DependencyProperty HoverOpacityProperty =
            DependencyProperty.RegisterAttached("HoverOpacity", typeof(double), typeof(MaterialButton), new FrameworkPropertyMetadata(0.08d, FrameworkPropertyMetadataOptions.Inherits));
        
        public double TouchOpacity
        {
            get { return (double)GetValue(TouchOpacityProperty); }
            set { SetValue(TouchOpacityProperty, value); }
        }
        public double GetTouchOpacity() => TouchOpacity;
        public void SetTouchOpacity(double value) => TouchOpacity = value;
        public static readonly DependencyProperty TouchOpacityProperty =
            DependencyProperty.RegisterAttached("TouchOpacity", typeof(double), typeof(MaterialButton), new FrameworkPropertyMetadata(0.16d, FrameworkPropertyMetadataOptions.Inherits));
        
        public double HoverShowDuration
        {
            get { return (double)GetValue(HoverShowDurationProperty); }
            set { SetValue(HoverShowDurationProperty, value); }
        }
        public double GetHoverShowDuration() => HoverShowDuration;
        public void SetHoverShowDuration(double value) => HoverShowDuration = value;
        public static readonly DependencyProperty HoverShowDurationProperty =
            DependencyProperty.RegisterAttached("HoverShowDuration", typeof(double), typeof(MaterialButton), new FrameworkPropertyMetadata(0.03d, FrameworkPropertyMetadataOptions.Inherits));
        
        public double HoverHideDuration
        {
            get { return (double)GetValue(HoverHideDurationProperty); }
            set { SetValue(HoverHideDurationProperty, value); }
        }
        public double GetHoverHideDuration() => HoverHideDuration;
        public void SetHoverHideDuration(double value) => HoverHideDuration = value;
        public static readonly DependencyProperty HoverHideDurationProperty =
            DependencyProperty.RegisterAttached("HoverHideDuration", typeof(double), typeof(MaterialButton), new FrameworkPropertyMetadata(0.03d, FrameworkPropertyMetadataOptions.Inherits));
        
        public double TouchDuration
        {
            get { return (double)GetValue(TouchDurationProperty); }
            set { SetValue(TouchDurationProperty, value); }
        }
        public double GetTouchDuration() => TouchDuration;
        public void SetTouchDuration(double value) => TouchDuration = value;
        public static readonly DependencyProperty TouchDurationProperty =
            DependencyProperty.RegisterAttached("TouchDuration", typeof(double), typeof(MaterialButton), new FrameworkPropertyMetadata(0.5d, FrameworkPropertyMetadataOptions.Inherits));
        
        public double FocusOpacity
        {
            get { return (double)GetValue(FocusOpacityProperty); }
            set { SetValue(FocusOpacityProperty, value); }
        }
        public double GetFocusOpacity() => FocusOpacity;
        public void SetFocusOpacity(double value) => FocusOpacity = value;
        public static readonly DependencyProperty FocusOpacityProperty =
            DependencyProperty.RegisterAttached("FocusOpacity", typeof(double), typeof(MaterialButton), new FrameworkPropertyMetadata(0.24d, FrameworkPropertyMetadataOptions.Inherits));
        
        public IEasingFunction EasingFunction
        {
            get { return (IEasingFunction)GetValue(EasingFunctionProperty); }
            set { SetValue(EasingFunctionProperty, value); }
        }
        public IEasingFunction GetEasingFunction() => EasingFunction;
        public void SetEasingFunction(IEasingFunction value) => EasingFunction = value;
        public static readonly DependencyProperty EasingFunctionProperty =
            DependencyProperty.RegisterAttached("EasingFunction", typeof(IEasingFunction), typeof(MaterialButton), new FrameworkPropertyMetadata(new QuadraticEase(), FrameworkPropertyMetadataOptions.Inherits));
        
        public bool IsDisabled
        {
            get { return (bool)GetValue(IsDisabledProperty); }
            set { SetValue(IsDisabledProperty, value); }
        }
        public bool GetIsDisabled() => IsDisabled;
        public void SetIsDisabled(bool value) => IsDisabled = value;
        public static readonly DependencyProperty IsDisabledProperty =
            DependencyProperty.RegisterAttached("IsDisabled", typeof(bool), typeof(MaterialButton), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback((s, e) =>
            {
                var Button = (MaterialButton)s;
                var Darken = (Rectangle)Button.Template.FindName("Darken", Button);
                DoubleAnimation opacityAnimation = new DoubleAnimation()
                {
                    From = Button.HoverOpacity,
                    To = 0,
                    Duration = TimeSpan.FromSeconds(Button.HoverHideDuration),
                    EasingFunction = Button.EasingFunction,
                };
                Darken.BeginAnimation(OpacityProperty, opacityAnimation);
            })));


        public bool IsToggleButton
        {
            get { return (bool)GetValue(IsToggleButtonProperty); }
            set { SetValue(IsToggleButtonProperty, value); }
        }
        public bool GetIsToggleButton() => IsToggleButton;
        public void SetIsToggleButton(bool value) => IsToggleButton = value;
        public static readonly DependencyProperty IsToggleButtonProperty =
            DependencyProperty.RegisterAttached("IsToggleButton", typeof(bool), typeof(MaterialButton), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));
        
        public bool IsToggledOn
        {
            get { return (bool)GetValue(IsToggledOnProperty); }
            set { SetValue(IsToggledOnProperty, value); }
        }
        public static readonly DependencyProperty IsToggledOnProperty =
            DependencyProperty.RegisterAttached("IsToggledOn", typeof(bool), typeof(MaterialButton), new PropertyMetadata(false, new PropertyChangedCallback(IsToggled_Changed)));

        private static void IsToggled_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MaterialButton materialButton = d as MaterialButton;
            if (materialButton.IsToggleButton)
            {
                var Toggle = (Rectangle)materialButton.Template.FindName("Toggle", materialButton);

                DoubleAnimation opacityAnimation;
                if (materialButton.IsToggledOn)
                {
                    opacityAnimation = new DoubleAnimation()
                    { From = 0, To = materialButton.ToggledOpacity, Duration = TimeSpan.FromSeconds(materialButton.ToggleShowDuration), EasingFunction = materialButton.EasingFunction, };
                }
                else
                {
                    opacityAnimation = new DoubleAnimation()
                    { From = materialButton.ToggledOpacity, To = 0, Duration = TimeSpan.FromSeconds(materialButton.ToggleHideDuration), EasingFunction = materialButton.EasingFunction, };
                }
                Toggle.BeginAnimation(OpacityProperty, opacityAnimation);
            }
        }

        public Brush ToggledOverlay
        {
            get { return (Brush)GetValue(ToggledOverlayProperty); }
            set { SetValue(ToggledOverlayProperty, value); }
        }
        public Brush GetToggledOverlay() => ToggledOverlay;
        public void SetToggledOverlay(Brush value) => ToggledOverlay = value;
        public static readonly DependencyProperty ToggledOverlayProperty =
            DependencyProperty.RegisterAttached("ToggledOverlay", typeof(Brush), typeof(MaterialButton), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.Inherits));

        public double ToggledOpacity
        {
            get { return (double)GetValue(ToggledOpacityProperty); }
            set { SetValue(ToggledOpacityProperty, value); }
        }
        public double GetToggledOpacity() => ToggledOpacity;
        public void SetToggledOpacity(double value) => ToggledOpacity = value;
        public static readonly DependencyProperty ToggledOpacityProperty =
            DependencyProperty.RegisterAttached("ToggledOpacity", typeof(double), typeof(MaterialButton), new FrameworkPropertyMetadata(0.2d, FrameworkPropertyMetadataOptions.Inherits));

        public double ToggleShowDuration
        {
            get { return (double)GetValue(ToggleShowDurationProperty); }
            set { SetValue(ToggleShowDurationProperty, value); }
        }
        public double GetToggleShowDuration() => ToggleShowDuration;
        public void SetToggleShowDuration(double value) => ToggleShowDuration = value;
        public static readonly DependencyProperty ToggleShowDurationProperty =
            DependencyProperty.RegisterAttached("ToggleShowDuration", typeof(double), typeof(MaterialButton), new FrameworkPropertyMetadata(0.1d, FrameworkPropertyMetadataOptions.Inherits));

        public double ToggleHideDuration
        {
            get { return (double)GetValue(ToggleHideDurationProperty); }
            set { SetValue(ToggleHideDurationProperty, value); }
        }
        public double GetToggleHideDuration() => ToggleHideDuration;
        public void SetToggleHideDuration(double value) => ToggleHideDuration = value;
        public static readonly DependencyProperty ToggleHideDurationProperty =
            DependencyProperty.RegisterAttached("ToggleHideDuration", typeof(double), typeof(MaterialButton), new FrameworkPropertyMetadata(0.1d, FrameworkPropertyMetadataOptions.Inherits));
        #endregion

        public MaterialButton()
        {
            InitializeComponent();
            Click += MaterialButton_Click;
            MouseEnter += MaterialButton_MouseEnter;
            MouseLeave += MaterialButton_MouseLeave;
            SizeChanged += MaterialButton_SizeChanged;
            FocusableChanged += MaterialButton_FocusableChanged;
            PreviewMouseDown += MaterialButton_MouseDown;
            PreviewMouseUp += MaterialButton_MouseUp;
        }

        public override void OnApplyTemplate()
        {
            AnimateShadow(DefaultShadowDepth, DefaultShadowOpacity, DefaultShadowRadius, TimeSpan.Zero);

            base.OnApplyTemplate();
        }

        private void MaterialButton_MouseUp(object sender, MouseButtonEventArgs e) 
            => AnimateShadow(HoverShadowDepth, HoverShadowOpacity, HoverShadowRadius, TimeSpan.FromSeconds(HoverShowDuration));

        private void MaterialButton_MouseDown(object sender, MouseButtonEventArgs e) 
            => AnimateShadow(TouchShadowDepth, TouchShadowOpacity, TouchShadowRadius, TimeSpan.FromSeconds(HoverHideDuration));

        private void MaterialButton_SizeChanged(object sender, SizeChangedEventArgs e) 
            => ((VisualBrush)Template.FindName("OpacityBrush", this)).Viewport = new Rect(0, 0, e.NewSize.Width, e.NewSize.Height);
        
        private void MaterialButton_FocusableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var Darken = (Rectangle)Template.FindName("Darken", this);
            TimeSpan duration;
            DoubleAnimation opacityAnimation;

            if (IsFocused)
            {
                opacityAnimation = new DoubleAnimation()
                { From = 0, To = HoverOpacity, Duration = duration = TimeSpan.FromSeconds(HoverShowDuration), EasingFunction = EasingFunction, };
            }
            else
            {
                opacityAnimation = new DoubleAnimation()
                { From = HoverOpacity, To = 0, Duration = duration = TimeSpan.FromSeconds(HoverHideDuration), EasingFunction = EasingFunction, };
            }
            Darken.BeginAnimation(OpacityProperty, opacityAnimation);

            AnimateShadow(FocusShadowDepth, FocusShadowOpacity, FocusShadowRadius, duration);
        }

        private void MaterialButton_MouseEnter(object sender, MouseEventArgs e)
        {
            var Darken = (Rectangle)Template.FindName("Darken", this);
            TimeSpan duration = TimeSpan.FromSeconds(HoverShowDuration);

            DoubleAnimation opacityAnimation = new DoubleAnimation()
            { From = 0, To = HoverOpacity, Duration = duration, EasingFunction = EasingFunction, };

            Darken.BeginAnimation(OpacityProperty, opacityAnimation);

            AnimateShadow(HoverShadowDepth, HoverShadowOpacity, HoverShadowRadius, duration);
        }
        private void MaterialButton_MouseLeave(object sender, MouseEventArgs e)
        {
            var Darken = (Rectangle)Template.FindName("Darken", this);
            TimeSpan duration = TimeSpan.FromSeconds(HoverHideDuration);

            DoubleAnimation opacityAnimation = new DoubleAnimation()
            { From = HoverOpacity, To = 0, Duration = duration, EasingFunction = EasingFunction, };

            Darken.BeginAnimation(OpacityProperty, opacityAnimation);

            AnimateShadow(DefaultShadowDepth, DefaultShadowOpacity, DefaultShadowRadius, duration);
        }

        private void MaterialButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsDisabled) return;

            var Touch = (EllipseGeometry)Template.FindName("Touch", this);

            TimeSpan duration = TimeSpan.FromSeconds(TouchDuration);
            double targetSize = (ActualWidth + ActualHeight);

            DoubleAnimation sizeAnimation = new DoubleAnimation()
            { From = 0, To = targetSize, Duration = duration, EasingFunction = EasingFunction, };

            Touch.BeginAnimation(EllipseGeometry.RadiusXProperty, sizeAnimation);
            Touch.BeginAnimation(EllipseGeometry.RadiusYProperty, sizeAnimation);

            Touch.Center = Mouse.GetPosition(this);

            var TouchPath = (Path)Template.FindName("TouchPath", this);

            DoubleAnimation opacityAnimation = new DoubleAnimation()
            { From = TouchOpacity, To = 0, Duration = duration, EasingFunction = EasingFunction, };
            TouchPath.BeginAnimation(OpacityProperty, opacityAnimation);

            if (IsToggleButton) IsToggledOn = !IsToggledOn;
        }

        #region Shadow Settings

        public bool ShadowEnabled
        {
            get { return (bool)GetValue(ShadowEnabledProperty); }
            set { SetValue(ShadowEnabledProperty, value); }
        }
        public bool GetShadowEnabled() => ShadowEnabled;
        public void SetShadowEnabled(bool value) => ShadowEnabled = value;
        public static readonly DependencyProperty ShadowEnabledProperty =
            DependencyProperty.RegisterAttached("ShadowEnabled", typeof(bool), typeof(MaterialButton), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits, (s, e) =>
            {
                MaterialButton materialButton = s as MaterialButton;
                if (materialButton != null)
                {
                    if (materialButton.ShadowEnabled) materialButton.DropShadow.Opacity = 0;
                    else materialButton.DropShadow.Opacity = materialButton.ShadowOpacity;
                }
            }));

        
        public double DefaultShadowDepth
        {
            get { return (double)GetValue(DefaultShadowDepthProperty); }
            set { SetValue(DefaultShadowDepthProperty, value); }
        }
        public double GetDefaultShadowDepth() => DefaultShadowDepth;
        public void SetDefaultShadowDepth(double value) => DefaultShadowDepth = value;
        public static readonly DependencyProperty DefaultShadowDepthProperty =
            DependencyProperty.RegisterAttached("DefaultShadowDepth", typeof(double), typeof(MaterialButton), new FrameworkPropertyMetadata(2d, FrameworkPropertyMetadataOptions.Inherits));

        public double HoverShadowDepth
        {
            get { return (double)GetValue(HoverShadowDepthProperty); }
            set { SetValue(HoverShadowDepthProperty, value); }
        }
        public double GetHoverShadowDepth() => HoverShadowDepth;
        public void SetHoverShadowDepth(double value) => HoverShadowDepth = value;
        public static readonly DependencyProperty HoverShadowDepthProperty =
            DependencyProperty.RegisterAttached("HoverShadowDepth", typeof(double), typeof(MaterialButton), new FrameworkPropertyMetadata(3d, FrameworkPropertyMetadataOptions.Inherits));

        public double FocusShadowDepth
        {
            get { return (double)GetValue(FocusShadowDepthProperty); }
            set { SetValue(FocusShadowDepthProperty, value); }
        }
        public double GetFocusShadowDepth() => FocusShadowDepth;
        public void SetFocusShadowDepth(double value) => FocusShadowDepth = value;
        public static readonly DependencyProperty FocusShadowDepthProperty =
            DependencyProperty.RegisterAttached("FocusShadowDepth", typeof(double), typeof(MaterialButton), new FrameworkPropertyMetadata(3.5d, FrameworkPropertyMetadataOptions.Inherits));

        public double TouchShadowDepth
        {
            get { return (double)GetValue(TouchShadowDepthProperty); }
            set { SetValue(TouchShadowDepthProperty, value); }
        }
        public double GetTouchShadowDepth() => TouchShadowDepth;
        public void SetTouchShadowDepth(double value) => TouchShadowDepth = value;
        public static readonly DependencyProperty TouchShadowDepthProperty =
            DependencyProperty.RegisterAttached("TouchShadowDepth", typeof(double), typeof(MaterialButton), new FrameworkPropertyMetadata(4d, FrameworkPropertyMetadataOptions.Inherits));


        public double DefaultShadowOpacity
        {
            get { return (double)GetValue(DefaultShadowOpacityProperty); }
            set { SetValue(DefaultShadowOpacityProperty, value); }
        }
        public double GetDefaultShadowOpacity() => DefaultShadowOpacity;
        public void SetDefaultShadowOpacity(double value) => DefaultShadowOpacity = value;
        public static readonly DependencyProperty DefaultShadowOpacityProperty =
            DependencyProperty.RegisterAttached("DefaultShadowOpacity", typeof(double), typeof(MaterialButton), new FrameworkPropertyMetadata(0.3d, FrameworkPropertyMetadataOptions.Inherits));

        public double HoverShadowOpacity
        {
            get { return (double)GetValue(HoverShadowOpacityProperty); }
            set { SetValue(HoverShadowOpacityProperty, value); }
        }
        public double GetHoverShadowOpacity() => HoverShadowOpacity;
        public void SetHoverShadowOpacity(double value) => HoverShadowOpacity = value;
        public static readonly DependencyProperty HoverShadowOpacityProperty =
            DependencyProperty.RegisterAttached("HoverShadowOpacity", typeof(double), typeof(MaterialButton), new FrameworkPropertyMetadata(0.6d, FrameworkPropertyMetadataOptions.Inherits));

        public double FocusShadowOpacity
        {
            get { return (double)GetValue(FocusShadowOpacityProperty); }
            set { SetValue(FocusShadowOpacityProperty, value); }
        }
        public double GetFocusShadowOpacity() => FocusShadowOpacity;
        public void SetFocusShadowOpacity(double value) => FocusShadowOpacity = value;
        public static readonly DependencyProperty FocusShadowOpacityProperty =
            DependencyProperty.RegisterAttached("FocusShadowOpacity", typeof(double), typeof(MaterialButton), new FrameworkPropertyMetadata(0.65d, FrameworkPropertyMetadataOptions.Inherits));

        public double TouchShadowOpacity
        {
            get { return (double)GetValue(TouchShadowOpacityProperty); }
            set { SetValue(TouchShadowOpacityProperty, value); }
        }
        public double GetTouchShadowOpacity() => TouchShadowOpacity;
        public void SetTouchShadowOpacity(double value) => TouchShadowOpacity = value;
        public static readonly DependencyProperty TouchShadowOpacityProperty =
            DependencyProperty.RegisterAttached("TouchShadowOpacity", typeof(double), typeof(MaterialButton), new FrameworkPropertyMetadata(0.7d, FrameworkPropertyMetadataOptions.Inherits));


        private double ShadowOpacity;
        
        public double DefaultShadowRadius
        {
            get { return (double)GetValue(DefaultShadowRadiusProperty); }
            set { SetValue(DefaultShadowRadiusProperty, value); }
        }
        public double GetDefaultShadowRadius() => DefaultShadowRadius;
        public void SetDefaultShadowRadius(double value) => DefaultShadowRadius = value;
        public static readonly DependencyProperty DefaultShadowRadiusProperty =
            DependencyProperty.RegisterAttached("DefaultShadowRadius", typeof(double), typeof(MaterialButton), new FrameworkPropertyMetadata(10d, FrameworkPropertyMetadataOptions.Inherits));

        public double HoverShadowRadius
        {
            get { return (double)GetValue(HoverShadowRadiusProperty); }
            set { SetValue(HoverShadowRadiusProperty, value); }
        }
        public double GetHoverShadowRadius() => HoverShadowRadius;
        public void SetHoverShadowRadius(double value) => HoverShadowRadius = value;
        public static readonly DependencyProperty HoverShadowRadiusProperty =
            DependencyProperty.RegisterAttached("HoverShadowRadius", typeof(double), typeof(MaterialButton), new FrameworkPropertyMetadata(12d, FrameworkPropertyMetadataOptions.Inherits));

        public double FocusShadowRadius
        {
            get { return (double)GetValue(FocusShadowRadiusProperty); }
            set { SetValue(FocusShadowRadiusProperty, value); }
        }
        public double GetFocusShadowRadius() => FocusShadowRadius;
        public void SetFocusShadowRadius(double value) => FocusShadowRadius = value;
        public static readonly DependencyProperty FocusShadowRadiusProperty =
            DependencyProperty.RegisterAttached("FocusShadowRadius", typeof(double), typeof(MaterialButton), new FrameworkPropertyMetadata(12.25d, FrameworkPropertyMetadataOptions.Inherits));

        public double TouchShadowRadius
        {
            get { return (double)GetValue(TouchShadowRadiusProperty); }
            set { SetValue(TouchShadowRadiusProperty, value); }
        }
        public double GetTouchShadowRadius() => TouchShadowRadius;
        public void SetTouchShadowRadius(double value) => TouchShadowRadius = value;
        public static readonly DependencyProperty TouchShadowRadiusProperty =
            DependencyProperty.RegisterAttached("TouchShadowRadius", typeof(double), typeof(MaterialButton), new FrameworkPropertyMetadata(12.5d, FrameworkPropertyMetadataOptions.Inherits));
        #endregion

        public void AnimateShadow(double depth, double opacity, double radius, Duration duration)
        {
            if (!ShadowEnabled)
            {
                DropShadow.Opacity = 0;
                ShadowOpacity = opacity;
            }
            else
            {
                DropShadow.BeginAnimation(DropShadowEffect.OpacityProperty, new DoubleAnimation()
                { From = DropShadow.Opacity, To = opacity, Duration = duration, EasingFunction = EasingFunction, });
            }

            DropShadow.BeginAnimation(DropShadowEffect.ShadowDepthProperty, new DoubleAnimation()
            { From = DropShadow.ShadowDepth, To = depth, Duration = duration, EasingFunction = EasingFunction, });

            DropShadow.BeginAnimation(DropShadowEffect.BlurRadiusProperty, new DoubleAnimation()
            { From = DropShadow.BlurRadius, To = radius, Duration = duration, EasingFunction = EasingFunction, });
        }
    }
}
