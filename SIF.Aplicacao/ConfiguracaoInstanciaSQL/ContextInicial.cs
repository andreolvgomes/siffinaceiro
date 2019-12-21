using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Sql;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Linq;

namespace SIF.Aplicacao.ConfiguracaoInstanciaSQL
{
    public class ContextInicial : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public String Detalhe
        {
            get
            {
                switch (_armazenamento)
                {
                    case TIPO_ARMAZENAMENTO.ARMAZENAMENTO_EM_ARQUIVO:
                        return "Trabalha com armazenamento dos DADOS em arquivo. Interessante para quem quer utilizar o Sistema em um só local sem a distribuição dos dados, pois desta forma os dados são armazenado na máquina local. Assim não há necessidade de instalar o SGBD SQL Server.";
                    case TIPO_ARMAZENAMENTO.ARMAZENAMENTO_BANCO_DE_DADOS:
                        return "Trabalha com armazenamento dos DADOS em um banco de dados vai SGBD. Desta forma é obrigatorio ter o SQL Server instalado na máquina ou em algum computador na rede. Interessante para quem utilizar a Aplicação em vários terminais tendo um só local para armazenamento e distribuição dos dados. Para utilizar desta forma é aconselhável ter noções básicas de SGBD SQL Server.";
                }
                return "";
            }
        }

        private TIPO_ARMAZENAMENTO _armazenamento = TIPO_ARMAZENAMENTO.ARMAZENAMENTO_EM_ARQUIVO;
        /// <summary>
        /// Tipo de armazenamento
        /// </summary>
        public TIPO_ARMAZENAMENTO Armazenamento
        {
            get { return _armazenamento; }
            set
            {
                if (_armazenamento != value)
                {
                    _armazenamento = value;
                    OnPropertyChanged("Armazenamento");
                    OnPropertyChanged("Detalhe");
                }
            }
        }

        private ObservableCollection<String> _ListInstancias;
        /// <summary>
        /// Lista de instâncias encontrada na rede
        /// </summary>
        public ObservableCollection<String> ListInstancias
        {
            get { return _ListInstancias; }
            set
            {
                if (_ListInstancias != value)
                {
                    _ListInstancias = value;
                    OnPropertyChanged("ListInstancias");
                }
            }
        }

        private bool _Autenticacao;
        /// <summary>
        /// Se irá autenticar a conexão com sql
        /// </summary>
        public bool Autenticacao
        {
            get { return _Autenticacao; }
            set
            {
                if (_Autenticacao != value)
                {
                    _Autenticacao = value;
                    OnPropertyChanged("Autenticacao");
                }
            }
        }

        private string _SenhaSql;
        /// <summary>
        /// Senha de autenticação com SQL
        /// </summary>
        public string SenhaSql
        {
            get { return _SenhaSql; }
            set
            {
                if (_SenhaSql != value)
                {
                    _SenhaSql = value;
                    OnPropertyChanged("SenhaSql");
                }
            }
        }

        private string _UsuarioSql;
        /// <summary>
        /// Usuário de autenticação com SQL
        /// </summary>
        public string UsuarioSql
        {
            get { return _UsuarioSql; }
            set
            {
                if (_UsuarioSql != value)
                {
                    _UsuarioSql = value;
                    OnPropertyChanged("UsuarioSql");
                }
            }
        }

        private string _Instancia;
        /// <summary>
        /// Instância caminho do servidor SQL
        /// </summary>
        public string Instancia
        {
            get { return _Instancia; }
            set
            {
                if (_Instancia != value)
                {
                    _Instancia = value;
                    OnPropertyChanged("Instancia");
                }
            }
        }

        private bool _InfoTimeout;
        /// <summary>
        /// Se irá trabalhar com timeout nas conexões
        /// </summary>
        public bool InfoTimeout
        {
            get { return _InfoTimeout; }
            set
            {
                if (_InfoTimeout != value)
                {
                    _InfoTimeout = value;
                    OnPropertyChanged("InfoTimeout");
                }
            }
        }

        private string _Timeout;
        /// <summary>
        /// Tempo do timeout nas conexões
        /// </summary>
        public string Timeout
        {
            get { return _Timeout; }
            set
            {
                if (_Timeout != value)
                {
                    _Timeout = value;
                    OnPropertyChanged("Timeout");
                }
            }
        }

        public string SenhaSis { get; set; }

        private string _UsuarioSis;
        /// <summary>
        /// Usuário padrão
        /// </summary>
        public string UsuarioSis
        {
            get { return _UsuarioSis; }
            set
            {
                if (_UsuarioSis != value)
                {
                    _UsuarioSis = value;
                    OnPropertyChanged("UsuarioSis");
                }
            }
        }

