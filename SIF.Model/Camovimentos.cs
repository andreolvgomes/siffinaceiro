using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIF
{
    public partial class Camovimentos : IModelProperty, IDataErrorInfo
    {
        public override string NomeTabela
        {
            get { return "Camovimentos"; }
        }

        private int _Cam_sequencial;
        [CamposSqlAttibutes("Cam_sequencial", IsPrimaryKey = true, IsIdentity = true)]
        public int Cam_sequencial
        {
            get { return _Cam_sequencial; }
            set
            {
                if (_Cam_sequencial != value)
                {
                    _Cam_sequencial = value;
                    NotifyPropertyChanged("Cam_sequencial");
                }
            }
        }

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

        private decimal _Cam_valorlancado;
        [CamposSqlAttibutes("Cam_valorlancado")]
        public decimal Cam_valorlancado
        {
            get { return _Cam_valorlancado; }
            set
            {
                if (_Cam_valorlancado != value)
                {
                    _Cam_valorlancado = value;
                    NotifyPropertyChanged("Cam_valorlancado");
                }
            }
        }

        private string _Cam_tipomovimento;
        [CamposSqlAttibutes("Cam_tipomovimento")]
        public string Cam_tipomovimento
        {
            get { return _Cam_tipomovimento; }
            set
            {
                if (_Cam_tipomovimento != value)
                {
                    _Cam_tipomovimento = value;
                    NotifyPropertyChanged("Cam_tipomovimento");
                }
            }
        }

        private DateTime _Cam_datalancamento;
        [CamposSqlAttibutes("Cam_datalancamento")]
        public DateTime Cam_datalancamento
        {
            get { return _Cam_datalancamento; }
            set
            {
                if (_Cam_datalancamento != value)
                {
                    _Cam_datalancamento = value;
                    NotifyPropertyChanged("Cam_datalancamento");
                }
            }
        }

        private string _Cam_observacao;
        [CamposSqlAttibutes("Cam_observacao")]
        public string Cam_observacao
        {
            get { return _Cam_observacao; }
            set
            {
                if (_Cam_observacao != value)
                {
                    _Cam_observacao = value;
                    NotifyPropertyChanged("Cam_observacao");
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
                    case "DataLancamento":
                        return string.IsNullOrEmpty(DataLancamento) ? "O campo não pode ser vazio" : null;
                    case "NomeCliente":
                        return string.IsNullOrEmpty(NomeCliente) ? "O campo não pode ser vazio" : null;
                    case "Caixa":
                        return string.IsNullOrEmpty(Caixa) ? "O campo não pode ser vazio" : null;
                    case "FpagamentoDescricao":
                        return string.IsNullOrEmpty(FpagamentoDescricao) ? "O campo não pode ser vazio" : null;
                }
                return null;
            }
        }
    }
}
