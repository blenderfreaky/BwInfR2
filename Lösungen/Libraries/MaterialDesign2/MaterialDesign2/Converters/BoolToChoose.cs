using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace MaterialDesign2.Converters
{
    public class BoolToChoose : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            //throw new Exception($"{values[0].GetType()} and {values[1].GetType()} and {values[2].GetType()}");
            return values[2] is bool condition && condition ? values[0] : values[1];
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            return targetType.Select(x => Binding.DoNothing).ToArray();
        }
    }
}
