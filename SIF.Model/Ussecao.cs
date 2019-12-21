using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIF
{
    public partial class Ussecao : IModelProperty
    {
        public override string NomeTabela
        {
            get { return "Ussecao"; }
        }

        private string _Uss_descricao;
        [CamposSqlAttibutes("Uss_descricao")]
        public string Uss_descricao
        {
            get { return _Uss_descricao; }
            set
            {
                if (_Uss_descricao != value)
                {
                    _Uss_descricao = value;
                    NotifyPropertyChanged("Uss_descricao");
                }
            }
        }

        private int _Usm_sequencial;
        [CamposSqlAttibutes("Usm_sequencial")]
        public int Usm_sequencial
        {
            get { return _Usm_sequencial; }
            set
            {
                if (_Usm_sequencial != value)
                {
                    _Usm_sequencial = value;
                    NotifyPropertyChanged("Usm_sequencial");
                }
            }
        }

        [CamposSqlAttibutes("Sis_sequencialdb", IsIdentity = true)]
        public int Sis_sequencialdb { get; set; }
    }
}
