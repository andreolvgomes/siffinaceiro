using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SIF.Commom.Converters
{
    /// <summary>
    /// Convert 0 para System.Windows.Visibility.Collapsed e maior que zero System.Windows.Visibility.Visible
    /// </summary>
    public class BooleanToVisibilityZeroCollapsed : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if ((int)value > 0)
                    return System.Windows.Visibility.Visible;
            }
            catch
            {
            }
            return System.Windows.Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
