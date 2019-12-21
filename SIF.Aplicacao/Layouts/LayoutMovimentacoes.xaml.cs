using SIF.Aplicacao.LayoutMovimentacao;
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
    /// Interaction logic for LayoutMovimentacoes.xaml
    /// </summary>
    public partial class LayoutMovimentacoes : UserControl
    {
        public LayoutMovimentacoes()
        {
            InitializeComponent();
        }

        private void ImageIcon_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            SistemaGlobal.Sis.ControleJanelas.ShowWindow(new MovimentacaoFinan(Window.GetWindow(this), SistemaGlobal.Sis.Connection));
        }
    }
}
