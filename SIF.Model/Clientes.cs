using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIF
{
    public partial class Clientes : IModelProperty, IDataErrorInfo
    {
        public override string NomeTabela
        {
            get
            {
                return "Clientes";
            }
        }

        private int _Cli_codio;
        [CamposSqlAttibutes("Cli_codigo", IsIdentity = true, IsPrimaryKey = true)]
        public int Cli_codigo
        {
            get { return _Cli_codio; }
            set
            {
                if (_Cli_codio != value)
                {
                    _Cli_codio = value;
                    NotifyPropertyChanged("Cli_codigo");
                }
            }
        }

        private string _Cli_nome;
        [CamposSqlAttibutes("Cli_nome")]
        public string Cli_nome
        {
            get { return _Cli_nome; }
            set
            {
                if (_Cli_nome != value)
                {
                    _Cli_nome = value;
                    NotifyPropertyChanged("Cli_nome");
                }
            }
        }

        private string _Cli_nomerazao;
        [CamposSqlAttibutes("Cli_nomerazao")]
        public string Cli_nomerazao
        {
            get { return _Cli_nomerazao; }
            set
            {
                if (_Cli_nomerazao != value)
                {
                    _Cli_nomerazao = value;
                    NotifyPropertyChanged("Cli_nomerazao");
                }
            }
        }

        private string _Cli_endereco;
        [CamposSqlAttibutes("Cli_endereco")]
        public string Cli_endereco
        {
            get { return _Cli_endereco; }
            set
            {
                if (_Cli_endereco != value)
                {
                    _Cli_endereco = value;
                    NotifyPropertyChanged("Cli_endereco");
                }
            }
        }

        private int _Cli_numero;
        [CamposSqlAttibutes("Cli_numero")]
        public int Cli_numero
        {
            get { return _Cli_numero; }
            set
            {
                if (_Cli_numero != value)
                {
                    _Cli_numero = value;
                    NotifyPropertyChanged("Cli_numero");
                }
            }
        }

        private string _Cli_bairro;
        [CamposSqlAttibutes("Cli_bairro")]
        public string Cli_bairro
        {
            get { return _Cli_bairro; }
            set
            {
                if (_Cli_bairro != value)
                {
                    _Cli_bairro = value;
                    NotifyPropertyChanged("Cli_bairro");
                }
            }
        }

        private string _Cli_cidade;
        [CamposSqlAttibutes("Cli_cidade")]
        public string Cli_cidade
        {
            get { return _Cli_cidade; }
            set
            {
                if (_Cli_cidade != value)
                {
                    _Cli_cidade = value;
                    NotifyPropertyChanged("Cli_cidade");
                }
            }
        }

        private Byte[] _Cli_foto;
        [CamposSqlAttibutes("Cli_foto")]
        public Byte[] Cli_foto
        {
            get { return _Cli_foto; }
            set
            {
                if (_Cli_foto != value)
                {
                    _Cli_foto = value;
                    NotifyPropertyChanged("Cli_foto");
                }
            }
        }

        private string _Cli_observacao;
        [CamposSqlAttibutes("Cli_observacao")]
        public string Cli_observacao
        {
            get { return _Cli_observacao; }
            set
            {
                if (_Cli_observacao != value)
                {
                    _Cli_observacao = value;
                    NotifyPropertyChanged("Cli_observacao");
                }
            }
        }

        private string _Cli_sexo;
        [CamposSqlAttibutes("Cli_sexo")]
        public string Cli_sexo
        {
            get { return _Cli_sexo; }
            set
            {
                if (_Cli_sexo != value)
                {
                    _Cli_sexo = value;
                    NotifyPropertyChanged("Cli_sexo");
                }
            }
        }

        private string _Cli_estadocivil;
        [CamposSqlAttibutes("Cli_estadocivil")]
        public string Cli_estadocivil
        {
            get { return _Cli_estadocivil; }
            set
            {
                if (_Cli_estadocivil != value)
                {
                    _Cli_estadocivil = value;
                    NotifyPropertyChanged("Cli_estadocivil");
                }
            }
        }

        private string _Cli_naturalidade;
        [CamposSqlAttibutes("Cli_naturalidade")]
        public string Cli_naturalidade
        {
            get { return _Cli_naturalidade; }
            set
            {
                if (_Cli_naturalidade != value)
                {
                    _Cli_naturalidade = value;
                    NotifyPropertyChanged("Cli_naturalidade");
                }
            }
        }

        private DateTime? _Cli_datanascimento;
        [CamposSqlAttibutes("Cli_datanascimento")]
        public DateTime? Cli_datanascimento
        {
            get { return _Cli_datanascimento; }
            set
            {
                if (_Cli_datanascimento != value)
                {
                    _Cli_datanascimento = value;
                    NotifyPropertyChanged("Cli_datanascimento");
                }
            }
        }

        private string _Cli_fone2;
        [CamposSqlAttibutes("Cli_fone2")]
        public string Cli_fone2
        {
            get { return _Cli_fone2; }
            set
            {
                if (_Cli_fone2 != value)
                {
                    _Cli_fone2 = value;
                    NotifyPropertyChanged("Cli_fone2");
                }
            }
        }

        private string _Cli_fone1;
        [CamposSqlAttibutes("Cli_fone1")]
        public string Cli_fone1
        {
            get { return _Cli_fone1; }
            set
            {
                if (_Cli_fone1 != value)
                {
                    _Cli_fone1 = value;
                    NotifyPropertyChanged("Cli_fone1");
                }
            }
        }

        private string _Cli_celular;
        [CamposSqlAttibutes("Cli_celular")]
        public string Cli_celular
        {
            get { return _Cli_celular; }
            set
            {
                if (_Cli_celular != value)
                {
                    _Cli_celular = value;
                    NotifyPropertyChanged("Cli_celular");
                }
            }
        }

        private string _Cli_apelido;
        [CamposSqlAttibutes("Cli_apelido")]
        public string Cli_apelido
        {
            get { return _Cli_apelido; }
            set
            {
                if (_Cli_apelido != value)
                {
                    _Cli_apelido = value;
                    NotifyPropertyChanged("Cli_apelido");
                }
            }
        }

        private string _Cli_extra2;
        [CamposSqlAttibutes("Cli_extra2")]
        public string Cli_extra2
        {
            get { return _Cli_extra2; }
            set
            {
                if (_Cli_extra2 != value)
                {
                    _Cli_extra2 = value;
                    NotifyPropertyChanged("Cli_extra2");
                }
            }
        }

        private string _Cli_extra1;
        [CamposSqlAttibutes("Cli_extra1")]
        public string Cli_extra1
        {
            get { return _Cli_extra1; }
            set
            {
                if (_Cli_extra1 != value)
                {
                    _Cli_extra1 = value;
                    NotifyPropertyChanged("Cli_extra1");
                }
            }
        }

        private string _Cli_cpfcnpj;
        [CamposSqlAttibutes("Cli_cpfcnpj")]
        public string Cli_cpfcnpj
        {
            get { return _Cli_cpfcnpj; }
            set
            {
                if (_Cli_cpfcnpj != value)
                {
                    _Cli_cpfcnpj = value;
                    NotifyPropertyChanged("Cli_cpfcnpj");
                }
            }
        }

        private int _Cli_tipopessoa;
        [CamposSqlAttibutes("Cli_tipopessoa")]
        public int Cli_tipopessoa
        {
            get { return _Cli_tipopessoa; }
            set
            {
                if (_Cli_tipopessoa != value)
                {
                    _Cli_tipopessoa = value;
                    NotifyPropertyChanged("Cli_tipopessoa");
                    NotifyPropertyChanged_Cli_tipopessoa();
                }
            }
        }

        partial void NotifyPropertyChanged_Cli_tipopessoa();

        private string _Cli_complemento;
        [CamposSqlAttibutes("Cli_complemento")]
        public string Cli_complemento
        {
            get { return _Cli_complemento; }
            set
            {
                if (_Cli_complemento != value)
                {
                    _Cli_complemento = value;
                    NotifyPropertyChanged("Cli_complemento");
                }
            }
        }

        private string _Cli_cep;
        [CamposSqlAttibutes("Cli_cep")]
        public string Cli_cep
        {
            get { return _Cli_cep; }
            set
            {
                if (_Cli_cep != value)
                {
                    _Cli_cep = value;
                    NotifyPropertyChanged("Cli_cep");
                }
            }
        }

        private string _Cli_uf;
        [CamposSqlAttibutes("Cli_uf")]
        public string Cli_uf
        {
            get { return _Cli_uf; }
            set
            {
                if (_Cli_uf != value)
                {
                    _Cli_uf = value;
                    NotifyPropertyChanged("Cli_uf");
                }
            }
        }

        public static Clientes GetNewCliente()
        {
            Clientes clinew = new Clientes();

            clinew.Cli_nome = "";
            clinew.Cli_nomerazao = "";
            clinew.Cli_endereco = "";
            clinew.Cli_bairro = "";
            clinew.Cli_cidade = "";
            clinew.Cli_cep = "";
            clinew.Cli_complemento = "";
            clinew.Cli_cpfcnpj = "";
            clinew.Cli_extra1 = "";
            clinew.Cli_extra2 = "";
            clinew.Cli_apelido = "";
            clinew.Cli_celular = "";
            clinew.Cli_fone1 = "";
            clinew.Cli_fone2 = "";
            clinew.Cli_naturalidade = "";
            clinew.Cli_observacao = "";

            //clinew.Cli_uf = clinew.ListaCommom.ListUFs.FirstOrDefault();
            //clinew.Cli_estadocivil = clinew.ListaCommom.ListEstadoCivil.FirstOrDefault();
            //clinew.Cli_sexo = clinew.ListaCommom.ListSexo.FirstOrDefault();

            return clinew;
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
                    case "Cli_nome":
                        return string.IsNullOrEmpty(Cli_nome) ? "O campo não pode ser vazio" : null;
                    case "Cli_endereco":
                        return string.IsNullOrEmpty(Cli_endereco) ? "O campo não pode ser vazio" : null;
                    case "Cli_bairro":
                        return string.IsNullOrEmpty(Cli_bairro) ? "O campo não pode ser vazio" : null;
                    case "Cli_cidade":
                        return string.IsNullOrEmpty(Cli_cidade) ? "O campo não pode ser vazio" : null;
                }
                return null;
            }
        }
    }
}
