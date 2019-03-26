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
}
