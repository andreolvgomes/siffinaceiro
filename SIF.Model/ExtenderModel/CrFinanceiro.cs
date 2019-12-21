using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIF
{
    public partial class Crfinanceiro
    {
        private string _Crf_sequencialString;

        public string Crf_sequencialString
        {
            get { return _Crf_sequencialString; }
            set
            {
                if (_Crf_sequencialString != value)
                {
                    _Crf_sequencialString = value;
                    NotifyPropertyChanged("Crf_sequencialString");
                }
            }
        }

        private string _Vencimento;
        public string Vencimento
        {
            get { return _Vencimento; }
            set
            {
                if (_Vencimento != value)
                {
                    _Vencimento = value;
                    NotifyPropertyChanged("Vencimento");
                }
            }
        }

        private string _ValorParcela = "0,00";

        public string ValorParcela
        {
            get { return _ValorParcela; }
            set
            {
                if (_ValorParcela != value)
                {
                    _ValorParcela = value;
                    NotifyPropertyChanged("ValorParcela");
                }
            }
        }

        private string _valorAReceber = "0,00";

        public string ValorAReceber
        {
            get { return _valorAReceber; }
            set
            {
                if (_valorAReceber != value)
                {
                    _valorAReceber = value;
                    NotifyPropertyChanged("ValorAReceber");
                }
            }
        }
        
        private string _NomeCliente;

        public string NomeCliente
        {
            get { return _NomeCliente; }
            set
            {
                if (_NomeCliente != value)
                {
                    _NomeCliente = value;
                    NotifyPropertyChanged("NomeCliente");
                }
            }
        }

        private string _descricaoFpagamento;
        public string DescricaoFpagamento
        {
            get { return _descricaoFpagamento; }
            set
            {
                if (_descricaoFpagamento != value)
                {
                    _descricaoFpagamento = value;
                    NotifyPropertyChanged("DescricaoFpagamento");
                }
            }
        }
    }
}