        private string _CaminhoMdf;
        /// <summary>
        /// Caminho do Arquivo MDF
        /// </summary>
        public string CaminhoMdf
        {
            get { return _CaminhoMdf; }
            set
            {
                if (_CaminhoMdf != value)
                {
                    _CaminhoMdf = value;
                    OnPropertyChanged("CaminhoMdf");
                }
            }
        }

        private string _CaminhoArquivoBanco;

        public string CaminhoArquivoBanco
        {
            get { return _CaminhoArquivoBanco; }
            set
            {
                _CaminhoArquivoBanco = value;
            }
        }

        private string _CaminhoArquivoExistente;
        /// <summary>
        /// Caminho do arquivo existente
        /// </summary>
        public string CaminhoArquivoExistente
        {
            get { return _CaminhoArquivoExistente; }
            set
            {
                if (_CaminhoArquivoExistente != value)
                {
                    _CaminhoArquivoExistente = value;
                    OnPropertyChanged("CaminhoArquivoExistente");
                }
            }
        }

        private CONFIGURACAO_ARQUIVO _ConfigArquivo;
        /// <summary>
        /// Configuração do arquivo MDF
        /// </summary>
        public CONFIGURACAO_ARQUIVO ConfigArquivo
        {
            get { return _ConfigArquivo; }
            set
            {
                if (_ConfigArquivo != value)
                {
                    _ConfigArquivo = value;
                    OnPropertyChanged("ConfigArquivo");
                }
            }
        }

        private bool _ArquivoEmOutroCaminho;
        /// <summary>
        /// 
        /// </summary>
        public bool ArquivoEmOutroCaminho
        {
            get { return _ArquivoEmOutroCaminho; }
            set
            {
                if (_ArquivoEmOutroCaminho != value)
                {
                    _ArquivoEmOutroCaminho = value;
                    OnPropertyChanged("ArquivoEmOutroCaminho");
                }
            }
        }

        private bool _UsuarioSisExists;
        /// <summary>
        /// True caso já tenha usuário cadastrado
        /// </summary>
        public bool UsuarioSisExists
        {
            get { return _UsuarioSisExists; }
            set
            {
                if (_UsuarioSisExists != value)
                {
                    _UsuarioSisExists = value;
                    OnPropertyChanged("UsuarioSisExists");
                }
            }
        }

        public bool ExistsXml
        {
            get
            {
                return System.IO.File.Exists(caminhoSIFXml);
            }
        }

        private string NOME_ARQUIVO_MDF = "BDSIF.mdf";
        private string NOME_ARQUIVO_LDF = "BDSIF_1.ldf";

        private string caminhoSIFXml = string.Empty;

        public ContextInicial()
        {
            this.caminhoSIFXml = System.IO.Path.Combine(Environment.CurrentDirectory, "SIFXml.xml");
            this.CaminhoMdf = System.IO.Path.Combine(Environment.CurrentDirectory, "BDSIF.mdf");
            SistemaGlobal.Sis.XmlSistema = this;
        }

        /// <summary>
        /// Organiza lista de instância sql encontrada na rede
        /// </summary>
        public void LoadedInstancias()
        {
            this.ListInstancias = new ObservableCollection<String>();
            DataTable dataTable = SqlDataSourceEnumerator.Instance.GetDataSources();
            string instancia = "";
            foreach (DataRow row in dataTable.Rows)
            {
                instancia = row[0].ToString();
                if (!string.IsNullOrEmpty(row[1].ToString().Trim()))
                    instancia += "\\" + row[1];
                ListInstancias.Add(instancia);
            }
            if (this.ListInstancias.Count > 0)
                this.Instancia = this.ListInstancias.FirstOrDefault();
        }

        internal bool ValidaInstanciaSql(System.Windows.Window owner)
        {
            try
            {
                if (string.IsNullOrEmpty(this.Instancia))
                    return SistemaGlobal.Sis.Msg.MostraMensagem("Instância não pode ser vazia", "Atenção", owner) != System.Windows.MessageBoxResult.OK;
            }
            catch (Exception ex)
            {
                return SistemaGlobal.Sis.Log.LogError(ex, true, owner);
            }
            return true;
        }

