using SIF.Dao;
using SIF.Aplicacao.Commom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SIF.Helper;
using SIF.Aplicacao.ConfiguracaoInstanciaSQL;
using SIF.Aplicacao.ManagerWindow;

namespace SIF.Aplicacao
{
    public class SistemaGlobal
    {
        private static object _lockRoot = new object();
        private static SistemaGlobal _local;
        /// <summary>
        /// Instância global para todo o sistema
        /// </summary>
        public static SistemaGlobal Sis
        {
            get
            {
                lock (_lockRoot)
                {
                    if (_local == null)
                        _local = new SistemaGlobal();

                    return _local;
                }
            }
        }

        public CONSULTAS_SELECTED ConsultaType { get; set; }

        public string ConnectionString { get; set; }

        /// <summary>
        /// Log de erros
        /// </summary>
        public LogException Log { get; private set; }
        /// <summary>
        /// Xml de configuração do banco sql
        /// </summary>
        //public XmlConfiguracaoInstancia XmlConnectionDb { get; set; }
        /// <summary>
        /// Controle de janelas window abertas
        /// </summary>
        public ControleJanelas ControleJanelas { get; private set; }
        /// <summary>
        /// Definições do usuário logado
        /// </summary>
        public DefinicoesUsuario DefinicoesUsuario { get; set; }
        /// <summary>
        /// Mensagens
        /// </summary>
        public EstruturaMensagem Msg { get; set; }

        private ConnectionDb _connection;
        /// <summary>
        /// Conexão com o banco
        /// </summary>
        public ConnectionDb Connection
        {
            get
            {
                if (_connection == null)
                    _connection = new ConnectionDb(this.ConnectionString);
                //_connection = new ConnectionDb(XmlConnectionDb.ConnectionString);
                return _connection;
            }
        }

        public ContextInicial XmlSistema { get; set; }
        public ManagementWindowState ManagerWindow { get; private set; }

        public SistemaGlobal()
        {
            this.ControleJanelas = new ControleJanelas();
            this.Msg = new EstruturaMensagem();
            this.Log = new LogException();
            this.ManagerWindow = new ManagementWindowState();
        }

        ///// <summary>
        ///// Verifica conexão com o banco de dados
        ///// </summary>
        ///// <param name="owner"></param>
        ///// <returns></returns>
        //internal bool VerificaConexao(Window owner)
        //{
        //    return this.VerificaConexao(owner, false);
        //}

        /// <summary>
        /// Verifica conexão com o banco de dados
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="displayMsg"></param>
        /// <returns></returns>
        internal bool VerificaConexao(Window owner)
        {
            bool ifConnection = true;
            if (SistemaGlobal.Sis.XmlSistema.Armazenamento == TIPO_ARMAZENAMENTO.ARMAZENAMENTO_BANCO_DE_DADOS)
            {
                try
                {
                    this.Msg.ExecutaSync(owner, "Estabelecendo conexão com o banco de dados...",
                        () =>
                        {
                            try
                            {
                                //if (Connection.GetSqlConnection().State != System.Data.ConnectionState.Open)
                                ifConnection = Connection.VerificaConexao();
                            }
                            catch
                            {
                            }
                        });
                }
                catch
                {
                }
                if (!ifConnection)
                {
                    this.Msg.MostraMensagem("Não foi possível estabelecer uma conexão com o banco de dados!", "Atenção", owner);
                }
            }
            return ifConnection;
        }
    }
}
