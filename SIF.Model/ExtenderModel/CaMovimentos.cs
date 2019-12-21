using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIF
{
    public partial class Camovimentos
    {
        private string _FpagamentoDescricao;

        public string FpagamentoDescricao
        {
            get { return _FpagamentoDescricao; }
            set
            {
                if (_FpagamentoDescricao != value)
                {
                    _FpagamentoDescricao = value;
                    NotifyPropertyChanged("FpagamentoDescricao");
                }
            }
        }

        private string _Caixa;

        public string Caixa
        {
            get { return _Caixa; }
            set
            {
                if (_Caixa != value)
                {
                    _Caixa = value;
                    NotifyPropertyChanged("Caixa");
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

        private string _tipoMovimentoEscolhido;
        public string TipoMovimentoEscolhido
        {
            get { return _tipoMovimentoEscolhido; }
            set
            {
                if (_tipoMovimentoEscolhido != value)
                {
                    _tipoMovimentoEscolhido = value;
                    NotifyPropertyChanged("TipoMovimentoEscolhido");
                }
            }
        }

        private string _dataLancamento;
        public string DataLancamento
        {
            get { return _dataLancamento; }
            set
            {
                if (_dataLancamento != value)
                {
                    _dataLancamento = value;
                    NotifyPropertyChanged("DataLancamento");
                }
            }
        }

        private string _Valor = "0,00";

        public string Valor
        {
            get { return _Valor; }
            set
            {
                if (_Valor != value)
                {
                    _Valor = value;
                    NotifyPropertyChanged("Valor");
                }
            }
        }

        //public string Error
        //{
        //    get { return null; }
        //}

        //public string this[string columnName]
        //{
        //    get
        //    {
        //        switch (columnName)
        //        {
        //            case "DataLancamento":
        //                return string.IsNullOrEmpty(DataLancamento) ? "O campo não pode ser vazio" : null;
        //            case "NomeCliente":
        //                return string.IsNullOrEmpty(NomeCliente) ? "O campo não pode ser vazio" : null;
        //            case "Caixa":
        //                return string.IsNullOrEmpty(Caixa) ? "O campo não pode ser vazio" : null;
        //            case "FpagamentoDescricao":
        //                return string.IsNullOrEmpty(FpagamentoDescricao) ? "O campo não pode ser vazio" : null;
        //        }
        //        return null;
        //    }
        //}
    }
}