        internal bool ValidaTimeOutSql(System.Windows.Window owner)
        {
            try
            {
                if (this.InfoTimeout)
                {
                    if (string.IsNullOrEmpty(this.Timeout))
                        return SistemaGlobal.Sis.Msg.MostraMensagem("Informe o Timerout", "Atenção", owner) != System.Windows.MessageBoxResult.OK;
                }
            }
            catch (Exception ex)
            {
                return SistemaGlobal.Sis.Log.LogError(ex, true, owner);
            }
            return true;
        }

        internal bool ValidaSenhaSql(System.Windows.Window owner, string senha)
        {
            try
            {
                if (this.Autenticacao)
                {
                    if (string.IsNullOrEmpty(senha))
                        return SistemaGlobal.Sis.Msg.MostraMensagem("Informe a Senha de Autenticação SQL", "Atenção", owner) != System.Windows.MessageBoxResult.OK;
                }
                this.SenhaSql = senha;
            }
            catch (Exception ex)
            {
                return SistemaGlobal.Sis.Log.LogError(ex, true, owner);
            }
            return true;
        }

        internal bool SelecinaText(PasswordBox text)
        {
            text.Focus();
            text.SelectAll();
            return false;
        }

        internal bool SelecinaText(TextBox text)
        {
            text.Focus();
            text.SelectAll();
            return false;
        }

        internal bool ValidaUsuarioSql(System.Windows.Window owner)
        {
            try
            {
                if (this.Autenticacao)
                {
                    if (string.IsNullOrEmpty(this.UsuarioSql))
                        return SistemaGlobal.Sis.Msg.MostraMensagem("Informe o Usuário de Autenticação SQL", "Atenção", owner) != System.Windows.MessageBoxResult.OK;
                }
            }
            catch (Exception ex)
            {
                return SistemaGlobal.Sis.Log.LogError(ex, true, owner);
            }
            return true;
        }

        internal bool ValidaUsuarioSis(System.Windows.Window owner)
        {
            try
            {
                if (string.IsNullOrEmpty(this.UsuarioSis))
                    return SistemaGlobal.Sis.Msg.MostraMensagem("Informe o Nome de Usuário", "Atenção", owner) != System.Windows.MessageBoxResult.OK;
            }
            catch (Exception ex)
            {
                return SistemaGlobal.Sis.Log.LogError(ex, true, owner);
            }
            return true;
        }

        internal bool ValidaSenhaSis(System.Windows.Window owner, string senha)
        {
            try
            {
                if (string.IsNullOrEmpty(senha))
                    return SistemaGlobal.Sis.Msg.MostraMensagem("Informe a Senha", "Atenção", owner) != System.Windows.MessageBoxResult.OK;
            }
            catch (Exception ex)
            {
                return SistemaGlobal.Sis.Log.LogError(ex, true, owner);
            }
            return true;
        }

        internal bool ValidacaoSenhaConfSis(System.Windows.Window owner, string senha)
        {
            try
            {
                if (string.IsNullOrEmpty(senha))
                    return SistemaGlobal.Sis.Msg.MostraMensagem("Informe a confirmação da Senha", "Atenção", owner) != System.Windows.MessageBoxResult.OK;
            }
            catch (Exception ex)
            {
                return SistemaGlobal.Sis.Log.LogError(ex, true, owner);
            }
            return true;
        }

        internal bool ValidacaoSenhaSisEquals(System.Windows.Window owner, string senha, string senhaConf)
        {
            try
            {
                if (!senha.Equals(senhaConf))
                    return SistemaGlobal.Sis.Msg.MostraMensagem("Senha inválida, diferentes!", "Atenção", owner) != System.Windows.MessageBoxResult.OK;
            }
            catch (Exception ex)
            {
                return SistemaGlobal.Sis.Log.LogError(ex, true, owner);
            }
            return true;
        }

        internal bool Save(Window owner)
        {
            try
            {
                if (this.OrganizaDados(owner))
                {
                    if (!System.IO.File.Exists(this.caminhoSIFXml))
                    {
                        XDocument xml = new XDocument(new XDeclaration("1.0", "UTF-8", "Yes"), new XElement("SISTEMA"));
                        xml.Save(this.caminhoSIFXml);
                    }
                    string tagMain = "DEFINICOES";
                    XElement doc = XElement.Load(this.caminhoSIFXml);
                    if (doc.Element(tagMain) != null) doc.Element(tagMain).Remove();
                    XElement def = new XElement(tagMain);

                    def.Add(this.GetXelementValue<ContextInicial>(c => c.Armazenamento));
                    if (Armazenamento == TIPO_ARMAZENAMENTO.ARMAZENAMENTO_EM_ARQUIVO)
                    {
                        def.Add(new XElement("ArquivoDB", this.GetXelementValue<ContextInicial>(c => c.CaminhoArquivoBanco)));
                    }
                    else
                    {
                        def.Add(new XElement("SQLServer"
                            , this.GetXelementValue<ContextInicial>(c => c.Instancia)
                            , this.GetXelementValue<ContextInicial>(c => c.Autenticacao)
                            , this.GetXelementValue<ContextInicial>(c => c.UsuarioSql)
                            , this.GetXelementValue<ContextInicial>(c => c.SenhaSql)
                            , this.GetXelementValue<ContextInicial>(c => c.InfoTimeout)
                            , this.GetXelementValue<ContextInicial>(c => c.Timeout)));
                    }

                    doc.Add(def);
                    doc.Save(this.caminhoSIFXml);

                    return true;
                }
            }
            catch (Exception ex)
            {
                return SistemaGlobal.Sis.Log.LogError(ex, true, owner);
            }
            return false;
        }

