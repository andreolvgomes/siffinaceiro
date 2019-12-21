using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIF
{
    public class Fpagamentos : IModelProperty
    {
        public override string NomeTabela
        {
            get { return "Fpagamentos"; }
        }

        private string _Fpa_codigo;
        [CamposSqlAttibutes("Fpa_codigo", IsPrimaryKey = true)]
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

        private string _Fpa_descricao;
        [CamposSqlAttibutes("Fpa_descricao")]
        public string Fpa_descricao
        {
            get { return _Fpa_descricao; }
            set
            {
                if (_Fpa_descricao != value)
                {
                    _Fpa_descricao = value;
                    NotifyPropertyChanged("Fpa_descricao");
                }
            }
        }

        private string _Fpa_observacao;
        [CamposSqlAttibutes("Fpa_observacao")]
        public string Fpa_observacao
        {
            get { return _Fpa_observacao; }
            set
            {
                if (_Fpa_observacao != value)
                {
                    _Fpa_observacao = value;
                    NotifyPropertyChanged("Fpa_observacao");
                }
            }
        }

        [CamposSqlAttibutes("Sis_sequencialdb", IsIdentity = true)]
        public int Sis_sequencialdb { get; set; }

        public static Fpagamentos GetNewFpagamentos()
        {
            Fpagamentos f = new Fpagamentos();
            f.Fpa_codigo = "";
            f.Fpa_descricao = "";
            f.Fpa_observacao = "";
            return f;
        }
    }
}
