using SIF.WPF.Styles.Windows.Controls;
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
using System.Windows.Shapes;

namespace SIF.Aplicacao
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login
    {
        private bool sucesso;
        private int count;
        //private DaoGenerico<Usuarios> provider;

        public Login(ConnectionDb conexao)
        {
            InitializeComponent();

            //provider = new DaoGenerico<Usuarios>(conexao);

            this.DataContext = this;

            this.txtNome.Focus();

            this.PreviewKeyDown += new KeyEventHandler(W_PreviewKeyDown);
            this.Loaded += new RoutedEventHandler(W_Loaded);
        }

        private void W_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void W_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                //try
                //{
                //    //Window w = new Window();
                //    //w.Style = FindResource("BlankWindow") as Style;
                //    //w.ShowDialog();

                //    var wnd = new ModernWindow
                //    {
                //        Style = (Style)App.Current.Resources["BlankWindow"],
                //        Width = 480,
                //        Height = 480
                //    };

                //    wnd.Show();
                //}
                //catch (Exception ex)
                //{
                //    throw;
                //}
                //HelpJanela h = new HelpJanela(this);
                //h.ShowDialog();
                //SIF.Pesquisa.PesquisaWindow s = new SIF.Pesquisa.PesquisaWindow(this, "SELECT Cli_codigo as codigo, Cli_nome AS Nome FROM Clientes", Sistema.Sis.XmlConnectionDb.ConnectionString);
                //s.ShowDialog();
            }
            else if (e.Key == Key.Escape)
            {
                this.Close();
            }
            else if (e.Key == Key.Enter)
            {
                UIElement element = e.OriginalSource as UIElement;
                element.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }

        internal static bool PegaLogin(ConnectionDb conexao)
        {
            Login l = new Login(conexao);
            l.ShowDialog();
            return l.sucesso;
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Loga();
            //Notification.Balloon b = new Notification.Balloon(this, "teste");
            //b.Show();
        }

        private void Loga()
        {
            if (Validacao())
            {
                Usuarios usuario = SistemaGlobal.Sis.Connection.GetRegistros<Usuarios>(string.Format("Usu_nome = '{0}' AND Usu_senha = '{1}'", txtNome.Text, pwdSenha.Password)).FirstOrDefault();
                if (usuario == null)
                {
                    SistemaGlobal.Sis.Msg.MostraMensagem("Usuário/Senha inválido!", "Atenção", MessageBoxButton.OK, this);
                    SelecionaText(txtNome);
                }
                else
                {
                    DefinicoesUsuario def = new DefinicoesUsuario();
                    def.SetUsuario(usuario, SistemaGlobal.Sis.Connection);
                    SistemaGlobal.Sis.DefinicoesUsuario = def;
                    sucesso = true;
                    this.Close();
                }
                count++;
                if (count == 3)
                {
                    this.Close();
                }
            }
        }

        private bool Validacao()
        {
            if (string.IsNullOrEmpty(txtNome.Text))
            {
                SistemaGlobal.Sis.Msg.MostraMensagem("Informe o usuário!", "Atenção", MessageBoxButton.OK, this);
                return SelecionaText(txtNome);
            }
            if (string.IsNullOrEmpty(pwdSenha.Password))
            {
                SistemaGlobal.Sis.Msg.MostraMensagem("Informe a senha!", "Atenção", MessageBoxButton.OK, this);
                return SelecionaPwd(pwdSenha);
            }
            if (!SistemaGlobal.Sis.VerificaConexao(this))
            {
                return false;
            }
            return true;
        }

        private bool SelecionaPwd(PasswordBox pwd)
        {
            pwd.Focus();
            pwd.SelectAll();
            return false;
        }
    }
}
