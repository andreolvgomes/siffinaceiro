using SIF.WPF.Styles.Windows.Controls;
using SIF.Dao;
using SIF.Aplicacao.Providers;
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

namespace SIF.Aplicacao.LayoutFinanceiroW
{
    /// <summary>
    /// Interaction logic for Baxa.xaml
    /// </summary>
    public partial class BaixaContas
    {
        private ProviderBaixasContas provider;

        public BaixaContas(Window owner, ConnectionDb conexao, ContasReceberPagar interf)
        {
            InitializeComponent();

            provider = new ProviderBaixasContas(this, buttons, conexao, interf);

            provider.Provider.Event_GetNewRecordEventHandler += new ExecutacaoCommandsComDaoSqlModelEventHandler<Crbaixas>(NovaBaixa);
            provider.Provider.Event_ValidacaoCamposCorrenteEventHandler += new ExecucaoCommandsComDaoSqlBooleanEventHandler<Crbaixas>(ValidaCampos);
            provider.Provider.Event_EntidadeInseridaComSucessoEventHandler += new ExecutaCommandsBemSucedidaEventHandler<Crbaixas>(BaixadaComSucesso);
            provider.Provider.Event_PesquisaEventHandler += new ModelPesquisaEventHandler(PesquisaTodasAsBaixas);
            provider.Provider.Event_CommandExecutadoEventHandler += new ExecutaCommandsNotificacaoEventHandler(ComandoExecutado);
            provider.Provider.Event_ConfirmacaoDeleteEventHandler += new ExecucaoCommandsComDaoSqlBooleanEventHandler<Crbaixas>(ConfirmacaoDelete);

            this.DataContext = provider;

            SistemaGlobal.Sis.DefinicoesUsuario.SetDefinicoesWindow(this, (interf == ContasReceberPagar.ContasPagar) ? SIF.Commom.UssessaoEnum.BAIXA_CONTAS_PAGAR : SIF.Commom.UssessaoEnum.BAIXA_CONTAS_RECEBER, buttons);

            using (EffectWindow e = new EffectWindow())
            {
                e.SetEffectBackgound(owner, this);
            }
            SelecionaText(txtConta);
            this.PreviewKeyDown += new KeyEventHandler(W_PreviewKeyDown);
        }

        private bool ConfirmacaoDelete(Crbaixas model)
        {
            if (SistemaGlobal.Sis.Msg.MostraMensagem("Deseja realmente excluir o registro?", "Atenção", MessageBoxButton.YesNo, this) == MessageBoxResult.Yes)
                return true;
            return false;
        }

        private void ComandoExecutado(ButtonType command)
        {
            if (command == ButtonType.New)
                provider.SomenteLeitura = false;
            else if (command == ButtonType.Save)
                provider.SomenteLeitura = true;
            else if (command == ButtonType.Cancel)
                provider.SomenteLeitura = true;
        }

        private void PesquisaTodasAsBaixas()
        {
            using (CrfinanceiroPes pes = new CrfinanceiroPes(((provider.IntefaceLayout == ContasReceberPagar.ContasPagar) ? TipoPesquisaFinanceiro.Contas_pagar_baixada : TipoPesquisaFinanceiro.Contas_receber_baixada), SistemaGlobal.Sis.Connection))
            {
                pes.Pesquisa(this);
            }
        }

        private void BaixadaComSucesso(Crbaixas model, bool sucesso)
        {
            provider.GravaBaixa();
            SelecionaText(txtFpagamento);
        }

        private bool ValidaCampos(Crbaixas model)
        {
            if (!provider.ValidaFpagamento(this)) return SelecionaText(txtFpagamento);
            if (!provider.ValidaPlanoContas(this)) return SelecionaText(txtPlanoContas);
            if (!provider.ValidaCaixa(this)) return SelecionaText(txtCaixa);
            if (!provider.ValidaDataBaixa(this)) return SelecionaText(txtDtBaixa);
            if (!provider.ValidaValor(this)) return SelecionaText(txtValor);

            return true;
        }

        private Crbaixas NovaBaixa()
        {
            SelecionaText(txtConta);
            provider.CrFinanaceiro = new Crfinanceiro();
            return new Crbaixas();
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
                using (CrfinanceiroPes pes = new CrfinanceiroPes(((provider.IntefaceLayout == ContasReceberPagar.ContasPagar) ? TipoPesquisaFinanceiro.Contas_pagar : TipoPesquisaFinanceiro.Contas_receber), SistemaGlobal.Sis.Connection))
                {
                    Crfinanceiro crfinanceiro = pes.Pesquisa(this);
                    if (crfinanceiro != null)
                        provider.SetCrfinanceiro(crfinanceiro);
                    SelecionaText(txtFpagamento);
                }
            }
        }

