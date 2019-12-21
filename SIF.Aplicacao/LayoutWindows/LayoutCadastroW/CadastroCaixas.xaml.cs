using SIF.WPF.Styles.Windows.Controls;
using SIF.Dao;
using SIF.Aplicacao.Providers;
using SIF.Aplicacao.UserControls;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for CadastroCaixas.xaml
    /// </summary>
    public partial class CadastroCaixas : ModernWindow
    {

        private ProviderCadCaixas provider;

        public CadastroCaixas(Window owner, ConnectionDb conexao)
        {
            InitializeComponent();

            provider = new ProviderCadCaixas(this, buttons, conexao);
            this.DataContext = provider;

            provider.Event_ValidacaoCamposCorrenteEventHandler += new ExecucaoCommandsComDaoSqlBooleanEventHandler<Caixas>(Validacao);
            provider.Event_GetNewRecordEventHandler += new ExecutacaoCommandsComDaoSqlModelEventHandler<Caixas>(GetNewCaixa);
            provider.Event_PesquisaEventHandler += new ModelPesquisaEventHandler(ExecutaPesquisa);
            provider.Event_ConfirmacaoDeleteEventHandler += new ExecucaoCommandsComDaoSqlBooleanEventHandler<Caixas>(ConfirmacaoDelete);

            provider.LoadRegistros("Caixas", "Cai_codigo");

            using (EffectWindow e = new EffectWindow())
            {
                e.SetEffectBackgound(owner, this);
            }
            SistemaGlobal.Sis.DefinicoesUsuario.SetDefinicoesWindow(this, SIF.Commom.UssessaoEnum.CADASTRO_CAIXA, buttons);
            this.PreviewKeyDown += new KeyEventHandler(W_PreviewKeyDown);
        }

        private bool ConfirmacaoDelete(Caixas model)
        {
            if (SistemaGlobal.Sis.Msg.MostraMensagem("Deseja realmente excluir o registro?", "Atenção", MessageBoxButton.YesNo, this) == MessageBoxResult.Yes)
                return true;
            return false;
        }

        private void ExecutaPesquisa()
        {
            using (CaixasPes pes = new CaixasPes(SistemaGlobal.Sis.Connection))
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

        private Caixas GetNewCaixa()
        {    
            SelecionaText(txtDescricao);
            Caixas caixa = Caixas.GetNewCaixa();
            caixa.Cai_codigo = (provider.CountRegistros + 1).ToString().PadLeft(3, '0');
            provider.ViewMovimentacao = new DataView();
            return caixa;
        }

        private bool Validacao(Caixas model)
        {
            if (string.IsNullOrEmpty(model.Cai_codigo))
            {
                SistemaGlobal.Sis.Msg.MostraMensagem("Campo Código do Caixa não pode ser vazio!", "Atenção", MessageBoxButton.OK, this);
                return SelecionaText(txtCodigo);
            }
            if (string.IsNullOrEmpty(model.Cai_descricao))
            {
                SistemaGlobal.Sis.Msg.MostraMensagem("Campo Descrição do Caixa não pode ser vazio!", "Atenção", MessageBoxButton.OK, this);
                return SelecionaText(txtDescricao);
            }
            if (provider.ComandoCorrente == ControleButtons.New)
            {
                int count = this.provider.Connection.GetValue<int>(string.Format("SELECT COUNT(Cai_codigo) FROM dbo.Caixas WHERE Cai_codigo = '{0}'", model.Cai_codigo));
                if (count > 0)
                {
                    SistemaGlobal.Sis.Msg.MostraMensagem("Já existe um Caixa cadastrado com o mesmo código!", "Atenção", MessageBoxButton.OK, this);
                    return SelecionaText(txtCodigo);
                }
            }
            return true;
        }
    }
}
