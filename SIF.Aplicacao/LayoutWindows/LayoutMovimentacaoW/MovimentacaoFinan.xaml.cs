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

namespace SIF.Aplicacao.LayoutMovimentacao
{
    public partial class MovimentacaoFinan
    {
        private ProviderMovimentacaoFinan provider;

        public MovimentacaoFinan(Window owner, ConnectionDb conexao)
        {
            InitializeComponent();

            provider = new ProviderMovimentacaoFinan(this, buttons, conexao);
            provider.Provider.Event_GetNewRecordEventHandler += new ExecutacaoCommandsComDaoSqlModelEventHandler<Camovimentos>(NovoMovimento);
            provider.Provider.Event_ValidacaoCamposCorrenteEventHandler += new ExecucaoCommandsComDaoSqlBooleanEventHandler<Camovimentos>(Validacao);
            provider.Provider.Event_PesquisaEventHandler += new ModelPesquisaEventHandler(ExecutaPesquisa);
            provider.Provider.Event_ConfirmacaoDeleteEventHandler += new ExecucaoCommandsComDaoSqlBooleanEventHandler<Camovimentos>(ConfirmacaoDelete);

            this.DataContext = provider;

            SistemaGlobal.Sis.DefinicoesUsuario.SetDefinicoesWindow(this, SIF.Commom.UssessaoEnum.LANCAMENTO_MOVIMENTACAO, buttons);

            using (EffectWindow e = new EffectWindow())
            {
                e.SetEffectBackgound(owner, this);
            }

            SelecionaText(txtCliente);
            this.PreviewKeyDown += new KeyEventHandler(W_PreviewKeyDown);
        }

        private bool ConfirmacaoDelete(Camovimentos model)
        {
            if (SistemaGlobal.Sis.Msg.MostraMensagem("Deseja realmente excluir o registro?", "Atenção", MessageBoxButton.YesNo, this) == MessageBoxResult.Yes)
                return true;
            return false;
        }

        private void ExecutaPesquisa()
        {
            using (CamovimentosPes pes = new CamovimentosPes(SistemaGlobal.Sis.Connection))
            {
                Camovimentos camoviemntos = pes.Pesquisa(this);
                if (camoviemntos != null)
                {
                    provider.SetCamovimentos(camoviemntos);
                    SelecionaText(txtCliente);
                }
            }
        }

        private bool Validacao(Camovimentos model)
        {
            if (!provider.ValidaCliente(this)) return SelecionaText(txtCliente);
            if (!provider.ValidaFpagamento(this)) return SelecionaText(txtFpagamento);
            if (!provider.ValidaPlanoContas(this)) return SelecionaText(txtPlanoContas);
            if (!provider.ValidaCaixa(this)) return SelecionaText(txtCaixa);
            if (!provider.ValidaDataLancamento(this)) return SelecionaText(txtDataLancamento);
            if (!provider.ValidaValor(this)) return SelecionaText(txtValor);
            if (!provider.ValidaTipoMovimento(this)) { cmbTipoMov.Focus(); return false; };
            return true;
        }

        private Camovimentos NovoMovimento()
        {
            SelecionaText(txtCliente);
            provider.SomenteLeitura = false;
            Camovimentos mov = new Camovimentos();
            mov.DataLancamento = DateTime.Now.ConvertToString();
            return mov;
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

        private void Txt_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PesquisaEnter(GetPesquisa(sender as TextBox));
            }
            else if (e.Key == Key.F2)
            {
                PesquisaF2(GetPesquisa(sender as TextBox));
            }
        }

