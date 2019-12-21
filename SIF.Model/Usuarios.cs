using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIF
{
    public partial class Usuarios : IModelProperty
    {
        public override string NomeTabela
        {
            get { return "Usuarios"; }
        }

        private int _Usu_codigo;
        [CamposSqlAttibutes("Usu_codigo", IsPrimaryKey = true, IsIdentity = true)]
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

        private string _Usu_nome;
        [CamposSqlAttibutes("Usu_nome")]
        public string Usu_nome
        {
            get { return _Usu_nome; }
            set
            {
                if (_Usu_nome != value)
                {
                    _Usu_nome = value;
                    NotifyPropertyChanged("Usu_nome");
                }
            }
        }

        private string _Usu_senha;
        [CamposSqlAttibutes("Usu_senha")]
        public string Usu_senha
        {
            get { return _Usu_senha; }
            set
            {
                if (_Usu_senha != value)
                {
                    _Usu_senha = value;
                    NotifyPropertyChanged("Usu_senha");
                }
            }
        }

        private string _Usu_observacao;
        [CamposSqlAttibutes("Usu_observacao")]
        public string Usu_observacao
        {
            get { return _Usu_observacao; }
            set
            {
                if (_Usu_observacao != value)
                {
                    _Usu_observacao = value;
                    NotifyPropertyChanged("Usu_observacao");
                }
            }
        }

        private string _Usu_perfil;
        [CamposSqlAttibutes("Usu_perfil")]
        public string Usu_perfil
        {
            get { return _Usu_perfil; }
                        set
            {
                if (_Usu_perfil != value)
                {
                    _Usu_perfil = value;
                    NotifyPropertyChanged("Usu_perfil");
                }
            }
        }   
    }
}
