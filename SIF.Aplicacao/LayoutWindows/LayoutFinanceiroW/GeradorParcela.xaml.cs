using SIF.WPF.Styles.Windows.Controls;
using SIF.Dao;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for GeradorParcelas.xaml
    /// </summary>
    public partial class GeradorParcela : ModernWindow
    {
        private ProviderGeradorParcelas provider;

        public GeradorParcela(Window owner, ConnectionDb conexao)
        {
            InitializeComponent();

            provider = new ProviderGeradorParcelas(conexao);
            this.DataContext = provider;

            using (EffectWindow f = new EffectWindow())
            {
                f.SetEffectBackgound(owner, this);
            }

            SelecionaText(txtCliente);

            this.PreviewKeyDown += new KeyEventHandler(W_PreviewKeyDown);
        }

        private void W_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Up)
            {
                UIElement ui = e.OriginalSource as UIElement;
                if (e.Key == Key.Enter)
                    ui.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                else
                    ui.MoveFocus(new TraversalRequest(FocusNavigationDirection.Previous));
            }
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F2)
            {
                PesquisaF2(provider.GetTextBoxPesquisa(sender as TextBox));
            }
            else if(e.Key == Key.Enter)
            {
                PesquisaEnter(provider.GetTextBoxPesquisa(sender as TextBox));
            }
        }

        private void PesquisaEnter(TextBoxPesquisa textBoxPesquisa)
        {
            switch (textBoxPesquisa)
            {
                case TextBoxPesquisa.Clientes:
                    if (string.IsNullOrEmpty(provider.Cliente.Cli_nome))
                    {
                        PesquisaF2(textBoxPesquisa);
                    }
                    else
                    {
                    }
                    break;
                case TextBoxPesquisa.FormasPagamento:
                    if (string.IsNullOrEmpty(provider.Fpagamento.Fpa_descricao))
                    {
                        PesquisaF2(textBoxPesquisa);
                    }
                    else
                    {
                    }
                    break;
                case TextBoxPesquisa.PlanosContas:
                    if (string.IsNullOrEmpty(provider.PlanoConta.Pla_numeroconta))
                    {
                        PesquisaF2(textBoxPesquisa);
                    }
                    else
                    {
                    }
                    break;
            }
        }

        private void PesquisaF2(TextBoxPesquisa textboxpes)
        {
            switch (textboxpes)
            {
                case TextBoxPesquisa.Clientes:
                    using (ClientesPes pes = new ClientesPes(provider.conexao))
                    {
                        Clientes cliente = pes.Pesquisa(this);
                        if (cliente != null)
                        {
                            provider.SetCliente(cliente);
                        }
                        else
                        {
                        }
                    }
                    break;
                case TextBoxPesquisa.FormasPagamento:
                    using (FpagamentosPes pes = new FpagamentosPes(provider.conexao))
                    {
                        Fpagamentos fpagamento = pes.Pesquisa(this);
                        if (fpagamento != null)
                        {
                            provider.SetFpagamento(fpagamento);
                        }
                        else
                        {
                        }
                    }
                    break;
                case TextBoxPesquisa.PlanosContas:
                    using (PlanoContasPes pes = new PlanoContasPes(provider.conexao))
                    {
                        Planocontas plano = pes.Pesquisa(this);
                        if (plano != null)
                        {
                            provider.SetPlanoConta(plano);
                        }
                    }
                    break;
            }            
        }

        private void Visualizar_Click(object sender, RoutedEventArgs e)
        {
            if (ExecutaValidacoesVisualizar())
            {
                provider.GeraVisualizacao();
            }
        }

        private void GerarParcelas_Click(object sender, RoutedEventArgs e)
        {
            if (ExecutaValidacoes())
            {
                if (provider.GravaParcelas())
                {                    
                    SistemaGlobal.Sis.Msg.MostraMensagem("Parcelas gerada com sucesso!", "Atenção", MessageBoxButton.OK, this);
                    provider.CriaNovo();
                    SelecionaText(txtCliente);
                }
            }
        }

        private bool ExecutaValidacoesVisualizar()
        {
            if (!provider.ValidaValor(this)) return SelecionaText(txtValor);
            if (!provider.ValidaQtParcela(this)) return SelecionaText(txtQtParcelas);
            if (!provider.ValidaVencimento(this)) return SelecionaText(txtVencimento);

            return true;
        }

        private bool ExecutaValidacoes()
        {
            if (!provider.ValidaCliente(this)) return SelecionaText(txtCliente);
            if (!provider.ValidaNdocumento(this)) return SelecionaText(txtNdocumento);
            if (!provider.ValidaFpagamento(this)) return SelecionaText(txtFpagamento);
            if (!provider.ValidaFinanceiro(this)) { cmbFinanceiro.Focus(); return false; }
            if (!provider.ValidaValor(this)) return SelecionaText(txtValor);
            if (!provider.ValidaQtParcela(this)) return SelecionaText(txtQtParcelas);
            if (!provider.ValidaVencimento(this)) return SelecionaText(txtVencimento);
            if (!provider.ValidaPlanoPagamento(this)) return SelecionaText(txtPlanoContas);

            return true;
        }
    }
}
