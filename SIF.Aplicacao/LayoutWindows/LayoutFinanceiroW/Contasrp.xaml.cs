using SIF.WPF.Styles.Windows.Controls;
using SIF.Dao;
using SIF.Aplicacao.LayoutWindows.LayoutFinanceiroW;
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
using System.Windows.Threading;

namespace SIF.Aplicacao.LayoutFinanceiroW
{
    /// <summary>
    /// Interaction logic for Contasrp.xaml
    /// </summary>
    public partial class Contasrp
    {
        private ProviderCrp provider;
        private List<TextBox> listTextBox = new List<TextBox>();

        public Contasrp(Window owner, ConnectionDb conexao, ContasReceberPagar interf)
            : this(owner, conexao, interf, false)
        {
        }
        public Contasrp(Window owner, ConnectionDb conexao, ContasReceberPagar interf, bool somenteUmaCr)
        {
            InitializeComponent();

            provider = new ProviderCrp(this, buttons, conexao, interf);
            
            provider.Provider.Event_ValidacaoCamposCorrenteEventHandler += new ExecucaoCommandsComDaoSqlBooleanEventHandler<Crfinanceiro>(Validacao);
            provider.Provider.Event_GetNewRecordEventHandler += new ExecutacaoCommandsComDaoSqlModelEventHandler<Crfinanceiro>(GetNewRecod);
            provider.Provider.Event_PesquisaEventHandler += new ModelPesquisaEventHandler(ExecutaPesquisa);
            provider.Provider.Event_ConfirmacaoDeleteEventHandler += new ExecucaoCommandsComDaoSqlBooleanEventHandler<Crfinanceiro>(ConfirmacaoDelete);

            this.DataContext = provider;

            SistemaGlobal.Sis.DefinicoesUsuario.SetDefinicoesWindow(this, (interf == ContasReceberPagar.ContasPagar) ? SIF.Commom.UssessaoEnum.LANCAMENTO_CONTAS_PAGAR : SIF.Commom.UssessaoEnum.LANCAMENTO_CONTAS_RECEBER, buttons);

            using (EffectWindow f = new EffectWindow())
            {
                f.SetEffectBackgound(owner, this);
            }
            if (!somenteUmaCr)
            {
                this.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                {
                    provider.CarregaRegistros();
                }));
            }

