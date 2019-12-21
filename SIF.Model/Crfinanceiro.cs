using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIF
{
    public partial class Crfinanceiro : IModelProperty, IDataErrorInfo
    {
        public override string NomeTabela
        {
            get { return "Crfinanceiro"; }
        }

        private int _Crf_sequencial;
        [CamposSqlAttibutes("Crf_sequencial", IsPrimaryKey = true, IsIdentity = true)]
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

        private int _Fat_sequencial_origem;
        [CamposSqlAttibutes("Fat_sequencial_origem")]
        public int Fat_sequencial_origem
        {
            get { return _Fat_sequencial_origem; }
            set
            {
                if (_Fat_sequencial_origem != value)
                {
                    _Fat_sequencial_origem = value;
                    NotifyPropertyChanged("Fat_sequencial_origem");
                }
            }
        }
        
        private int _Fat_sequencial_dest;
        [CamposSqlAttibutes("Fat_sequencial_dest")]
        public int Fat_sequencial_dest
        {
            get { return _Fat_sequencial_dest; }
            set
            {
                if (_Fat_sequencial_dest != value)
                {
                    _Fat_sequencial_dest = value;
                    NotifyPropertyChanged("Fat_sequencial_dest");
                }
            }
        }

        partial void OnFat_sequencial_destChanged();

        private decimal _Crf_valorareceber;
        [CamposSqlAttibutes("Crf_valorareceber")]
        public decimal Crf_valorareceber
        {
            get { return _Crf_valorareceber; }
            set
            {
                if (_Crf_valorareceber != value)
                {
                    _Crf_valorareceber = value;
                    NotifyPropertyChanged("Crf_valorareceber");
                    this.OnCrf_valorareceberChanged();
                }
            }
        }

        partial void OnCrf_valorareceberChanged();

        private int _Cli_codigo;
        [CamposSqlAttibutes("Cli_codigo")]
        public int Cli_codigo
        {
            get { return _Cli_codigo; }
            set
            {
                if (_Cli_codigo != value)
                {
                    _Cli_codigo = value;
                    NotifyPropertyChanged("Cli_codigo");
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

        private string _Crf_ndocumento;
        [CamposSqlAttibutes("Crf_ndocumento")]
        public string Crf_ndocumento
        {
            get { return _Crf_ndocumento; }
            set
            {
                if (_Crf_ndocumento != value)
                {
                    _Crf_ndocumento = value;
                    NotifyPropertyChanged("Crf_ndocumento");
                }
            }
        }

        private string _Crf_tipoconta;
        [CamposSqlAttibutes("Crf_tipoconta")]
        public string Crf_tipoconta
        {
            get { return _Crf_tipoconta; }
            set
            {
                if (_Crf_tipoconta != value)
                {
                    _Crf_tipoconta = value;
                    NotifyPropertyChanged("Crf_tipoconta");
                }
            }
        }

        private string _Crf_parcela;
        [CamposSqlAttibutes("Crf_parcela")]
        public string Crf_parcela
        {
            get { return _Crf_parcela; }
            set
            {
                if (_Crf_parcela != value)
                {
                    _Crf_parcela = value;
                    NotifyPropertyChanged("Crf_parcela");
                }
            }
        }

        private decimal _Crf_valorparcela;
        [CamposSqlAttibutes("Crf_valorparcela")]
        public decimal Crf_valorparcela
        {
            get { return _Crf_valorparcela; }
            set
            {
                if (_Crf_valorparcela != value)
                {
                    _Crf_valorparcela = value;
                    NotifyPropertyChanged("Crf_valorparcela");
                }
            }
        }

        private decimal _Crf_valordocumento;
        [CamposSqlAttibutes("Crf_valordocumento")]
        public decimal Crf_valordocumento
        {
            get { return _Crf_valordocumento; }
            set
            {
                if (_Crf_valordocumento != value)
                {
                    _Crf_valordocumento = value;
                    NotifyPropertyChanged("Crf_valordocumento");
                }
            }
        }

        private decimal _Crf_valorrecebido;
        [CamposSqlAttibutes("Crf_valorrecebido")]
        public decimal Crf_valorrecebido
        {
            get { return _Crf_valorrecebido; }
            set
            {
                if (_Crf_valorrecebido != value)
                {
                    _Crf_valorrecebido = value;
                    NotifyPropertyChanged("Crf_valorrecebido");
                }
            }
        }

        private DateTime? _Crf_datalancamento;
        [CamposSqlAttibutes("Crf_datalancamento")]
        public DateTime? Crf_datalancamento
        {
            get { return _Crf_datalancamento; }
            set
            {
                if (_Crf_datalancamento != value)
                {
                    _Crf_datalancamento = value;
                    NotifyPropertyChanged("Crf_datalancamento");
                }
            }
        }

        private DateTime? _Crf_datavencimento;
        [CamposSqlAttibutes("Crf_datavencimento")]
        public DateTime? Crf_datavencimento
        {
            get { return _Crf_datavencimento; }
            set
            {
                if (_Crf_datavencimento != value)
                {
                    _Crf_datavencimento = value;
                    NotifyPropertyChanged("Crf_datavencimento");
                }
            }
        }

        private DateTime? _Crf_databaixa;
        [CamposSqlAttibutes("Crf_databaixa")]
        public DateTime? Crf_databaixa
        {
            get { return _Crf_databaixa; }
            set
            {
                if (_Crf_databaixa != value)
                {
                    _Crf_databaixa = value;
                    NotifyPropertyChanged("Crf_databaixa");
                }
            }
        }

        private string _Crf_observacao;
        [CamposSqlAttibutes("Crf_observacao")]
        public string Crf_observacao
        {
            get { return _Crf_observacao; }
            set
            {
                if (_Crf_observacao != value)
                {
                    _Crf_observacao = value;
                    NotifyPropertyChanged("Crf_observacao");
                }
            }
        }

        private bool _Crf_empagamento;
        [CamposSqlAttibutes("Crf_empagamento")]
        public bool Crf_empagamento
        {
            get { return _Crf_empagamento; }
            set
            {
                if (_Crf_empagamento != value)
                {
                    _Crf_empagamento = value;
                    NotifyPropertyChanged("Crf_empagamento");
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
                    case "Crf_ndocumento":
                        return string.IsNullOrEmpty(Crf_ndocumento) ? "O campo não pode ser vazio" : null;
                    case "Vencimento":
                        return string.IsNullOrEmpty(Vencimento) ? "O campo não pode ser vazio" : null;
                    case "Crf_parcela":
                        return string.IsNullOrEmpty(Crf_parcela) ? "O campo não pode ser vazio" : null;
                    case "NomeCliente":
                        return string.IsNullOrEmpty(NomeCliente) ? "O campo não pode ser vazio" : null;
                    case "DescricaoFpagamento":
                        return string.IsNullOrEmpty(DescricaoFpagamento) ? "O campo não pode ser vazio" : null;
                    case "Pla_numeroconta":
                        return string.IsNullOrEmpty(Pla_numeroconta) ? "O campo não pode ser vazio" : null;
                    case "ValorParcela":
                        return string.IsNullOrEmpty(ValorParcela) ? "O campo não pode ser vazio" : null;
                }
                return null;
            }
        }
    }
}