        private void PesquisaF2(TextBoxPesquisa textpes)
        {
            switch (textpes)
            {
                case TextBoxPesquisa.Clientes:
                    using (ClientesPes pes = new ClientesPes(SistemaGlobal.Sis.Connection))
                    {
                        Clientes cliente = pes.Pesquisa(this);
                        if (cliente != null)
                        {
                            provider.SetCliente(cliente);
                            SelecionaText(txtFpagamento);
                        }
                        else
                        {
                            SelecionaText(txtCliente);
                        }
                    }
                    break;
                case TextBoxPesquisa.FormasPagamento:
                    using (FpagamentosPes pes = new FpagamentosPes(SistemaGlobal.Sis.Connection))
                    {
                        Fpagamentos fpagamento = pes.Pesquisa(this);
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
                    using (PlanoContasPes pes = new PlanoContasPes(SistemaGlobal.Sis.Connection))
                    {
                        Planocontas planoContas = pes.Pesquisa(this);
                        if (planoContas != null)
                        {
                            provider.SetPlanoContas(planoContas);
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
                        Caixas caixa = pes.Pesquisa(this);
                        if (caixa != null)
                        {
                            provider.SetCaixa(caixa);
                            SelecionaText(txtDataLancamento);
                        }
                        else
                        {
                            SelecionaText(txtCaixa);
                        }
                    }
                    break;
                case TextBoxPesquisa.TipoMovimento:
                    break;
            }
        }

        private void PesquisaEnter(TextBoxPesquisa textpes)
        {
            EstruturaPesquisa result;
            switch (textpes)
            {
                case TextBoxPesquisa.Clientes:
                    if (string.IsNullOrEmpty(provider.Provider.Entidade.NomeCliente))
                    {
                        PesquisaF2(textpes);
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
                                SelecionaText(txtCliente);
                                break;
                            case ResultadoPesquisa.VARIOS_ENCONTRADO:
                                PesquisaF2(textpes);
                                break;
                        }
                    }
                    break;
                case TextBoxPesquisa.FormasPagamento:
                    if (string.IsNullOrEmpty(provider.Provider.Entidade.FpagamentoDescricao))
                    {
                        PesquisaF2(textpes);
                    }
                    else
                    {
                        result = FpagamentosPes.PegaFpagamentoByDescricao(SistemaGlobal.Sis.Connection, provider.Provider.Entidade.FpagamentoDescricao);
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
                                PesquisaF2(textpes);
                                break;
                        }
                    }
                    break;
                case TextBoxPesquisa.PlanosContas:
                    if (string.IsNullOrEmpty(provider.Provider.Entidade.Pla_numeroconta))
                    {
                        PesquisaF2(textpes);
                    }
                    else
                    {
                        result = PlanoContasPes.PegaPlanoPagamento(SistemaGlobal.Sis.Connection, provider.Provider.Entidade.Pla_numeroconta);
                        switch (result.Resultado)
                        {
                            case ResultadoPesquisa.ENCONTRADO:
                                provider.SetPlanoContas(result.PlanoContas.FirstOrDefault());
                                break;
                            case ResultadoPesquisa.NAO_ENCONTRADO:
                                SistemaGlobal.Sis.Msg.MostraMensagem("Plano de Contas não cadastrado!", "Atenção", MessageBoxButton.OK, this);
                                SelecionaText(txtPlanoContas);
                                break;
                            case ResultadoPesquisa.VARIOS_ENCONTRADO:
                                PesquisaF2(textpes);
                                break;
                        }
                    }
                    break;
                case TextBoxPesquisa.Caixa:
                    if (string.IsNullOrEmpty(provider.Provider.Entidade.Caixa))
                    {
                        PesquisaF2(textpes);
                    }
                    else
                    {
                        result = CaixasPes.PegaCaixaByDescricao(SistemaGlobal.Sis.Connection, provider.Provider.Entidade.Caixa);
                        switch (result.Resultado)
                        {
                            case ResultadoPesquisa.ENCONTRADO:
                                provider.SetCaixa(result.Caixas.FirstOrDefault());
                                break;
                            case ResultadoPesquisa.NAO_ENCONTRADO:
                                SistemaGlobal.Sis.Msg.MostraMensagem("Caixa não cadastrado!", "Atenção", MessageBoxButton.OK, this);
                                SelecionaText(txtCaixa);
                                break;
                            case ResultadoPesquisa.VARIOS_ENCONTRADO:
                                PesquisaF2(textpes);
                                break;
                        }
                    }
                    break;
                case TextBoxPesquisa.TipoMovimento:
                    break;
            }
        }

        private TextBoxPesquisa GetPesquisa(TextBox box)
        {
            switch (box.Name)
            {
                case "txtCliente": return TextBoxPesquisa.Clientes;
                case "txtFpagamento": return TextBoxPesquisa.FormasPagamento;
                case "txtPlanoContas": return TextBoxPesquisa.PlanosContas;
                case "txtCaixa": return TextBoxPesquisa.Caixa;
                case "txtTipoMovimento": return TextBoxPesquisa.TipoMovimento;
            }
            return TextBoxPesquisa.Nenhum;
        }
    }
}
