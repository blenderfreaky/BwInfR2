using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
/*
namespace MaterialDesign2.Controls
{
    public partial class MaterialTooltip : ToolTip
    {
        static MaterialTooltip()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MaterialTooltip), new FrameworkPropertyMetadata(typeof(MaterialTooltip)));

            ContentProperty.OverrideMetadata(typeof(MaterialTooltip), new FrameworkPropertyMetadata(new PropertyChangedCallback(OnContentChanged)));
        }

        public MaterialTooltip() : base()
        {
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        private static void OnContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MaterialTooltip materialTooltip && materialTooltip.GetTemplateChild("frame") is Border frame)
            {
                frame.CornerRadius = new CornerRadius(Math.Min(frame.Width, frame.Height)/2d);
            }
        }
    }
}
*/