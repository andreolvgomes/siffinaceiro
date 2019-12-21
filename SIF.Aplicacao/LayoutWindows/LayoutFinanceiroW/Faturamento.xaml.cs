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

namespace SIF.Aplicacao.LayoutWindows.LayoutFinanceiroW
{
    /// <summary>
    /// Interaction logic for Faturamento.xaml
    /// </summary>
    public partial class Faturamento //: Window
    {
        private ProviderFaturamento provider = null;

        public Faturamento(Window owner, ConnectionDb conexao)
        {
            InitializeComponent();

            this.provider = new ProviderFaturamento(conexao);
            this.NovoFaturamento();

            this.DataContext = this.provider;

            using (EffectWindow f = new EffectWindow())
            {
                f.SetEffectBackgound(owner, this);
            }

            this.PreviewKeyDown += new KeyEventHandler(W_PreviewKeyDown);
            this.Loaded += new RoutedEventHandler(W_Loaded);
        }

        private void NovoFaturamento()
        {
            this.provider.NovoFaturamento();
            this.SelecionaText(txtCli_nome);
        }

        private void W_Loaded(object sender, RoutedEventArgs e)
        {
            List<DataGrid> list = new List<DataGrid>() { dgvContas, dgvLacamentos };
            foreach (DataGrid dgv in list)
            {
                Decorator decorator = VisualTreeHelper.GetChild(dgv, 0) as Decorator;
                if (decorator != null)
                {
                    ScrollViewer scrollViewer = VisualTreeHelper.GetChild(decorator, 0) as ScrollViewer;
                    if (scrollViewer != null)
                        scrollViewer.ScrollChanged += new ScrollChangedEventHandler(ScrollChanged);
                }
            }
        }

