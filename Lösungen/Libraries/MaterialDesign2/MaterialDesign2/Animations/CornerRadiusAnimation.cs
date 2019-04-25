using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace MaterialDesign2
{
    public class CornerRadiusAnimation : AnimationTimeline
    {
        public static readonly DependencyProperty FromProperty = DependencyProperty.Register("From", typeof(CornerRadius), typeof(CornerRadiusAnimation));
        public CornerRadius? From
        {
            get
            {
                return (CornerRadius)GetValue(FromProperty);
            }
            set
            {
                SetValue(FromProperty, value);
            }
        }
        public static readonly DependencyProperty ToProperty = DependencyProperty.Register("To", typeof(CornerRadius), typeof(CornerRadiusAnimation));
        public CornerRadius? To
        {
            get
            {
                return GetValue(ToProperty) as CornerRadius?;
            }
            set
            {
                SetValue(ToProperty, value);
            }
        }

        public IEasingFunction EasingFunction
        {
            get { return (IEasingFunction)GetValue(EasingFunctionProperty); }
            set { SetValue(EasingFunctionProperty, value); }
        }
        public static readonly DependencyProperty EasingFunctionProperty =
            DependencyProperty.Register("EasingFunction", typeof(IEasingFunction), typeof(SolidColorBrushAnimation), new PropertyMetadata(new QuadraticEase()));

        public Func<double, double> Graph;

        public CornerRadiusAnimation()
        {
            Graph = x => EasingFunction == null ? x : EasingFunction.Ease(x);
        }

        public override Type TargetPropertyType
        {
            get
            {
                return typeof(CornerRadius);
            }
        }

        protected override Freezable CreateInstanceCore()
        {
            return new CornerRadiusAnimation();
        }

        public override object GetCurrentValue(object defaultOriginValue, object defaultDestinationValue, AnimationClock animationClock)
        {
            CornerRadius from = (From ?? (CornerRadius)defaultOriginValue);
            CornerRadius to = (To ?? (CornerRadius)defaultDestinationValue);
            double? progress = animationClock.CurrentProgress.Value;
            double LerpFor(Func<CornerRadius, double> getter) => Lerp(getter(from), getter(to), progress ?? 0);

            return new CornerRadius(LerpFor(x => x.TopLeft), LerpFor(x => x.TopRight), LerpFor(x => x.BottomRight), LerpFor(x => x.BottomLeft));
        }

        private double Lerp(double from, double to, double val)
        {
            return from + (to - from) * val;
        }
    }
}
