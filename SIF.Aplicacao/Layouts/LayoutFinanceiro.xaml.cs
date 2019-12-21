using SIF.Dao;
using SIF.Aplicacao.LayoutFinanceiroW;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SIF.Aplicacao.Layouts
{
    /// <summary>
    /// Interaction logic for LayoutFinanceiro.xaml
    /// </summary>
    public partial class LayoutFinanceiro : UserControl
    {
        public LayoutFinanceiro()
        {
            InitializeComponent();
        }

        private void ImageIcon_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            SistemaGlobal.Sis.ControleJanelas.ShowWindow(new GeradorParcela(Window.GetWindow(this), SistemaGlobal.Sis.Connection));
            //WindowViewState.AddWindowManager(new GeradorParcela(Window.GetWindow(this), SistemaGlobal.Sis.Connection));
        }

        private void ContasReceber_Click(object sender, MouseButtonEventArgs e)
        {
            SistemaGlobal.Sis.ControleJanelas.ShowWindow(new Contasrp(Window.GetWindow(this), SistemaGlobal.Sis.Connection, ContasReceberPagar.ContasReceber));
            //WindowViewState.AddWindowManager(new Contasrp(Window.GetWindow(this), SistemaGlobal.Sis.Connection, ContasReceberPagar.ContasReceber));
        }

        private void ContasPagar_Click(object sender, MouseButtonEventArgs e)
        {
            SistemaGlobal.Sis.ControleJanelas.ShowWindow(new Contasrp(Window.GetWindow(this), SistemaGlobal.Sis.Connection, ContasReceberPagar.ContasPagar));
            //WindowViewState.AddWindowManager(new Contasrp(Window.GetWindow(this), SistemaGlobal.Sis.Connection, ContasReceberPagar.ContasPagar));
        }

        private void BaixasContasPagar_Click(object sender, MouseButtonEventArgs e)
        {
            SistemaGlobal.Sis.ControleJanelas.ShowWindow(new BaixaContas(Window.GetWindow(this), SistemaGlobal.Sis.Connection, ContasReceberPagar.ContasPagar));
            //WindowViewState.AddWindowManager(new BaixaContas(Window.GetWindow(this), SistemaGlobal.Sis.Connection, ContasReceberPagar.ContasPagar));
        }

        private void BaixasContasReceber_Click(object sender, MouseButtonEventArgs e)
        {
            SistemaGlobal.Sis.ControleJanelas.ShowWindow(new BaixaContas(Window.GetWindow(this), SistemaGlobal.Sis.Connection, ContasReceberPagar.ContasReceber));
            //WindowViewState.AddWindowManager(new BaixaContas(Window.GetWindow(this), SistemaGlobal.Sis.Connection, ContasReceberPagar.ContasReceber));
        }

        private void BaixaColetiva_Click(object sender, MouseButtonEventArgs e)
        {
            SistemaGlobal.Sis.ControleJanelas.ShowWindow(new BaixaFull(Window.GetWindow(this), SistemaGlobal.Sis.Connection));
        }

        private void Faturamento_Click(object sender, MouseButtonEventArgs e)
        {
            SistemaGlobal.Sis.ControleJanelas.ShowWindow(new Faturamento(Window.GetWindow(this), SistemaGlobal.Sis.Connection));
            //WindowViewState.AddWindowManager(new Faturamento(Window.GetWindow(this), SistemaGlobal.Sis.Connection));
        }
    }
}
