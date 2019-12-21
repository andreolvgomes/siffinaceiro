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

namespace SIF.Aplicacao.LayoutConfiguracaoW
{
    /// <summary>
    /// Interaction logic for AlteracaoSenha.xaml
    /// </summary>
    public partial class AlteracaoSenha
    {
        public Usuarios Usuario
        {
            get { return (Usuarios)GetValue(UsuarioProperty); }
            set { SetValue(UsuarioProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Usuario.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UsuarioProperty =
            DependencyProperty.Register("Usuario", typeof(Usuarios), typeof(AlteracaoSenha));


        private ConnectionDb conexao;

        public AlteracaoSenha(Window owner, ConnectionDb conexao, Usuarios usuario)
        {
            InitializeComponent();

            this.conexao = conexao;
            this.DataContext = this;
            this.Usuario = usuario;

            using (EffectWindow e  = new EffectWindow())
            {
                e.SetEffectBackgound(owner, this);
            }
            SelecionaPwd(pwdSenhaAtual);
        }

        private void SalvarSenha(object sender, RoutedEventArgs e)
        {
            if (Validacao())
            {
                using (ProviderRecord<Usuarios> provider = new ProviderRecord<Usuarios>())
                {
                    Usuario.Usu_senha = pwdNovaSenha.Password;
                    provider.Update(Usuario);
                    SistemaGlobal.Sis.Msg.MostraMensagem("Senha alterada com sucesso!", "Atenção", MessageBoxButton.OK, this);
                    this.Close();
                }
            }
        }

        private bool Validacao()
        {
            if(string.IsNullOrEmpty(pwdSenhaAtual.Password))
            {
                SistemaGlobal.Sis.Msg.MostraMensagem("Informe a senha atual!", "Atenção", MessageBoxButton.OK, this);
                return SelecionaPwd(pwdSenhaAtual);
            }
            if (string.IsNullOrEmpty(pwdNovaSenha.Password))
            {
                SistemaGlobal.Sis.Msg.MostraMensagem("Informe a nova senha!", "Atenção", MessageBoxButton.OK, this);
                return SelecionaPwd(pwdNovaSenha);
            }
            if (string.IsNullOrEmpty(pwdNovaSenhaConf.Password))
            {
                SistemaGlobal.Sis.Msg.MostraMensagem("Informe a confirmação da nova senha!", "Atenção", MessageBoxButton.OK, this);
                return SelecionaPwd(pwdNovaSenhaConf);
            }
            if (Usuario.Usu_senha != pwdSenhaAtual.Password)
            {
                SistemaGlobal.Sis.Msg.MostraMensagem("Atual senha inválida!", "Atenção", MessageBoxButton.OK, this);
                return SelecionaPwd(pwdSenhaAtual);
            }
            if (pwdNovaSenha.Password != pwdNovaSenhaConf.Password)
            {
                SistemaGlobal.Sis.Msg.MostraMensagem("Senha diferente da senha de confirmação!", "Atenção", MessageBoxButton.OK, this);
                return SelecionaPwd(pwdNovaSenha);
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
