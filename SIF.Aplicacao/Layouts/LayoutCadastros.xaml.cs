using SIF.WPF.Styles.Windows.Controls;
using SIF.Dao;
using SIF.Aplicacao.LayoutCadastroW;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using SIF.Aplicacao.FrenteCaixa;

namespace SIF.Aplicacao.Layouts
{
    /// <summary>
    /// Interaction logic for LayoutCadastros.xaml
    /// </summary>
    public partial class LayoutCadastros : UserControl
    {
        public LayoutCadastros()
        {
            InitializeComponent();
        }

        private void Clientes_Click(object sender, MouseButtonEventArgs e)
        {
            //SistemaGlobal.Sis.ControleJanelas.ShowWindow(new CadastroClientes(Window.GetWindow(this), SistemaGlobal.Sis.Connection));
            ManagerWindowState.AddWindowManager(new CadastroClientes(Window.GetWindow(this), SistemaGlobal.Sis.Connection));
        }

        private void Produtos_Click(object sender, MouseButtonEventArgs e)
        {
            //SistemaGlobal.Sis.ControleJanelas.ShowWindow(new CadastroProdutos(Window.GetWindow(this), SistemaGlobal.Sis.Connection));
            ManagerWindowState.AddWindowManager(new CadastroProdutos(Window.GetWindow(this), SistemaGlobal.Sis.Connection));
        }

        private void FormasPgto_Click(object sender, MouseButtonEventArgs e)
        {
            //SistemaGlobal.Sis.ControleJanelas.ShowWindow(new CadastroFormaPgto(Window.GetWindow(this), SistemaGlobal.Sis.Connection));
            ManagerWindowState.AddWindowManager(new CadastroFormaPgto(Window.GetWindow(this), SistemaGlobal.Sis.Connection));
        }

        private void PlanoContas_Click(object sender, MouseButtonEventArgs e)
        {
            //SistemaGlobal.Sis.ControleJanelas.ShowWindow(new CadastroPlanoContas(Window.GetWindow(this), SistemaGlobal.Sis.Connection));
            CadastroPlanoContas c = new CadastroPlanoContas(Window.GetWindow(this), SistemaGlobal.Sis.Connection);
            ManagerWindowState.AddWindowManager(c);
        }

        private void Caixas_Click(object sender, MouseButtonEventArgs e)
        {
            //SistemaGlobal.Sis.ControleJanelas.ShowWindow(new CadastroCaixas(Window.GetWindow(this), SistemaGlobal.Sis.Connection));
            CadastroCaixas c = new CadastroCaixas(Window.GetWindow(this), SistemaGlobal.Sis.Connection);
            ManagerWindowState.AddWindowManager(c);
        }

        private void ImageIcon_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            string command = @"SELECT Cli_codigo
                                                                          ,Cli_nome
                                                                          ,Cli_nomerazao
                                                                          ,Cli_endereco
                                                                          ,Cli_numero
                                                                          ,Cli_bairro
                                                                          ,Cli_cidade
                                                                          ,Cli_uf
                                                                          ,Cli_complemento
                                                                          ,Cli_cpfcnpj
                                                                          ,Cli_celular
                                                                          ,Cli_observacao
                                                                      FROM dbo.Clientes";
            //SIF.PesquisaView.PesquisaWindow p = new SIF.PesquisaView.PesquisaWindow(Window.GetWindow(this), command, Sistema.Sis.Connection.GetSqlConnection());
            //p.ShowDialog();
            //SIF.Pesquisa.PesquisaWindow pes = new SIF.Pesquisa.PesquisaWindow(Window.GetWindow(this), command, Sistema.Sis.XmlConnectionDb.ConnectionString);
            //pes.ShowDialog();

            SIF.PesquisaViewSP.PesquisaWindow p = new PesquisaViewSP.PesquisaWindow(Window.GetWindow(this), command, SistemaGlobal.Sis.Connection.GetSqlConnection());
            p.ShowDialog();
        }

        private void CupomFiscal_Click(object sender, MouseButtonEventArgs e)
        {
            CupomFiscal c = new CupomFiscal(Window.GetWindow(this));
            c.ShowDialog();
        }
    }
}