        internal void SetCrmfinanceiro(Crfinanceiro crfinanceiro)
        {
            provider.SetCrfinanceiro(crfinanceiro);
        }

        private void textBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (!provider.SomenteLeitura)
                    PesquisaEnter(GetPesquisa(sender as TextBox));
            }
            else if (e.Key == Key.F2)
            {
                if (!provider.SomenteLeitura)
                    PesquisaF2(GetPesquisa(sender as TextBox));
            }
        }

        private void PesquisaEnter(TextBoxPesquisa pesquisa)
        {
            EstruturaPesquisa result;
            switch (pesquisa)
            {
                case TextBoxPesquisa.Contas:
                    if (string.IsNullOrEmpty(provider.Provider.Entidade.CodigoConta))
                    {
                        PesquisaF2(pesquisa);
                    }
                    else
                    {
                        result = CrfinanceiroPes.PegaCrfinanceiro(SistemaGlobal.Sis.Connection, Convert.ToInt16(provider.Provider.Entidade.CodigoConta));
                        switch (result.Resultado)
                        {
                            case ResultadoPesquisa.ENCONTRADO:
                                provider.SetCrfinanceiro(result.Crfinanceiros.FirstOrDefault());
                                SelecionaText(txtFpagamento);
                                break;
                            case ResultadoPesquisa.NAO_ENCONTRADO:
                                SistemaGlobal.Sis.Msg.MostraMensagem("Conta não encontrada!", "Atenção", MessageBoxButton.OK, this);
                                SelecionaText(txtConta);
                                break;
                            case ResultadoPesquisa.VARIOS_ENCONTRADO:
                                SistemaGlobal.Sis.Msg.MostraMensagem("Conta não encontrada!", "Atenção", MessageBoxButton.OK, this);
                                SelecionaText(txtConta);
                                break;
                        }
                    }
                    break;
                case TextBoxPesquisa.FormasPagamento:
                    if (string.IsNullOrEmpty(provider.Provider.Entidade.DescricaoFpagamento))
                    {
                        PesquisaF2(pesquisa);
                    }
                    else
                    {
                        result = FpagamentosPes.PegaFpagamentoByDescricao(SistemaGlobal.Sis.Connection, provider.Provider.Entidade.DescricaoFpagamento);
                        switch (result.Resultado)
                        {
                            case ResultadoPesquisa.ENCONTRADO:
                                provider.SetFpagamento(result.Fpagamentos.FirstOrDefault());
                                SelecionaText(txtPlanoContas);
                                break;
                            case ResultadoPesquisa.NAO_ENCONTRADO:
                                SistemaGlobal.Sis.Msg.MostraMensagem("Forma de pagamento não cadastrada!", "Atenção", MessageBoxButton.OK, this);
                                SelecionaText(txtFpagamento);
                                break;
                            case ResultadoPesquisa.VARIOS_ENCONTRADO:
                                PesquisaF2(pesquisa);
                                break;
                        }
                    }
                    break;
                case TextBoxPesquisa.PlanosContas:
                    if (string.IsNullOrEmpty(provider.Provider.Entidade.Pla_numeroconta))
                    {
                        PesquisaF2(pesquisa);
                    }
                    else
                    {
                        result = PlanoContasPes.PegaPlanoPagamento(SistemaGlobal.Sis.Connection, provider.Provider.Entidade.Pla_numeroconta);
                        switch (result.Resultado)
                        {
                            case ResultadoPesquisa.ENCONTRADO:
                                SelecionaText(txtCaixa);
                                break;
                            case ResultadoPesquisa.NAO_ENCONTRADO:
                                SistemaGlobal.Sis.Msg.MostraMensagem("Plano de Contas não cadastrado!", "Atenção", MessageBoxButton.OK, this);
                                SelecionaText(txtPlanoContas);
                                break;
                            case ResultadoPesquisa.VARIOS_ENCONTRADO:
                                PesquisaF2(pesquisa);
                                break;
                        }
                    }
                    break;
                case TextBoxPesquisa.Caixa:
                    if (string.IsNullOrEmpty(provider.Provider.Entidade.DescricaoCaixa))
                    {
                        PesquisaF2(pesquisa);
                    }
                    else
                    {
                        result = CaixasPes.PegaCaixaByDescricao(SistemaGlobal.Sis.Connection, provider.Provider.Entidade.DescricaoCaixa);
                        switch (result.Resultado)
                        {
                            case ResultadoPesquisa.ENCONTRADO:
                                provider.SetCaixa(result.Caixas.FirstOrDefault());
                                SelecionaText(txtDtBaixa);
                                break;
                            case ResultadoPesquisa.NAO_ENCONTRADO:
                                SistemaGlobal.Sis.Msg.MostraMensagem("Caixa não cadastrado!", "Atenção", MessageBoxButton.OK, this);
                                SelecionaText(txtCaixa);
                                break;
                            case ResultadoPesquisa.VARIOS_ENCONTRADO:
                                PesquisaF2(pesquisa);
                                break;
                        }
                    }
                    break;
            }
        }

        private void PesquisaF2(TextBoxPesquisa pesquisa)
        {
            switch (pesquisa)
            {
                case TextBoxPesquisa.Contas:
                    if (provider.Provider.ComandoCorrente == ControleButtons.New)
                    {
                        using (CrfinanceiroPes pes = new CrfinanceiroPes((provider.IntefaceLayout == ContasReceberPagar.ContasPagar) ? TipoPesquisaFinanceiro.Contas_pagar_nao_baixada : TipoPesquisaFinanceiro.Contas_receber_nao_baixada, SistemaGlobal.Sis.Connection))
                        {
                            Crfinanceiro crfinanceiro = pes.Pesquisa(this);
                            if (crfinanceiro != null)
                            {
                                provider.SetCrfinanceiro(crfinanceiro);
                                SelecionaText(txtFpagamento);
                            }
                            else
                            {
                                SelecionaText(txtConta);
                            }
                        }
                    }
                    break;
                case TextBoxPesquisa.FormasPagamento:
                    using (FpagamentosPes pes = new FpagamentosPes(SistemaGlobal.Sis.Connection))
                    {
                        Fpagamentos fp = pes.Pesquisa(this);
                        if (fp != null)
                        {
                            provider.SetFpagamento(fp);
                            SelecionaText(txtPlanoContas);
                        }
                        else
                        {
                            SelecionaText(txtFpagamento);
                        }
                    }
                    break;
                case TextBoxPesquisa.PlanosContas:
                    using (PlanoContasPes pes = new PlanoContasPes(SistemaGlobal.Sis.Connection))
                    {
                        Planocontas pl = pes.Pesquisa(this);
                        if (pl != null)
                        {
                            provider.SetPlanoContas(pl);
                            SelecionaText(txtCaixa);
                        }
                        else
                        {
                            SelecionaText(txtPlanoContas);
                        }
                    }
                    break;
                case TextBoxPesquisa.Caixa:
                    using (CaixasPes pes = new CaixasPes(SistemaGlobal.Sis.Connection))
                    {
                        Caixas cx = pes.Pesquisa(this);
                        if (cx != null)
                        {
                            provider.SetCaixa(cx);

                        }
                        else
                        {
                            SelecionaText(txtCaixa);
                        }
                    }
                    break;
            }
        }

        private TextBoxPesquisa GetPesquisa(TextBox text)
        {
            switch (text.Name)
            {
                case "txtCaixa":
                    return TextBoxPesquisa.Caixa;
                case "txtPlanoContas":
                    return TextBoxPesquisa.PlanosContas;
                case "txtFpagamento":
                    return TextBoxPesquisa.FormasPagamento;
                case "txtConta":
                    return TextBoxPesquisa.Contas;
            }
            return TextBoxPesquisa.Nenhum;
        }

        private void txtValor_GotKeyboardFocus_1(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (Convert.ToDecimal(provider.Provider.Entidade.ValorBaixa) == 0)
            {
                provider.Provider.Entidade.ValorBaixa = provider.CrFinanaceiro.Crf_valorareceber.ConvertToString();
                if (Convert.ToDecimal(provider.Provider.Entidade.ValorBaixa) == 0)
                    provider.Provider.Entidade.ValorBaixa = provider.CrFinanaceiro.Crf_valorparcela.ConvertToString();
            }
        }

        private void TextBox_PreviewKeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (provider.Provider.ComandoCorrente == ControleButtons.New)
                    provider.Provider.Control_Buttons(ButtonType.Save);
                e.Handled = true;
            }
        }

        internal bool ExecutaBaixa(int crf_sequencial)
        {
            provider.Provider.Control_Buttons(ButtonType.New);
            Crfinanceiro crfinanceiro = SistemaGlobal.Sis.Connection.GetRegistros<Crfinanceiro>(string.Format("Crf_sequencial = {0}", crf_sequencial)).FirstOrDefault();
            provider.SetCrfinanceiro(crfinanceiro);
            SelecionaText(txtFpagamento);
            this.ShowDialog();

            return true;
        }
    }
}
