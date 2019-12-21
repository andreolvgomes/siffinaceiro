using SIF.WPF.Styles.Windows.Controls;
using SIF.Dao;
using SIF.Aplicacao.UserControls;
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

namespace SIF.Aplicacao.LayoutCadastroW
{
    /// <summary>
    /// Interaction logic for CadastroPlanoContas.xaml
    /// </summary>
    public partial class CadastroPlanoContas : ModernWindow
    {
        private ProviderInterfacesCadastros2<Planocontas> provider;

        public CadastroPlanoContas(Window owner, ConnectionDb conexao)
        {
            InitializeComponent();

            provider = new ProviderInterfacesCadastros2<Planocontas>(this, buttons, conexao);
            provider.LoadRegistros("Planocontas", "Pla_numeroconta");

            provider.Event_ValidacaoCamposCorrenteEventHandler += new ExecucaoCommandsComDaoSqlBooleanEventHandler<Planocontas>(Validacao);
            provider.Event_GetNewRecordEventHandler += new ExecutacaoCommandsComDaoSqlModelEventHandler<Planocontas>(GetNewRecod);
            provider.Event_PesquisaEventHandler += new ModelPesquisaEventHandler(ExecutaPesquisa);
            provider.Event_ConfirmacaoDeleteEventHandler += new ExecucaoCommandsComDaoSqlBooleanEventHandler<Planocontas>(ConfirmacaoDelete);

            this.DataContext = provider;

            SistemaGlobal.Sis.DefinicoesUsuario.SetDefinicoesWindow(this, SIF.Commom.UssessaoEnum.CADASTRO_PLANO_CONTAS, buttons);

            using (EffectWindow e = new EffectWindow())
            {
                e.SetEffectBackgound(owner, this);
            }
            this.PreviewKeyDown += new KeyEventHandler(W_PreviewKeyDown);
        }

        private bool ConfirmacaoDelete(Planocontas model)
        {
            if (SistemaGlobal.Sis.Msg.MostraMensagem("Deseja realmente excluir o registro?", "Atenção", MessageBoxButton.YesNo, this) == MessageBoxResult.Yes)
                return true;
            return false;
        }

        private void ExecutaPesquisa()
        {
            using (PlanoContasPes pes = new PlanoContasPes(SistemaGlobal.Sis.Connection))
            {
                provider.SetEntidade(pes.Pesquisa(this));
            }
        }

        private void W_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.SystemKey == Key.F10)
            {
                ExecutaPesquisa();
                e.Handled = true;
            }
        }

        private bool Validacao(Planocontas model)
        {
            if (string.IsNullOrEmpty(model.Pla_numeroconta))
            {
                SistemaGlobal.Sis.Msg.MostraMensagem("O campo Número do Plano de Contas não pode ser vazio!", "Atenção", MessageBoxButton.OK, this);
                return SelecionaText(txtCodigo);
            }
            if (string.IsNullOrEmpty(model.Pla_descricao))
            {
                SistemaGlobal.Sis.Msg.MostraMensagem("O campo Descrição do Plano de Contas não pode ser vazio!", "Atenção", MessageBoxButton.OK, this);
                return SelecionaText(txtCodigo);
            }
            if (provider.ComandoCorrente == ControleButtons.New)
            {
                int count = this.provider.Connection.GetValue<int>(string.Format("SELECT COUNT(Pla_numeroconta) FROM dbo.Planocontas WHERE Pla_numeroconta = '{0}'", model.Pla_numeroconta));
                if (count > 0)
                {
                    SistemaGlobal.Sis.Msg.MostraMensagem("Já existe um Plano de Contas cadastro com o mesmo número!", "Atenção", MessageBoxButton.OK, this);
                    return SelecionaText(txtCodigo);
                }
                //if ((from x in provider.ListaRecord where x.Pla_numeroconta == model.Pla_numeroconta select x.Pla_numeroconta).FirstOrDefault() != null)
                //if (provider.Tabela.Select(string.Format("Pla_numeroconta = '{0}'", model.Pla_numeroconta)).FirstOrDefault() != null)
                //{
                //    SistemaGlobal.Sis.Msg.MostraMensagem("Já existe um Plano de Contas cadastro com o mesmo número!", "Atenção", MessageBoxButton.OK, this);
                //    return SelecionaText(txtCodigo);
                //}
            }
            return true;
        }

        private Planocontas GetNewRecod()
        {
            SelecionaText(txtCodigo);

            return Planocontas.GetNewFpagamentos();
        }
    }
}
