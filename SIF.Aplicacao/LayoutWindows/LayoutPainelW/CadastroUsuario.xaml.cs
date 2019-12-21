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
    /// Interaction logic for CadastroUsuario.xaml
    /// </summary>
    public partial class CadastroUsuario
    {
        private ProviderUsuarios provider;

        public CadastroUsuario(Window owner, ConnectionDb conexao)
        {
            InitializeComponent();

            provider = new ProviderUsuarios(this, buttons, conexao, new Action<Usuarios>(OrganizaValoresToInterface));
            provider.Provider.Event_GetNewRecordEventHandler += new ExecutacaoCommandsComDaoSqlModelEventHandler<Usuarios>(NovoUsuario);
            provider.Provider.Event_ValidacaoCamposCorrenteEventHandler += new ExecucaoCommandsComDaoSqlBooleanEventHandler<Usuarios>(ValidaCampos);
            provider.Provider.Event_PesquisaEventHandler += new ModelPesquisaEventHandler(PesquisaF8);
            provider.Provider.Event_ConfirmacaoDeleteEventHandler += new ExecucaoCommandsComDaoSqlBooleanEventHandler<Usuarios>(ConfirmacaoDelete);
            this.DataContext = provider;

            SistemaGlobal.Sis.DefinicoesUsuario.SetDefinicoesWindow(this, SIF.Commom.UssessaoEnum.CADASTRO_USUARIOS, buttons);

            using (EffectWindow e = new EffectWindow())
            {
                e.SetEffectBackgound(owner, this);
            }
        }

        private bool ConfirmacaoDelete(Usuarios model)
        {
            if (SistemaGlobal.Sis.Msg.MostraMensagem("Deseja realmente excluir o registro?", "Atenção", MessageBoxButton.YesNo, this) == MessageBoxResult.Yes)
                return true;
            return false;
        }

        private void PesquisaF8()
        {
            using (UsuariosPes pes = new UsuariosPes(SistemaGlobal.Sis.Connection))
            {
                provider.SetUsuario(pes.Pesquisa(this));
            }
        }

        private void OrganizaValoresToInterface(Usuarios model)
        {
            pwdSenhaConf.Password = pwdSenha.Password = model.Usu_senha;
        }

        private bool ValidaCampos(Usuarios model)
        {
            if (!provider.ValidaUsuario(this)) return SelecionaText(txtUsuario);
            if (provider.Provider.ComandoCorrente == ControleButtons.New)
            {
                if (!provider.ValidaSenha(this, pwdSenha.Password)) return SelecionaTextPwd(pwdSenha);
                if (!provider.ValidaSenhaConf(this, pwdSenhaConf.Password)) return SelecionaTextPwd(pwdSenhaConf);
                if (!provider.ValidaSenhaOk(this, pwdSenha.Password, pwdSenhaConf.Password)) return SelecionaTextPwd(pwdSenha);
            }
            return true;
        }

        private Usuarios NovoUsuario()
        {
            provider.ValidaNomeUsuario = true;
            pwdSenhaConf.Password = pwdSenha.Password = "";
            provider.InformarSenha = true;
            SelecionaText(txtUsuario);
            Usuarios us = new Usuarios();
            us.Usu_perfil = provider.SourcePerfil.FirstOrDefault();
            return us;
        }

        private bool SelecionaTextPwd(PasswordBox pass)
        {
            pass.Focus();
            pass.SelectAll();

            return false;
        }

        private void AlterarSenha_Click(object sender, RoutedEventArgs e)
        {
            AlteracaoSenha a = new AlteracaoSenha(this, SistemaGlobal.Sis.Connection, provider.Provider.Entidade);
            a.ShowDialog();
        }

        private void btnPermissoes_Click_1(object sender, RoutedEventArgs e)
        {
            UsuarioPermissoes p = new UsuarioPermissoes(this, provider.Provider.Entidade, SistemaGlobal.Sis.Connection);
            p.ShowDialog();
        }
    }
}
