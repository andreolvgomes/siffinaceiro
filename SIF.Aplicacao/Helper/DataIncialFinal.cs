using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SIF.Aplicacao
{
    /// <summary>
    /// 
    /// </summary>
    public class DataIncialFinal : INotifyPropertyChanged
    {
        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 
        /// </summary>
        public DataIncialFinal()
        {
            this._dataInicial = DateTime.Now.ConvertToString();
            this._dataFinal = DateTime.Now.ConvertToString();
        }

        private string _dataInicial;
        /// <summary>
        /// 
        /// </summary>
        public string DataInicial
        {
            get { return _dataInicial; }
            set 
            {
                if (_dataInicial != value)
                {
                    _dataInicial = value;
                    NotifyPropertyChanged("DataInicial");


                }
            }
        }

        private string _dataFinal;

        /// <summary>
        /// 
        /// </summary>
        public string DataFinal
        {
            get { return _dataFinal; }
            set
            {
                if (_dataFinal != value)
                {
                    _dataFinal = value;
                    NotifyPropertyChanged("DataFinal");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime DateTimeIncial
        {
            get
            {
                return Convert.ToDateTime(_dataInicial);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime DateTimeFinal
        {
            get
            {
                return Convert.ToDateTime(_dataFinal);
            }
        }
    }
}
