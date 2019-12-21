using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace SIF.Aplicacao
{
    public class XmlConfiguracaoInstancia : INotifyPropertyChanged, IDisposable
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
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
                    NotifyPropertyChanged("Autenticacao");
                }
            }
        }

        private string _Senha;
        /// <summary>
        /// Senha de autenticação com SQL
        /// </summary>
        public string Senha
        {
            get { return _Senha; }
            set
            {
                if (_Senha != value)
                {
                    _Senha = value;
                    NotifyPropertyChanged("Senha");
                }
            }
        }

        private string _Usuario;
        /// <summary>
        /// Usuário de autenticação com SQL
        /// </summary>
        public string Usuario
        {
            get { return _Usuario; }
            set
            {
                if (_Usuario != value)
                {
                    _Usuario = value;
                    NotifyPropertyChanged("Usuario");
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
                    NotifyPropertyChanged("Instancia");
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
                    NotifyPropertyChanged("InfoTimeout");
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
                    NotifyPropertyChanged("Timeout");
                }
            }
        }

        private string _connectionString;
        /// <summary>
        /// String de conexão completa com servidor sql
        /// </summary>
        public string ConnectionString
        {
            get
            {
                //return @"Data Source=(LocalDB)\v11.0;AttachDbFilename=c:\users\andre\documents\visual studio 2012\Projects\ConsoleApplication4\ConsoleApplication4\BDSCP.mdf;Integrated Security=True";

                if (string.IsNullOrEmpty(_connectionString))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(string.Format("Server = {0};", Instancia));
                    sb.Append(string.Format("Database = BDSIF;"));
                    if (Autenticacao)
                        sb.Append(string.Format("User Id = {0}; Password = {1};", Usuario, Senha));
                    else
                        sb.Append("Integrated Security = SSPI;");
                    if (InfoTimeout)
                        sb.Append(string.Format("Connection Timeout = {0};", Timeout));
                    _connectionString = sb.ToString();
                }
                return _connectionString;
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
                    NotifyPropertyChanged("ListInstancias");
                }
            }
        }

        /// <summary>
        /// Status se existe o xml de configuração de instância
        /// </summary>
        public bool ExisteXml
        {
            get
            {
                return System.IO.File.Exists(CaminhoXml);
            }
        }

        /// <summary>
        /// Caminho completo do xml de configurações do sql
        /// </summary>
        public string CaminhoXml
        {
            get
            {
                //return System.IO.Path.Combine(Environment.CurrentDirectory, "ConnectionString.xml");
                return System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "SIFXML.xml");
            }
        }

        public XmlConfiguracaoInstancia()
        {
            this.ReadXml();
        }

        /// <summary>
        /// Ler o xml de configuração
        /// </summary>
        private void ReadXml()
        {
            if (ExisteXml)
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(CaminhoXml);
                Instancia = xml.GetElementsByTagName("Instancia")[0].InnerText;

                Autenticacao = xml.GetElementsByTagName("Autenticado")[0].InnerText == "True" ? true : false;
                if (Autenticacao)
                {
                    Usuario = xml.GetElementsByTagName("Usuario")[0].InnerText;
                    Senha = xml.GetElementsByTagName("Senha")[0].InnerText;
                }
                InfoTimeout = xml.GetElementsByTagName("InfTimeout")[0].InnerText == "True" ? true : false;
                if (InfoTimeout)
                {
                    Timeout = xml.GetElementsByTagName("Timeout")[0].InnerText;
                }
            }
        }

        /// <summary>
        /// Salva o xml de configuração
        /// </summary>
        internal void SaveXml()
        {
            if (System.IO.File.Exists(CaminhoXml))
                System.IO.File.Delete(CaminhoXml);

            XDocument xml = new XDocument(new XDeclaration("1.0", "UFT-8", "Sys"),
                new XElement("Configuracao",
                    new XElement("SQL",
                        new XElement("Instancia", Instancia),
                        new XElement("Autenticado", Autenticacao.ToString()),
                        new XElement("Usuario", Usuario),
                        new XElement("Senha", Senha),
                        new XElement("InfTimeout", InfoTimeout),
                        new XElement("Timeout", Timeout))));

            xml.Save(CaminhoXml);
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
        }

        /// <summary>
        /// Testa uma conexão com o sql
        /// </summary>
        /// <returns></returns>
        public bool TestaConnection()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(this.ConnectionString))
                {
                    con.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return SistemaGlobal.Sis.Log.LogError(ex);
            }
        }
    }
}
