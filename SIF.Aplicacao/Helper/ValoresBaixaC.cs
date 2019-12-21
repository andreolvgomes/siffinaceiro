using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIF.Aplicacao.Helper
{
    public class ValoresBaixaC : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private decimal _totalGeral;

        public decimal TotalGeral
        {
            get { return _totalGeral; }
            set
            {
                if (_totalGeral != value)
                {
                    _totalGeral = value;
                    NotifyPropertyChanged("TotalGeral");
                }
            }
        }

        private decimal _totalEmDias;

        public decimal TotalEmDias
        {
            get { return _totalEmDias; }
            set 
            {
                if (_totalEmDias != value)
                {
                    _totalEmDias = value;
                    NotifyPropertyChanged("TotalEmDias");
                }
            }
        }
        
        private decimal _totalVecidas;

        public decimal TotalVencidas
        {
            get { return _totalVecidas; }
            set
            {
                if (_totalVecidas != value)
                {
                    _totalVecidas = value;
                    NotifyPropertyChanged("TotalVencidas");
                }
            }
        }

        internal void Reset()
        {
            this.TotalEmDias = 0;
            this.TotalGeral = 0;
            this.TotalVencidas = 0;
        }
    }
}
