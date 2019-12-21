using SIF.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SIF.Aplicacao.ConfiguracaoInstanciaSQL.UserControlPassos
{
    /// <summary>
    /// Interaction logic for PassoConfiguracaoISGBD.xaml
    /// </summary>
    public partial class PassoConfiguracaoISGBD : UserControl
    {
        private ContextInicial context = null;

        public PassoConfiguracaoISGBD(ContextInicial context)
        {
            InitializeComponent();

            this.context = context;

            this.DataContext = context;
        }

        public bool Validacao(Window owner)
        {
            if (!this.context.ValidaInstanciaSql(owner)) return false;
            if (!this.context.ValidaUsuarioSql(owner)) return this.context.SelecinaText(txtUsuario);
            if (!this.context.ValidaSenhaSql(owner, txtSenha.Password)) return this.context.SelecinaText(txtSenha);
            if (!this.context.ValidaTimeOutSql(owner)) return this.context.SelecinaText(txtTimeout);
            bool ifconnection = false;

            StringBuilder connectionString = new StringBuilder(string.Format("server = {0}; database = bdsif; ", this.context.Instancia));
            if (this.context.Autenticacao)
                connectionString.Append(string.Format("user id = {0}; pwd = {1}", this.context.UsuarioSql, this.context.SenhaSql));
            else
                connectionString.Append("integrated security=sspi;");
            SistemaGlobal.Sis.Msg.ExecutaSync(owner, "Verificando conexão com o servidor de banco de dados !",
                () =>
                {
                    using (ConnectionDb connection = new ConnectionDb(connectionString.ToString()))
                    {
                        ifconnection = connection.VerificaConexao();
                        if (!ifconnection)
                        {
                            SistemaGlobal.Sis.Msg.MostraMensagem("Não foi possível estabelecer uma conexão com servidor de banco de dados !", "Atenção", owner);
                        }
                    }
                });
            return ifconnection;
        }
    }
}
