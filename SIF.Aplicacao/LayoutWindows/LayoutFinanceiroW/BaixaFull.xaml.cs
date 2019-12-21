using SIF.Dao;
using SIF.Aplicacao.Providers;
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
using System.Windows.Threading;

namespace SIF.Aplicacao.LayoutFinanceiroW
{
    /// <summary>
    /// Interaction logic for BaixaFull.xaml
    /// </summary>
    public partial class BaixaFull
    {
        private ProviderBaixaColetiva provider;
        private ConnectionDb conexao;

        public BaixaFull(Window owner, ConnectionDb conexao)
        {
            InitializeComponent();

            this.conexao = conexao;

            provider = new ProviderBaixaColetiva(conexao);
            provider.Event_AtualizaCollection += new Action(AtualizaCollection);
            this.DataContext = provider;

            using (EffectWindow e = new EffectWindow())
            {
                e.SetEffectBackgound(owner, this);
            }

            this.AtualizaDataGrid();
        }

        private void AtualizaCollection()
        {
            if (PodeAtualizar())
                AtualizaDataGrid();
        }

        private void AtualizaDataGrid()
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Background, (System.Threading.ThreadStart)delegate
            {
                provider.StartCollection();

                this.Dispatcher.BeginInvoke(DispatcherPriority.Background, (System.Threading.ThreadStart)delegate
                {
                    provider.ProcessaResumoValores(provider.Contas);
                });
            });
        }

        private bool PodeAtualizar()
        {
            if (provider.EspecificarCliente)
            {
                if (!string.IsNullOrEmpty(provider.NomeCliente))
                {
                    if (string.IsNullOrEmpty(SistemaGlobal.Sis.Connection.GetValue<string>(string.Format("SELECT Cli_nome FROM Clientes WHERE Cli_nome = '{0}'", provider.NomeCliente))))
                    {
                        SistemaGlobal.Sis.Msg.MostraMensagem("Cliente não cadastrado", "Atenção", MessageBoxButton.OK, this);
                        return Seleciona(txtCliente);
                    }
                }
            }
            return true;
        }

        private void Baixar_Click(object sender, RoutedEventArgs e)
        {
            ExecBaixa();
        }

        private void ExecBaixa()
        {
            if (dgvContas.SelectedItem == null)
            {
                SistemaGlobal.Sis.Msg.MostraMensagem("Selecione uma conta!", "Atenção", MessageBoxButton.OK, this);
            }
            else
            {
                BaixaContas baixa = new BaixaContas(this, conexao, provider.Financeiro);
                DataRowView row = dgvContas.SelectedItem as DataRowView;

                if (baixa.ExecutaBaixa(row.Get<int>("Crf_sequencial")))
                    provider.AtualizaCollection();
            }
        }

        private void DataGrid_PreviewKeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                ExecBaixa();
                e.Handled = true;
            }
        }

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {
            Seleciona(txtCliente);
        }

        private bool Seleciona(TextBox textbox)
        {
            textbox.Focus();
            textbox.SelectAll();

            return false;
        }

        private void txtCliente_PreviewKeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TrataEnterPes();
            }
            else if (e.Key == Key.F2)
            {
                TrataF2Pes();
            }
        }

        private void TrataF2Pes()
        {
            using (ClientesPes pes = new ClientesPes(conexao))
            {
                Clientes cliente = pes.Pesquisa(this);
                if (cliente != null)
                {
                    provider.SetCliente(cliente);
                    AtualizaCollection();
                }
                else
                {
                    Seleciona(txtCliente);
                }
            }
        }

        private void TrataEnterPes()
        {
            if (string.IsNullOrEmpty(provider.NomeCliente))
            {
                TrataF2Pes();
            }
            else
            {
                EstruturaPesquisa result = ClientesPes.PegaCliente(conexao, provider.NomeCliente);
                switch (result.Resultado)
                {
                    case ResultadoPesquisa.ENCONTRADO:
                        provider.SetCliente(result.Clientes.FirstOrDefault());
                        AtualizaCollection();
                        break;
                    case ResultadoPesquisa.NAO_ENCONTRADO:
                        SistemaGlobal.Sis.Msg.MostraMensagem("Cliente não cadastrado", "Atenção", MessageBoxButton.OK, this);
                        Seleciona(txtCliente);
                        break;
                    case ResultadoPesquisa.VARIOS_ENCONTRADO:
                        break;
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            AtualizaCollection();
        }

        private void Editar_Click(object sender, RoutedEventArgs e)
        {
            if (dgvContas.Items.Count > 0)
            {
                if (dgvContas.SelectedItem == null)
                {
                    SistemaGlobal.Sis.Msg.MostraMensagem("Selecione uma conta da lista!", "Atenção", this);
                }
                else
                {
                    Contasrp ct = new Contasrp(this, conexao, provider.Financeiro, true);
                    DataRowView row = dgvContas.SelectedItem as DataRowView;

                    ct.VisualizaConta(row.Get<int>("Crf_sequencial"));
                    provider.AtualizaCollection();
                }
            }
        }

        private void CheckBoxStatusPagamento_Checked(object sender, RoutedEventArgs e)
        {            
            provider.SetSaveStatusCrfinanceiro(dgvContas.SelectedItem as DataRowView, true);
        }

        private void CheckBoxStatusPagamento_Unchecked(object sender, RoutedEventArgs e)
        {
            provider.SetSaveStatusCrfinanceiro(dgvContas.SelectedItem as DataRowView, false);
        }
    }
}
