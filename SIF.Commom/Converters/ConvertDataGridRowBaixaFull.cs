using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace SIF.Commom.Converters
{
    public class ConvertDataGridRowBaixaFull : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                DateTime crf_datavencimento = System.Convert.ToDateTime(value);

                if (crf_datavencimento.Date < DateTime.Now.Date)
                    return (Brush)new BrushConverter().ConvertFromString("Red");
                if (crf_datavencimento.Date == DateTime.Now.Date)
                    return (Brush)new BrushConverter().ConvertFromString("Blue");
            }
            return (Brush)new BrushConverter().ConvertFromString("Green");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
