using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIF
{
    public partial class Uscontrolesecao : IModelProperty
    {
        public override string NomeTabela
        {
            get { return "Uscontrolesecao"; }
        }

        private int _Usu_codigo;
        [CamposSqlAttibutes("Usu_codigo", IsPrimaryKey = true)]
        public int Usu_codigo
        {
            get { return _Usu_codigo; }
            set
            {
                if (_Usu_codigo != value)
                {
                    _Usu_codigo = value;
                    NotifyPropertyChanged("Usu_codigo");
                }
            }
        }

        private string _Uss_descricao;
        [CamposSqlAttibutes("Uss_descricao", IsPrimaryKey = true)]
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

        private bool _Usc_disponivel;
        [CamposSqlAttibutes("Usc_disponivel")]
        public bool Usc_disponivel
        {
            get { return _Usc_disponivel; }
            set
            {
                if (_Usc_disponivel != value)
                {
                    _Usc_disponivel = value;
                    NotifyPropertyChanged("Usc_disponivel");
                }
            }
        }

        private bool _Usc_incluir;
        [CamposSqlAttibutes("Usc_incluir")]
        public bool Usc_incluir
        {
            get { return _Usc_incluir; }
            set
            {
                if (_Usc_incluir != value)
                {
                    _Usc_incluir = value;
                    NotifyPropertyChanged("Usc_incluir");
                }
            }
        }

        private bool _Usc_editar;
        [CamposSqlAttibutes("Usc_editar")]
        public bool Usc_editar
        {
            get { return _Usc_editar; }
            set
            {
                if (_Usc_editar != value)
                {
                    _Usc_editar = value;
                    NotifyPropertyChanged("Usc_editar");
                }
            }
        }
        
        private bool _Usc_excluir;
        [CamposSqlAttibutes("Usc_excluir")]
        public bool Usc_excluir
        {
            get { return _Usc_excluir; }
            set
            {
                if (_Usc_excluir != value)
                {
                    _Usc_excluir = value;
                    NotifyPropertyChanged("Usc_excluir");
                }
            }
        }

        [CamposSqlAttibutes("Sis_sequencialdb", IsIdentity = true)]
        public int Sis_sequencialdb { get; set; }
    }
}
