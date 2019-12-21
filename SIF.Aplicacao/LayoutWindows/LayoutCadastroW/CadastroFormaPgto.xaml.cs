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

namespace SIF.Aplicacao.LayoutCadastroW
{
    /// <summary>
    /// Interaction logic for CadastroFormaPgto.xaml
    /// </summary>
    public partial class CadastroFormaPgto : ModernWindow
    {
        private ProviderInterfacesCadastros2<Fpagamentos> provider;

        public CadastroFormaPgto(Window owner, ConnectionDb conexao)
        {
            InitializeComponent();

            provider = new ProviderInterfacesCadastros2<Fpagamentos>(this, buttons, conexao);
            provider.LoadRegistros("Fpagamentos", "Fpa_codigo");

            provider.Event_ValidacaoCamposCorrenteEventHandler += new ExecucaoCommandsComDaoSqlBooleanEventHandler<Fpagamentos>(Validacao);
            provider.Event_GetNewRecordEventHandler += new ExecutacaoCommandsComDaoSqlModelEventHandler<Fpagamentos>(GetNewRecod);
            provider.Event_PesquisaEventHandler += new ModelPesquisaEventHandler(ExecutaPesquisa);
            provider.Event_ConfirmacaoDeleteEventHandler += new ExecucaoCommandsComDaoSqlBooleanEventHandler<Fpagamentos>(ConfirmacaoDelete);

            this.DataContext = provider;

            SistemaGlobal.Sis.DefinicoesUsuario.SetDefinicoesWindow(this, SIF.Commom.UssessaoEnum.CADASTRO_FPAGAMENTO, buttons);

            using (EffectWindow e = new EffectWindow())
            {
                e.SetEffectBackgound(owner, this);
            }
            this.PreviewKeyDown += new KeyEventHandler(W_PreviewKeyDown);
        }

        private bool ConfirmacaoDelete(Fpagamentos model)
        {
            if (SistemaGlobal.Sis.Msg.MostraMensagem("Deseja realmente excluir o registro?", "Atenção", MessageBoxButton.YesNo, this) == MessageBoxResult.Yes)
                return true;
            return false;
        }

        private void ExecutaPesquisa()
        {
            using (FpagamentosPes pes = new FpagamentosPes(SistemaGlobal.Sis.Connection))
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

        private Fpagamentos GetNewRecod()
        {
            SelecionaText(txtDescricao);

            Fpagamentos fpagamento = Fpagamentos.GetNewFpagamentos();
            fpagamento.Fpa_codigo = (provider.CountRegistros + 1).ToString().PadLeft(2, '0');
            return fpagamento;
        }

        private bool Validacao(Fpagamentos model)
        {
            if (string.IsNullOrEmpty(model.Fpa_codigo))
            {
                SistemaGlobal.Sis.Msg.MostraMensagem("O campo Código da Forma de pagamento não pode ser vazio!", "Atenção", MessageBoxButton.OK, this);
                return SelecionaText(txtCodigo);
            }
            if (string.IsNullOrEmpty(model.Fpa_descricao))
            {
                SistemaGlobal.Sis.Msg.MostraMensagem("O campo Descrição da Forma de pagamento não pode ser vazio!", "Atenção", MessageBoxButton.OK, this);
                return SelecionaText(txtDescricao);
            }
            if (provider.ComandoCorrente == ControleButtons.New)
            {
                //if ((from x in provider.ListaRecord where x.Fpa_codigo == model.Fpa_codigo select x).FirstOrDefault() != null)
                //if (provider.Tabela.Select(string.Format("Fpa_codigo = '{0}'", model.Fpa_codigo)).FirstOrDefault() != null)
                //{
                //    SistemaGlobal.Sis.Msg.MostraMensagem("Já existe uma Forma de Pagamento com o mesmo código!", "Atenção", MessageBoxButton.OK, this);
                //    return SelecionaText(txtCodigo);
                //}
                int count = this.provider.Connection.GetValue<int>(string.Format("SELECT COUNT(Fpa_codigo) FROM dbo.Fpagamentos WHERE Fpa_codigo = '{0}'", model.Fpa_codigo));
                if (count > 0)
                {
                    SistemaGlobal.Sis.Msg.MostraMensagem("Já existe uma Forma de Pagamento com o mesmo código!", "Atenção", MessageBoxButton.OK, this);
                    return SelecionaText(txtCodigo);
                }
            }
            return true;
        }
    }
}
