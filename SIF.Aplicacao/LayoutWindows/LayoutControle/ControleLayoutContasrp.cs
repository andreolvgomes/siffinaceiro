using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SIF.Aplicacao.LayoutControle
{
    public class ControleLayoutContasrp : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public ImageSource Logo
        {
            get
            {
                if (_interf == ContasReceberPagar.ContasPagar)
                    return new BitmapImage(new Uri("/Icones/contaspagar.png", UriKind.Relative));

                return new BitmapImage(new Uri("/Icones/contasreceber.png", UriKind.Relative));
            }
        }

        public string DescricaoValor
        {
            get
            {
                if (_interf == ContasReceberPagar.ContasPagar)
                    return "Valor a pagar";

                return "Valor a receber";
            }
        }

        public string Titulo
        {
            get
            {
                if (_interf == ContasReceberPagar.ContasPagar)
                    return "CONTAS A PAGAR";
                return "CONTAS A RECEBER";
            }
        }

        public ContasReceberPagar IntefaceLayout
        {
            get
            {
                return _interf;
            }
        }

        private ContasReceberPagar _interf;

        public ControleLayoutContasrp(ContasReceberPagar interf)
        {
            this._interf = interf;
        }
    }
}
