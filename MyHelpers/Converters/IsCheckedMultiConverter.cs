using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Data;

namespace Pavlo.MyHelpers.Converters
{
    /// <summary>
    /// Convert 2 string to the bool value. True - if strings are equal.
    /// </summary>
    public class IsCheckedMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            //two string values are expected
            string s0 = values[0] as string;
            string s1 = values[1] as string;
            return s0.ToLower() == s1.ToLower();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
