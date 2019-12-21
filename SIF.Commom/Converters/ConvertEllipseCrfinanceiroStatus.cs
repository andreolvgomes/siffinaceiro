using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace SIF.Commom.Converters
{
    public class ConvertEllipseCrfinanceiroStatus : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (value != null)
                {
                    Crfinanceiro crfinanceiro = value as Crfinanceiro;
                    if (crfinanceiro != null)
                    {
                        if(crfinanceiro.Fat_sequencial_dest > 0)
                            return (Brush)new BrushConverter().ConvertFromString("#000033");
                        else if (crfinanceiro.Crf_databaixa != null)
                            return (Brush)new BrushConverter().ConvertFromString("#FF4B0082");
                        if (System.Convert.ToDateTime(crfinanceiro.Crf_datavencimento).Date < DateTime.Now.Date)
                            return (Brush)new BrushConverter().ConvertFromString("Red");
                        if (System.Convert.ToDateTime(crfinanceiro.Crf_datavencimento).Date == DateTime.Now.Date)
                            return (Brush)new BrushConverter().ConvertFromString("Blue");
                    }
                }
            }
            catch
            {
            }
            return (Brush)new BrushConverter().ConvertFromString("Green");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
