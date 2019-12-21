using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIF
{
    public partial class Crbaixas
    {
        private string _dataBaixa;

        public string DataBaixa
        {
            get { return _dataBaixa; }
            set
            {
                if (_dataBaixa != value)
                {
                    _dataBaixa = value;
                    NotifyPropertyChanged("DataBaixa");
                }
            }
        }

        private string _valorBaixa = "0,00";

        public string ValorBaixa
        {
            get { return _valorBaixa; }
            set
            {
                if (_valorBaixa != value)
                {
                    _valorBaixa = value;
                    NotifyPropertyChanged("ValorBaixa");
                }
            }
        }

        private string _descricaoCaixa;

        public string DescricaoCaixa
        {
            get { return _descricaoCaixa; }
            set
            {
                if (_descricaoCaixa != value)
                {
                    _descricaoCaixa = value;
                    NotifyPropertyChanged("DescricaoCaixa");
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

        private string _codigoConta;

        public string CodigoConta
        {
            get { return _codigoConta; }
                        set
            {
                if (_codigoConta != value)
                {
                    _codigoConta = value;
                    NotifyPropertyChanged("CodigoConta");
                }
            }
        }
        
    }
}
