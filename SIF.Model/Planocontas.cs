using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIF
{
    public class Planocontas : IModelProperty
    {
        public override string NomeTabela
        {
            get { return "Planocontas"; }
        }

        private string _Pla_numeroconta;
        [CamposSqlAttibutes("Pla_numeroconta", IsPrimaryKey = true)]
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

        private string _Pla_descricao;
        [CamposSqlAttibutes("Pla_descricao")]
        public string Pla_descricao
        {
            get { return _Pla_descricao; }
            set
            {
                if (_Pla_descricao != value)
                {
                    _Pla_descricao = value;
                    NotifyPropertyChanged("Pla_descricao");
                }
            }
        }

        [CamposSqlAttibutes("Sis_sequencialdb", IsIdentity = true)]
        public int Sis_sequencialdb { get; set; }

        public static Planocontas GetNewFpagamentos()
        {
            Planocontas p = new Planocontas();
            p.Pla_numeroconta = "";
            p.Pla_descricao = "";
            return p;
        }
    }
}
