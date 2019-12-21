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

namespace SIF.Aplicacao.ConfiguracaoInstanciaSQL.UserControlPassos
{
    /// <summary>
    /// Interaction logic for PassoConfiguracaoArquivoBanco.xaml
    /// </summary>
    public partial class PassoConfiguracaoArquivoBanco : UserControl
    {
        private ContextInicial context = null;

        public PassoConfiguracaoArquivoBanco(ContextInicial context)
        {
            InitializeComponent();

            this.context = context;
            this.DataContext = context;
        }

        internal bool Validacao(Window owner)
        {
            return true;
        }

        private void MoverArquivo_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog op = new System.Windows.Forms.FolderBrowserDialog();
            if (op.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                context.CaminhoMdf = System.IO.Path.Combine(op.SelectedPath, "BDSIF.mdf");
            }
        }

        private void BuscarArquivo_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog op = new System.Windows.Forms.FolderBrowserDialog();
            if (op.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                context.CaminhoArquivoExistente = System.IO.Path.Combine(op.SelectedPath, "BDSIF.mdf");
            }
        }
    }
}