        private void ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.ExtentHeightChange > 0)
            {
                ((ScrollViewer)sender).ScrollToEnd();
            }
        }

        private void W_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Up)
            {
                UIElement element = e.OriginalSource as UIElement;
                if (e.Key == Key.Enter)
                    element.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                else
                    element.MoveFocus(new TraversalRequest(FocusNavigationDirection.Previous));
            }
        }

        private void Consulta_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    this.EnterConsulta(((TextBoxConsultas)Enum.Parse(typeof(TextBoxConsultas), (sender as TextBox).Name)));
                    e.Handled = true;
                }
                else if (e.Key == Key.F2)
                {
                    this.F2Consulta(((TextBoxConsultas)Enum.Parse(typeof(TextBoxConsultas), (sender as TextBox).Name)));
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                SistemaGlobal.Sis.Log.LogError(ex, true, this);
            }
        }

        private void EnterConsulta(TextBoxConsultas textBoxConsultas)
        {
            EstruturaPesquisa result;
            switch (textBoxConsultas)
            {
                case TextBoxConsultas.txtFat_sequencial:
                    if (string.IsNullOrEmpty(provider.Faturamentos.Fat_sequencialString))
                    {
                        this.F2Consulta(textBoxConsultas);
                    }
                    else
                    {
                        result = FaturamentosPes.PegaFaturamentos(SistemaGlobal.Sis.Connection, Convert.ToInt16(provider.Faturamentos.Fat_sequencialString));
                        switch (result.Resultado)
                        {
                            case ResultadoPesquisa.ENCONTRADO:
                                provider.SetFaturamentos(result.Faturamentos.FirstOrDefault());
                                break;
                            case ResultadoPesquisa.NAO_ENCONTRADO:
                                SistemaGlobal.Sis.Msg.MostraMensagem("Faturamento não encontrado!", "Atenção", MessageBoxButton.OK, this);
                                SelecionaText(txtFat_sequencial);
                                break;
                            case ResultadoPesquisa.VARIOS_ENCONTRADO:
                                this.F2Consulta(textBoxConsultas);
                                break;
                        }
                    }
                    break;
                case TextBoxConsultas.txtFpa_descricao:
                    if (string.IsNullOrEmpty(provider.Lancamento.DescricaoFpagamento))
                    {
                        this.F2Consulta(textBoxConsultas);
                    }
                    else
                    {
                        result = FpagamentosPes.PegaFpagamentoByDescricao(SistemaGlobal.Sis.Connection, provider.Lancamento.DescricaoFpagamento);
                        switch (result.Resultado)
                        {
                            case ResultadoPesquisa.ENCONTRADO:
                                provider.SetFpagamento(result.Fpagamentos.FirstOrDefault());
                                break;
                            case ResultadoPesquisa.NAO_ENCONTRADO:
                                SistemaGlobal.Sis.Msg.MostraMensagem("Forma de pagamento não cadastrada!", "Atenção", MessageBoxButton.OK, this);
                                SelecionaText(txtFpa_descricao);
                                break;
                            case ResultadoPesquisa.VARIOS_ENCONTRADO:
                                this.F2Consulta(textBoxConsultas);
                                break;
                        }
                    }
                    break;
                case TextBoxConsultas.txtPla_numeroconta:
                    if (string.IsNullOrEmpty(provider.Lancamento.Pla_numeroconta))
                    {
                        this.F2Consulta(textBoxConsultas);
                    }
                    else
                    {
                        result = PlanoContasPes.PegaPlanoPagamento(SistemaGlobal.Sis.Connection, provider.Lancamento.Pla_numeroconta);
                        switch (result.Resultado)
                        {
                            case ResultadoPesquisa.ENCONTRADO:
                                break;
                            case ResultadoPesquisa.NAO_ENCONTRADO:
                                SistemaGlobal.Sis.Msg.MostraMensagem("Plano de Contas não cadastrado!", "Atenção", MessageBoxButton.OK, this);
                                SelecionaText(txtPla_numeroconta);
                                break;
                            case ResultadoPesquisa.VARIOS_ENCONTRADO:
                                this.F2Consulta(textBoxConsultas);
                                break;
                        }
                    }
                    break;
                case TextBoxConsultas.txtCrf_sequencial:
                    if (this.ValidaCabecalho())
                    {
                        if (string.IsNullOrEmpty(provider.Conta.Crf_sequencialString))
                        {
                            this.F2Consulta(textBoxConsultas);
                        }
                        else
                        {
                            result = CrfinanceiroPes.PegaCrfinanceiro(SistemaGlobal.Sis.Connection, Convert.ToInt16(provider.Conta.Crf_sequencialString));
                            switch (result.Resultado)
                            {
                                case ResultadoPesquisa.ENCONTRADO:
                                    provider.SetCrfinanceiro(result.Crfinanceiros.FirstOrDefault());
                                    break;
                                case ResultadoPesquisa.NAO_ENCONTRADO:
                                    SistemaGlobal.Sis.Msg.MostraMensagem("Conta não encontrada!", "Atenção", MessageBoxButton.OK, this);
                                    SelecionaText(txtCrf_sequencial);
                                    break;
                                case ResultadoPesquisa.VARIOS_ENCONTRADO:
                                    this.F2Consulta(textBoxConsultas);
                                    break;
                            }
                        }
                    }
                    break;
                case TextBoxConsultas.txtCli_nome:
                    if (string.IsNullOrEmpty(provider.Faturamentos.Cli_nomeString))
                    {
                        F2Consulta(textBoxConsultas);
                    }
                    else
                    {
                        result = ClientesPes.PegaCliente(SistemaGlobal.Sis.Connection, provider.Faturamentos.Cli_nomeString);
                        switch (result.Resultado)
                        {
                            case ResultadoPesquisa.ENCONTRADO:
                                provider.SetCliente(result.Clientes.FirstOrDefault());
                                break;
                            case ResultadoPesquisa.NAO_ENCONTRADO:
                                SistemaGlobal.Sis.Msg.MostraMensagem("Cliente não cadastrado!", "Atenção", MessageBoxButton.OK, this);
                                SelecionaText(txtCli_nome);
                                break;
                            case ResultadoPesquisa.VARIOS_ENCONTRADO:
                                F2Consulta(textBoxConsultas);
                                break;
                        }
                    }
                    break;
            }
        }

        private void F2Consulta(TextBoxConsultas textBoxConsultas)
        {
            switch (textBoxConsultas)
            {
                case TextBoxConsultas.txtFat_sequencial:
                    using (FaturamentosPes ps = new FaturamentosPes(SistemaGlobal.Sis.Connection))
                    {
                        Faturamentos faturamento = ps.Pesquisa(this);
                        if (faturamento != null)
                        {
                            provider.SetFaturamentos(faturamento);
                            SelecionaText(txtCli_nome);
                        }
                        else
                        {
                            SelecionaText(txtFat_sequencial);
                        }
                    }
                    break;
                case TextBoxConsultas.txtCli_nome:
                    using (ClientesPes ps = new ClientesPes(SistemaGlobal.Sis.Connection))
                    {
                        Clientes cliente = ps.ConsultaClientesTemFinanceiro(this, this.provider.GetFinanceiro());
                        if (cliente != null)
                        {
                            provider.SetCliente(cliente);
                            SelecionaText(txtCrf_sequencial);
                        }
                        else
                        {
                            SelecionaText(txtCli_nome);
                        }
                    }
                    break;
                case TextBoxConsultas.txtFpa_descricao:
                    using (FpagamentosPes ps = new FpagamentosPes(SistemaGlobal.Sis.Connection))
                    {
                        Fpagamentos fpagamento = ps.Pesquisa(this);
                        if (fpagamento != null)
                        {
                            provider.SetFpagamento(fpagamento);
                            SelecionaText(txtPla_numeroconta);
                        }
                        else
                        {
                            SelecionaText(txtFpa_descricao);
                        }
                    }
                    break;
                case TextBoxConsultas.txtPla_numeroconta:
                    using (PlanoContasPes ps = new PlanoContasPes(SistemaGlobal.Sis.Connection))
                    {
                        Planocontas plano = ps.Pesquisa(this);
                        if (plano != null)
                        {
                            provider.SetPlanoConta(plano);
                            SelecionaText(txtCrf_ndocumento);
                        }
                        else
                        {
                            SelecionaText(txtPla_numeroconta);
                        }
                    }
                    break;
                case TextBoxConsultas.txtCrf_sequencial:
                    if (this.ValidaCabecalho())
                    {
                        using (CrfinanceiroPes pes = new CrfinanceiroPes(TipoPesquisaFinanceiro.Contas_pagar, SistemaGlobal.Sis.Connection))
                        {
                            Crfinanceiro crfinanceiro = pes.PesquisaFaturamentoByCli_nome(this, provider.Faturamentos.Cli_nomeString, this.provider.GetFinanceiro(), this.provider.GetListCrf());
                            if (crfinanceiro != null)
                            {
                                provider.SetCrfinanceiro(crfinanceiro);
                                SelecionaText(txtValor);
                            }
                            else
                            {
                                SelecionaText(txtCrf_sequencial);
                            }
                        }
                    }
                    break;
            }
        }

        private void txtValor_PreviewKeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.LancaConta();
                e.Handled = true;
            }
        }

        private void LancaConta()
        {
            if (this.ValidaLacaConta())
            {
                this.provider.AddContas();
                this.provider.NovaConta();

                SelecionaText(txtCrf_sequencial);
            }
        }

        private bool ValidaLacaConta()
        {
            if (!this.ValidaCabecalho()) return false;
            if (!this.provider.ValidaCrf_sequencial(this)) return SelecionaText(txtCrf_sequencial);
            return true;
        }

        private void txtCrf_ndocumento_GotKeyboardFocus_1(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (provider.ListContas.Count > 0)
            {
                if (string.IsNullOrEmpty(provider.Lancamento.Crf_ndocumento))
                {
                    provider.Lancamento.Crf_ndocumento = string.Format("FAT{0}", DateTime.Now.ToString("ddMMyyyyHHmmss"));
                }
            }
        }

        private void TextBox_GotKeyboardFocus_1(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (provider.ListContas.Count > 0)
            {
                if (string.IsNullOrEmpty(provider.Lancamento.Vencimento))
                {
                    int count = provider.ListLancamentos.Count;
                    count++;
                    provider.Lancamento.Vencimento = DateTime.Now.AddMonths(count).ConvertToString();
                }
            }
        }

        private void TextBox_GotKeyboardFocus_2(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (provider.ListContas.Count > 0)
            {
                if (string.IsNullOrEmpty(provider.Lancamento.Crf_parcela))
                {
                    int count = provider.ListLancamentos.Count;
                    count++;
                    provider.Lancamento.Crf_parcela = count.ToString().PadLeft(2, '0');
                }
            }
        }

        private void TextBoxValorLacamentol_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.LancaLancamento();
                e.Handled = true;
            }
        }

        private void LancaLancamento()
        {
            if (this.ValidaLancamento())
            {
                provider.AddLancamentos();
                provider.NovoLancamento();

                SelecionaText(txtFpa_descricao);
            }
        }

        private void TextBoxDecimal_GotKeyboardFocus_1(object sender, KeyboardFocusChangedEventArgs e)
        {
            decimal value = 0;
            if (!string.IsNullOrEmpty(provider.Lancamento.ValorAReceber))
                value = Convert.ToDecimal(provider.Lancamento.ValorAReceber);
            if (value == 0)
            {
                provider.Lancamento.ValorAReceber = provider.GetValorParcela();
            }
        }

        private void SalvarFaturamento_Click(object sender, RoutedEventArgs e)
        {
            if (this.ValidaFaturamento())
            {
                this.provider.GravaFaturamentos();
            }
        }

        private bool ValidaFaturamento()
        {
            if (provider.ListContas.Count == 0)
                return false;
            if (!this.ValidaCabecalho()) return false;
            if (!provider.ValidaFaturamento(this)) return SelecionaText(txtFpa_descricao);
            if (!provider.ValidaSaldoFaturamento(this)) return false;
            if (SistemaGlobal.Sis.Msg.MostraMensagem("Deseja realmente salvar o Faturamento?", "Alerta", MessageBoxButton.YesNo, this) == MessageBoxResult.No)
                return false;
            return true;
        }

        private bool ValidaLancamento()
        {
            if (!this.ValidaCabecalho()) return false;
            if (!provider.ValidaFpagamentos(this)) return SelecionaText(txtFpa_descricao);
            if (!provider.ValidaPla_numeroconta(this)) return SelecionaText(txtPla_numeroconta);
            if (!provider.ValidaValorLancamento(this)) return SelecionaText(txtValorParcela);
            return true;
        }

        private bool ValidaCabecalho()
        {
            if (!provider.ValidaCliente(this)) return SelecionaText(txtCli_nome);
            return true;
        }

        private void NovoFaturamento_Click(object sender, RoutedEventArgs e)
        {
            this.NovoFaturamento();
        }

        private void txtFat_sequencial_LostKeyboardFocus_1(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.provider.Faturamentos.Fat_sequencialString))
            {
                if (this.provider.Faturamentos.Fat_sequencial > 0)
                    this.NovoFaturamento();
            }
        }

        private void ExcluirFaturamento_Click(object sender, RoutedEventArgs e)
        {
            if (this.ValidaExclusaoFaturamento())
            {
            }
        }

        private bool ValidaExclusaoFaturamento()
        {
            if (string.IsNullOrEmpty(this.provider.Faturamentos.Fat_sequencialString))
                return false;
            if (!provider.ValidaExclusaoFat_sequencial(this)) return SelecionaText(txtFat_sequencial);
            return true;
        }
    }
}
