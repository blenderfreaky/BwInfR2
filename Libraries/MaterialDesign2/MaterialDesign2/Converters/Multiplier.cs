using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace MaterialDesign2.Converters
{
    public class DoubleMultiplier : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value * (double)parameter;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value / (double)parameter;
        }
    }

    public class ThicknessMultiplier : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Thickness valueT = (Thickness)value;
            Thickness parameterT = (Thickness)parameter;
            return new Thickness(valueT.Left * parameterT.Left, valueT.Top * parameterT.Top, valueT.Right * parameterT.Right, valueT.Bottom * parameterT.Bottom);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Thickness valueT = (Thickness)value;
            Thickness parameterT = (Thickness)parameter;
            return new Thickness(valueT.Left / parameterT.Left, valueT.Top / parameterT.Top, valueT.Right / parameterT.Right, valueT.Bottom / parameterT.Bottom);
        }
    }
    public class ThicknessDoubleMultiplier : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Thickness valueT = (Thickness)value;
            double parameterT = (double)parameter;
            return new Thickness(valueT.Left * parameterT, valueT.Top * parameterT, valueT.Right * parameterT, valueT.Bottom * parameterT);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Thickness valueT = (Thickness)value;
            double parameterT = (double)parameter;
            return new Thickness(valueT.Left / parameterT, valueT.Top / parameterT, valueT.Right / parameterT, valueT.Bottom / parameterT);
        }
    }

    public class CornerRadiusMultiplier : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            CornerRadius valueT = (CornerRadius)value;
            CornerRadius parameterT = (CornerRadius)parameter;
            return new CornerRadius(valueT.TopLeft * parameterT.TopLeft, valueT.TopRight * parameterT.TopRight, valueT.BottomRight * parameterT.BottomRight, valueT.BottomLeft * parameterT.BottomLeft);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            CornerRadius valueT = (CornerRadius)value;
            CornerRadius parameterT = (CornerRadius)parameter;
            return new CornerRadius(valueT.TopLeft / parameterT.TopLeft, valueT.TopRight / parameterT.TopRight, valueT.BottomRight / parameterT.BottomRight, valueT.BottomLeft / parameterT.BottomLeft);
        }
    }
    public class CornerRadiusDoubleMultiplier : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            CornerRadius valueT = (CornerRadius)value;
            double parameterT = (double)parameter;
            return new CornerRadius(valueT.TopLeft * parameterT, valueT.TopRight * parameterT, valueT.BottomRight * parameterT, valueT.BottomLeft * parameterT);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            CornerRadius valueT = (CornerRadius)value;
            double parameterT = (double)parameter;
            return new CornerRadius(valueT.TopLeft / parameterT, valueT.TopRight / parameterT, valueT.BottomRight / parameterT, valueT.BottomLeft / parameterT);
        }
    }
}
