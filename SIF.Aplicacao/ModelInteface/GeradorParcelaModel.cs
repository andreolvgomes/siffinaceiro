using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIF.Aplicacao
{
    /// <summary>
    /// 
    /// </summary>
    public class GeradorParcelaModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private string _NumeroDocumento;
        /// <summary>
        /// 
        /// </summary>
        public string NumeroDocumento
        {
            get { return _NumeroDocumento; }
            set
            {
                if (_NumeroDocumento != value)
                {
                    _NumeroDocumento = value;
                    NotifyPropertyChanged("NumeroDocumento");
                }
            }
        }

        private string _FinanceiroSelecionado;
        /// <summary>
        /// 
        /// </summary>
        public string FinanceiroSelecionado
        {
            get { return _FinanceiroSelecionado; }
            set
            {
                if (_FinanceiroSelecionado != value)
                {
                    _FinanceiroSelecionado = value;
                    NotifyPropertyChanged("FinanceiroSelecionado");
                }
            }
        }

        private int _quantidadeParcelas;
        /// <summary>
        /// 
        /// </summary>
        public int QuantidadeParcelas
        {
            get { return _quantidadeParcelas; }
            set
            {
                if (_quantidadeParcelas != value)
                {
                    _quantidadeParcelas = value;
                    NotifyPropertyChanged("QuantidadeParcelas");
                }
            }
        }

        private string _dadaVencimento;
        /// <summary>
        /// 
        /// </summary>
        public string DataVencimento
        {
            get { return _dadaVencimento; }
            set
            {
                if (_dadaVencimento != value)
                {
                    _dadaVencimento = value;
                    NotifyPropertyChanged("DataVencimento");
                }
            }
        }
        
        private string _valorTotal;
        /// <summary>
        /// 
        /// </summary>
        public string ValorTotal
        {
            get { return _valorTotal; }
            set
            {
                if (_valorTotal != value)
                {
                    _valorTotal = value;
                    NotifyPropertyChanged("ValorTotal");
                }
            }
        }

        private string _observavao;
        /// <summary>
        /// 
        /// </summary>
        public string Observacao
        {
            get { return _observavao; }
            set
            {
                if (_observavao != value)
                {
                    _observavao = value;
                    NotifyPropertyChanged("Observacao");
                }
            }
        }
    }
}
