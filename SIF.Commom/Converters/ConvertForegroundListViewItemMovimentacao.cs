using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SIF.Commom.Converters
{
    public class ConvertForegroundListViewItemMovimentacao : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if("Crédito".Equals(value))
                    return new System.Windows.Media.BrushConverter().ConvertFromString("Blue");
            }
            catch
            {
            }
            return new System.Windows.Media.BrushConverter().ConvertFromString("Red");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
