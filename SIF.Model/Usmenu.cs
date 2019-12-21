using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIF
{
    public class Usmenu : IModelProperty, IDataErrorInfo
    {
        private int _Usm_sequencial;
        [CamposSqlAttibutes("Usm_sequencial", IsIdentity = true, IsPrimaryKey = true)]
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

        private string _Usm_descricao;
        [CamposSqlAttibutes("Usm_descricao")]
        public string Usm_descricao
        {
            get { return _Usm_descricao; }
            set
            {
                if (_Usm_descricao != value)
                {
                    _Usm_descricao = value;
                    NotifyPropertyChanged("Usm_descricao");
                }
            }
        }

        public string Error
        {
            get { return ""; }
        }

        public string this[string columnName]
        {
            get { return ""; }
        }

        public override string NomeTabela
        {
            get { return "Usmenu"; }
        }
    }
}
