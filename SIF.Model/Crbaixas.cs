using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIF
{
    public partial class Crbaixas : IModelProperty, IDataErrorInfo
    {
        public override string NomeTabela
        {
            get 
            {
                return "Crbaixas";
            }
        }

        private int _Crb_sequencial;
        [CamposSqlAttibutes("Crb_sequencial", IsPrimaryKey = true, IsIdentity = true)]
        public int Crb_sequencial
        {
            get { return _Crb_sequencial; }
            set
            {
                if (_Crb_sequencial != value)
                {
                    _Crb_sequencial = value;
                    NotifyPropertyChanged("Crb_sequencial");
                }
            }
        }

        private int _Crf_sequencial;
        [CamposSqlAttibutes("Crf_sequencial")]
        public int Crf_sequencial
        {
            get { return _Crf_sequencial; }
            set
            {
                if (_Crf_sequencial != value)
                {
                    _Crf_sequencial = value;
                    NotifyPropertyChanged("Crf_sequencial");
                }
            }
        }

        private string _Fpa_codigo;
        [CamposSqlAttibutes("Fpa_codigo")]
        public string Fpa_codigo
        {
            get { return _Fpa_codigo; }
            set
            {
                if (_Fpa_codigo != value)
                {
                    _Fpa_codigo = value;
                    NotifyPropertyChanged("Fpa_codigo");
                }
            }
        }

        private string _Crb_tipodoconta;
        [CamposSqlAttibutes("Crb_tipodoconta")]
        public string Crb_tipodoconta
        {
            get { return _Crb_tipodoconta; }
            set
            {
                if (_Crb_tipodoconta != value)
                {
                    _Crb_tipodoconta = value;
                    NotifyPropertyChanged("Crb_tipodoconta");
                }
            }
        }

        private string _Pla_numeroconta;
        [CamposSqlAttibutes("Pla_numeroconta")]
        public string Pla_numeroconta
        {
            get { return _Pla_numeroconta; }
            set
            {
                if (_Pla_numeroconta != value)
                {
                    _Pla_numeroconta = value;
                    NotifyPropertyChanged("Pla_numeroconta");
                }
            }
        }

        private string _Cai_codigo;
        [CamposSqlAttibutes("Cai_codigo")]
        public string Cai_codigo
        {
            get { return _Cai_codigo; }
            set
            {
                if (_Cai_codigo != value)
                {
                    _Cai_codigo = value;
                    NotifyPropertyChanged("Cai_codigo");
                }
            }
        }

        private decimal _Crb_valorecebido;
        [CamposSqlAttibutes("Crb_valorecebido")]
        public decimal Crb_valorecebido
        {
            get { return _Crb_valorecebido; }
            set
            {
                if (_Crb_valorecebido != value)
                {
                    _Crb_valorecebido = value;
                    NotifyPropertyChanged("Crb_valorecebido");
                }
            }
        }

        private DateTime _Crb_databaixa;
        [CamposSqlAttibutes("Crb_databaixa")]
        public DateTime Crb_databaixa
        {
            get { return _Crb_databaixa; }
            set
            {
                if (_Crb_databaixa != value)
                {
                    _Crb_databaixa = value;
                    NotifyPropertyChanged("Crb_databaixa");
                }
            }
        }

        private string _Crb_observacao;
        [CamposSqlAttibutes("Crb_observacao")]
        public string Crb_observacao
        {
            get { return _Crb_observacao; }
            set
            {
                if (_Crb_observacao != value)
                {
                    _Crb_observacao = value;
                    NotifyPropertyChanged("Crb_observacao");
                }
            }
        }

        public string Error
        {
            get { return null; }
        }

        public string this[string columnName]
        {
            get 
            {
                switch (columnName)
                {
                    case "Pla_numeroconta":
                        return string.IsNullOrEmpty(Pla_numeroconta) ? "O campo não pode ser vazio" : null;
                    case "CodigoConta":
                        return string.IsNullOrEmpty(CodigoConta) ? "O campo não pode ser vazio" : null;
                    case "DescricaoFpagamento":
                        return string.IsNullOrEmpty(DescricaoFpagamento) ? "O campo não pode ser vazio" : null;
                    case "DescricaoCaixa":
                        return string.IsNullOrEmpty(DescricaoCaixa) ? "O campo não pode ser vazio" : null;
                    case "ValorBaixa":
                        return string.IsNullOrEmpty(ValorBaixa) ? "O campo não pode ser vazio" : null;
                    case "DataBaixa":
                        return string.IsNullOrEmpty(DataBaixa) ? "O campo não pode ser vazio" : null;
                    //case "Cli_cidade":
                    //    return string.IsNullOrEmpty(Cli_cidade) ? "O campo não pode ser vazio" : null;
                    //case "Cli_cidade":
                    //    return string.IsNullOrEmpty(Cli_cidade) ? "O campo não pode ser vazio" : null;
                }
                return null;
            }
        }
    }
}
