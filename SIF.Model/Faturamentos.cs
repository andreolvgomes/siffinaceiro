using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIF
{
    public partial class Faturamentos : IModelProperty
    {
        public override string NomeTabela
        {
            get { return "Faturamentos"; }
        }

        private int _Fat_sequencial;
        [CamposSqlAttibutes("Fat_sequencial", IsIdentity = true, IsPrimaryKey = true)]
        public int Fat_sequencial
        {
            get { return _Fat_sequencial; }
            set
            {
                if (_Fat_sequencial != value)
                {
                    _Fat_sequencial = value;
                    NotifyPropertyChanged("Fat_sequencial");

                    this.OnFat_sequencialChanged();
                }
            }
        }

        partial void OnFat_sequencialChanged();

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

        private DateTime _Fat_datafatura;
        [CamposSqlAttibutes("Fat_datafatura")]
        public DateTime Fat_datafatura
        {
            get { return _Fat_datafatura; }
            set
            {
                if (_Fat_datafatura != value)
                {
                    _Fat_datafatura = value;
                    NotifyPropertyChanged("Fat_datafatura");
                }
            }
        }

        private decimal _Fat_totfatura;
        [CamposSqlAttibutes("Fat_totfatura")]
        public decimal Fat_totfatura
        {
            get { return _Fat_totfatura; }
            set
            {
                if (_Fat_totfatura != value)
                {
                    _Fat_totfatura = value;
                    NotifyPropertyChanged("Fat_totfatura");
                }
            }
        }

        private int _Fat_quantidadecontas;
        [CamposSqlAttibutes("Fat_quantidadecontas")]
        public int Fat_quantidadecontas
        {
            get { return _Fat_quantidadecontas; }
            set
            {
                if (_Fat_quantidadecontas != value)
                {
                    _Fat_quantidadecontas = value;
                    NotifyPropertyChanged("Fat_quantidadecontas");
                }
            }
        }

        private int _Fat_quantidadelancamentos;
        [CamposSqlAttibutes("Fat_quantidadelancamentos")]
        public int Fat_quantidadelancamentos
        {
            get { return _Fat_quantidadelancamentos; }
            set
            {
                if (_Fat_quantidadelancamentos != value)
                {
                    _Fat_quantidadelancamentos = value;
                    NotifyPropertyChanged("Fat_quantidadelancamentos");
                }
            }
        }

        private string _Fat_observacao;
        [CamposSqlAttibutes("Fat_observacao")]
        public string Fat_observacao
        {
            get { return _Fat_observacao; }
            set
            {
                if (_Fat_observacao != value)
                {
                    _Fat_observacao = value;
                    NotifyPropertyChanged("Fat_observacao");
                }
            }
        }
    }
}