            this.listTextBox = this.FindChildren<TextBox>().ToList();
            this.provider.Event_TextBoxSomenteIsReadOnlyEventhandler += new TextBoxSomenteIsReadOnlyEventhandler(NotifyTextBoxIsReadOnly);
            this.PreviewKeyDown += new KeyEventHandler(W_PreviewKeyDown);
        }

        private void NotifyTextBoxIsReadOnly(bool isReadOny)
        {
            foreach (TextBox text in this.listTextBox)
                text.IsReadOnly = isReadOny;
        }

        private bool ConfirmacaoDelete(Crfinanceiro model)
        {
            if (SistemaGlobal.Sis.Msg.MostraMensagem("Deseja realmente excluir o registro?", "Atenção", MessageBoxButton.YesNo, this) == MessageBoxResult.Yes)
                return true;
            return false;
        }

        private void ExecutaPesquisa()
        {
            using (CrfinanceiroPes pes = new CrfinanceiroPes(((provider.ControleLayout.IntefaceLayout == ContasReceberPagar.ContasPagar) ? TipoPesquisaFinanceiro.Contas_pagar_nao_baixada : TipoPesquisaFinanceiro.Contas_receber_nao_baixada), SistemaGlobal.Sis.Connection))
            {
                provider.Provider.SetEntidade(pes.Pesquisa(this));
            }
        }

        private void W_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Up)
            {
                UIElement ele = e.OriginalSource as UIElement;
                if (e.Key == Key.Enter)
                    ele.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                else
                    ele.MoveFocus(new TraversalRequest(FocusNavigationDirection.Previous));
            }
            else if (e.SystemKey == Key.F10)
            {
                ExecutaPesquisa();
                e.Handled = true;
            }
        }

        private Crfinanceiro GetNewRecod()
        {            
            Crfinanceiro f = new Crfinanceiro();
            if (provider.ControleLayout.IntefaceLayout == ContasReceberPagar.ContasPagar)
            {
                f.Crf_tipoconta = "CP";
                f.Crf_ndocumento = string.Format("CP{0}", DateTime.Now.ToString("ddMMyyyyHHmm"));
            }
            else
            {
                f.Crf_tipoconta = "CR";
                f.Crf_ndocumento = string.Format("CR{0}", DateTime.Now.ToString("ddMMyyyyHHmm"));
            }
            f.Vencimento = DateTime.Now.AddDays(30).Date.ConvertToString();
            f.Crf_parcela = "01";
            provider.Status = false;

            NotifyTextBoxIsReadOnly(false);
            SelecionaText(txtNdocumento);
            return f;
        }

        private bool Validacao(Crfinanceiro model)
        {
            if (!provider.ValidaNdocumento(this)) return SelecionaText(txtNdocumento);
            if (!provider.ValidaVencimento(this)) return SelecionaText(txtVencimento);
            if (!provider.ValidaParcela(this)) return SelecionaText(txtParcela);
            if (!provider.ValidaCliente(this)) return SelecionaText(txtClienteForn);
            if (!provider.ValidaFpagamento(this)) return SelecionaText(txtFpagamento);
            if (!provider.ValidaPlanoContas(this)) return SelecionaText(txtPlanoContas);
            if (!provider.ValidaValor(this)) return SelecionaText(txtValor);
            return true;
        }

        private void TextBox_PreviewKeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F2)
            {
                ClientesPes pes = new ClientesPes(SistemaGlobal.Sis.Connection);
                Clientes cliente = pes.Pesquisa(this);
                e.Handled = true;
            }
        }

        private void txt_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F2)
            {
                PesquisaF2(GetPesqusia(sender as TextBox));
            }
            else if (e.Key == Key.Enter)
            {
                PesqusiaEnter(GetPesqusia(sender as TextBox));
            }
        }

        private void PesqusiaEnter(TextBoxPesquisa pes)
        {
            EstruturaPesquisa result;
            switch (pes)
            {
                case TextBoxPesquisa.Clientes:
                    if (string.IsNullOrEmpty(provider.Provider.Entidade.NomeCliente))
                    {
                        PesquisaF2(pes);
                    }
                    else
                    {
                        result = ClientesPes.PegaCliente(SistemaGlobal.Sis.Connection, provider.Provider.Entidade.NomeCliente);
                        switch (result.Resultado)
                        {
                            case ResultadoPesquisa.ENCONTRADO:
                                provider.SetCliente(result.Clientes.FirstOrDefault());
                                break;
                            case ResultadoPesquisa.NAO_ENCONTRADO:
                                SistemaGlobal.Sis.Msg.MostraMensagem("Cliente não cadastrado!", "Atenção", MessageBoxButton.OK, this);
                                SelecionaText(txtClienteForn);
                                break;
                            case ResultadoPesquisa.VARIOS_ENCONTRADO:
                                PesquisaF2(pes);
                                break;
                        }
                    }
                    break;
                case TextBoxPesquisa.FormasPagamento:
                    if (string.IsNullOrEmpty(provider.Provider.Entidade.DescricaoFpagamento))
                    {
                        PesquisaF2(pes);
                    }
                    else
                    {
                        result = FpagamentosPes.PegaFpagamentoByDescricao(SistemaGlobal.Sis.Connection, provider.Provider.Entidade.DescricaoFpagamento);
                        switch (result.Resultado)
                        {
                            case ResultadoPesquisa.ENCONTRADO:
                                provider.SetFpagamento(result.Fpagamentos.FirstOrDefault());
                                break;
                            case ResultadoPesquisa.NAO_ENCONTRADO:
                                SistemaGlobal.Sis.Msg.MostraMensagem("Forma de pagamento não cadastrada!", "Atenção", MessageBoxButton.OK, this);
                                SelecionaText(txtFpagamento);
                                break;
                            case ResultadoPesquisa.VARIOS_ENCONTRADO:
                                PesquisaF2(pes);
                                break;
                        }
                    }
                    break;
                case TextBoxPesquisa.PlanosContas:
                    if (string.IsNullOrEmpty(provider.Provider.Entidade.Pla_numeroconta))
                    {
                        PesquisaF2(pes);
                    }
                    else
                    {
                        result = PlanoContasPes.PegaPlanoPagamento(SistemaGlobal.Sis.Connection, provider.Provider.Entidade.Pla_numeroconta);
                        switch (result.Resultado)
                        {
                            case ResultadoPesquisa.ENCONTRADO:
                                break;
                            case ResultadoPesquisa.NAO_ENCONTRADO:
                                SistemaGlobal.Sis.Msg.MostraMensagem("Plano de Contas não cadastrado!", "Atenção", MessageBoxButton.OK, this);
                                SelecionaText(txtPlanoContas);
                                break;
                            case ResultadoPesquisa.VARIOS_ENCONTRADO:
                                PesquisaF2(pes);
                                break;
                        }
                    }
                    break;
            }
        }

        private void PesquisaF2(TextBoxPesquisa pes)
        {
            switch (pes)
            {
                case TextBoxPesquisa.Clientes:
                    using (ClientesPes ps = new ClientesPes(SistemaGlobal.Sis.Connection))
                    {
                        Clientes cliente = ps.Pesquisa(this);
                        if (cliente != null)
                        {
                            provider.SetCliente(cliente);
                            SelecionaText(txtFpagamento);
                        }
                        else
                        {
                            SelecionaText(txtClienteForn);
                        }
                    }
                    break;
                case TextBoxPesquisa.FormasPagamento:
                    using (FpagamentosPes ps = new FpagamentosPes(SistemaGlobal.Sis.Connection))
                    {
                        Fpagamentos fpagamento = ps.Pesquisa(this);
                        if (fpagamento != null)
                        {
                            provider.SetFpagamento(fpagamento);
                            SelecionaText(txtPlanoContas);
                        }
                        else
                        {
                            SelecionaText(txtFpagamento);
                        }
                    }
                    break;
                case TextBoxPesquisa.PlanosContas:
                    using (PlanoContasPes ps = new PlanoContasPes(SistemaGlobal.Sis.Connection))
                    {
                        Planocontas plano = ps.Pesquisa(this);
                        if (plano != null)
                        {
                            provider.SetPlanoConta(plano);
                            SelecionaText(txtValor);
                        }
                        else
                        {
                            SelecionaText(txtPlanoContas);
                        }
                    }
                    break;
            }
        }

        private TextBoxPesquisa GetPesqusia(TextBox textBox)
        {
            switch (textBox.Name)
            {
                case "txtFpagamento": return TextBoxPesquisa.FormasPagamento;
                case "txtClienteForn": return TextBoxPesquisa.Clientes;
                case "txtPlanoContas": return TextBoxPesquisa.PlanosContas;
            }
            return TextBoxPesquisa.Nenhum;
        }

        private void txtValorAReceber_GotKeyboardFocus_1(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (provider.Provider.Entidade != null && Convert.ToDecimal(provider.Provider.Entidade.ValorAReceber) == 0)
            {
                provider.Provider.Entidade.ValorAReceber = provider.Provider.Entidade.ValorParcela;
            }
        }

        internal void VisualizaConta(int crf_sequencial)
        {
            //Crfinanceiro crfinanceiro = fian.GetRegistrosWhere(string.Format("Crf_sequencial = {0}", crf_sequencial)).FirstOrDefault();
            //provider.SetCrfinanceiro(crfinanceiro);
            provider.CarregaRegistros(crf_sequencial);
            SelecionaText(txtFpagamento);
            this.ShowDialog();
        }

        private void AnexarImage_Click(object sender, RoutedEventArgs e)
        {
            Comprovantes c = new Comprovantes(this);
            c.ShowDialog();
        }
    }
}
