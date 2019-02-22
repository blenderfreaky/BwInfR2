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
    public class GridLengthAnimation : AnimationTimeline
    {
        public static readonly DependencyProperty FromProperty = DependencyProperty.Register("From", typeof(GridLength?), typeof(GridLengthAnimation));
        public GridLength? From
        {
            get
            {
                return (GridLength?)GetValue(FromProperty);
            }
            set
            {
                SetValue(FromProperty, value);
            }
        }
        public static readonly DependencyProperty ToProperty = DependencyProperty.Register("To", typeof(GridLength?), typeof(GridLengthAnimation));
        public GridLength? To
        {
            get
            {
                return (GridLength?)GetValue(ToProperty);
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
            DependencyProperty.Register("EasingFunction", typeof(IEasingFunction), typeof(GridLengthAnimation), new PropertyMetadata(new QuadraticEase()));

        public Func<double, double> Graph;

        public GridLengthAnimation()
        {
            Graph = x => EasingFunction == null ? x : EasingFunction.Ease(x);
        }

        public override Type TargetPropertyType
        {
            get
            {
                return typeof(GridLength);
            }
        }

        protected override Freezable CreateInstanceCore()
        {
            return new GridLengthAnimation();
        }

        public override object GetCurrentValue(object defaultOriginValue, object defaultDestinationValue, AnimationClock animationClock)
        {
            GridLength from = From ?? (GridLength)defaultOriginValue;
            GridLength to = To ?? (GridLength)defaultDestinationValue;
            double? progress = animationClock.CurrentProgress.Value;

            return new GridLength(Lerp(from.Value, to.Value, progress.Value), to.GridUnitType);
        }

        private double Lerp(double from, double to, double val)
        {
            return from + (to - from) * Graph(val);
        }
    }
}
