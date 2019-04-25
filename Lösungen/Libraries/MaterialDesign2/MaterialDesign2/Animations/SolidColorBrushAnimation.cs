using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace MaterialDesign2
{
    public class SolidColorBrushAnimation : AnimationTimeline
    {
        public static readonly DependencyProperty FromProperty = DependencyProperty.Register("From", typeof(SolidColorBrush), typeof(SolidColorBrushAnimation));
        public SolidColorBrush From
        {
            get
            {
                return (SolidColorBrush)GetValue(FromProperty);
            }
            set
            {
                SetValue(FromProperty, value);
            }
        }
        public static readonly DependencyProperty ToProperty = DependencyProperty.Register("To", typeof(SolidColorBrush), typeof(SolidColorBrushAnimation));
        public SolidColorBrush To
        {
            get
            {
                return (SolidColorBrush)GetValue(ToProperty);
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

        public SolidColorBrushAnimation()
        {
            Graph = x => EasingFunction == null ? x : EasingFunction.Ease(x);
        }

        public override Type TargetPropertyType
        {
            get
            {
                return typeof(SolidColorBrush);
            }
        }

        protected override Freezable CreateInstanceCore()
        {
            return new SolidColorBrushAnimation();
        }

        public override object GetCurrentValue(object defaultOriginValue, object defaultDestinationValue, AnimationClock animationClock)
        {
            SolidColorBrush from = From ?? (SolidColorBrush)defaultOriginValue;
            SolidColorBrush to = To ?? (SolidColorBrush)defaultDestinationValue;
            double? progress = animationClock.CurrentProgress.Value;
            double LerpFor(Func<Color, double> getter) => Lerp(getter(from.Color), getter(to.Color), progress ?? 0);

            return new SolidColorBrush(Color.FromArgb((byte)LerpFor(x => x.A), (byte)LerpFor(x => x.R), (byte)LerpFor(x => x.G), (byte)LerpFor(x => x.B)));
        }

        private double Lerp(double from, double to, double val)
        {
            return from + (to - from) * Graph(val);
        }
    }
}
