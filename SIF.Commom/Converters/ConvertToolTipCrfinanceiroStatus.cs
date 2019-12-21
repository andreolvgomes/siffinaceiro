using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SIF.Commom.Converters
{
    public class ConvertToolTipCrfinanceiroStatus : IValueConverter
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
                        //if (crfinanceiro.Fat_sequencial_dest > 0)
                        //    string.Format("Faturada → destino : {0}", crfinanceiro.Fat_sequencial_dest);
                        //if(crfinanceiro.Fat_sequencial_origem > 0)
                        if (crfinanceiro.Fat_sequencial_dest > 0)
                            return string.Format("Conta Faturada, Nº da Fatura = {0}", crfinanceiro.Fat_sequencial_dest);
                        else if (crfinanceiro.Crf_databaixa != null)
                            return string.Format("Conta Baixada em {0:dd/MM/yyyy}", crfinanceiro.Crf_databaixa);
                        if (System.Convert.ToDateTime(crfinanceiro.Crf_datavencimento).Date < DateTime.Now.Date)
                            return string.Format("Conta Vencida a {0} dia(s)", DateTime.Now.Date.Subtract(System.Convert.ToDateTime(crfinanceiro.Crf_datavencimento)).Days);
                        if (System.Convert.ToDateTime(crfinanceiro.Crf_datavencimento).Date == DateTime.Now.Date)
                            return string.Format("Conta Vence hoje, lançamento feito em {0:dd/MM/yyyy}", crfinanceiro.Crf_datalancamento);

                        return string.Format("Conta em dias, lançamento feito em {0:dd/MM/yyyy}, ainda faltam {1} dia(s) para vencer", crfinanceiro.Crf_datalancamento, System.Convert.ToDateTime(crfinanceiro.Crf_datavencimento).Date.Subtract(DateTime.Now.Date).Days);
                    }
                }
            }
            catch
            {
            }
            return "OK,,, Programar um Status";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