        private bool OrganizaDados(Window owner)
        {
            try
            {
                this.CaminhoArquivoBanco = string.Empty;
                /// set o verdadeiro caminho do arquivo
                /// 
                if (Armazenamento == TIPO_ARMAZENAMENTO.ARMAZENAMENTO_EM_ARQUIVO)
                {
                    if (ConfigArquivo == CONFIGURACAO_ARQUIVO.ARQUIVO_EXISTENTE)
                        this.CaminhoArquivoBanco = this.CaminhoArquivoExistente;
                    else
                        this.CaminhoArquivoBanco = this.CaminhoMdf;
                    /// move o arquivo se assim foi configurado
                    /// 
                    if (this.ArquivoEmOutroCaminho)
                    {
                        if (Environment.CurrentDirectory != System.IO.Path.GetDirectoryName(this.CaminhoMdf))
                        {
                            string source1 = System.IO.Path.Combine(Environment.CurrentDirectory, NOME_ARQUIVO_MDF);
                            string source2 = System.IO.Path.Combine(Environment.CurrentDirectory, NOME_ARQUIVO_LDF);

                            string dest1 = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.CaminhoMdf), NOME_ARQUIVO_MDF);
                            string dest2 = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.CaminhoMdf), NOME_ARQUIVO_LDF);

                            System.IO.File.Move(source1, dest1);
                            System.IO.File.Move(source2, dest2);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return SistemaGlobal.Sis.Log.LogError(ex, true, owner);
            }
            return true;
        }

        internal void LoadSIFXml()
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(this.caminhoSIFXml);
            StringBuilder sb = new StringBuilder();

            Armazenamento = (TIPO_ARMAZENAMENTO)Enum.Parse(typeof(TIPO_ARMAZENAMENTO), xml.GetValueTag<ContextInicial>(this, c => c.Armazenamento));
            switch (Armazenamento)
            {
                case TIPO_ARMAZENAMENTO.ARMAZENAMENTO_EM_ARQUIVO:
                    this.CaminhoArquivoBanco = xml.GetValueTag<ContextInicial>(this, c => c.CaminhoArquivoBanco);
                    sb.Append(string.Format(@"Data Source=(LocalDB)\v11.0;AttachDbFilename={0};Integrated Security=True", this.CaminhoArquivoBanco));
                    break;
                case TIPO_ARMAZENAMENTO.ARMAZENAMENTO_BANCO_DE_DADOS:
                    this.Instancia = xml.GetValueTag<ContextInicial>(this, c => c.Instancia);
                    this.Autenticacao = Convert.ToBoolean(xml.GetValueTag<ContextInicial>(this, c => c.Autenticacao));
                    this.UsuarioSql = xml.GetValueTag<ContextInicial>(this, c => c.UsuarioSql);
                    this.SenhaSql = xml.GetValueTag<ContextInicial>(this, c => c.SenhaSql);
                    this.InfoTimeout = Convert.ToBoolean(xml.GetValueTag<ContextInicial>(this, c => c.InfoTimeout));
                    this.Timeout = xml.GetValueTag<ContextInicial>(this, c => c.Timeout);

                    sb.Append(string.Format("Server = {0};", Instancia));
                    sb.Append(string.Format("Database = BDSIF;"));
                    if (Autenticacao)
                        sb.Append(string.Format("User Id = {0}; Password = {1};", UsuarioSql, SenhaSql));
                    else
                        sb.Append("Integrated Security = SSPI;");
                    if (InfoTimeout)
                        sb.Append(string.Format("Connection Timeout = {0};", Timeout));
                    break;
            }
            SistemaGlobal.Sis.ConnectionString = sb.ToString();
        }
    }
}