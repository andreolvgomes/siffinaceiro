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

namespace SIF.Aplicacao
{
    /// <summary>
    /// Interaction logic for HelpJanela.xaml
    /// </summary>
    public partial class HelpJanela
    {
        public HelpJanela(Window owner)
        {
            InitializeComponent();

            this.Owner = owner;

            this.Loaded += new RoutedEventHandler(W_Loaded);
        }

        private void W_Loaded(object sender, RoutedEventArgs e)
        {
            richTextBox.Document.Blocks.Add(new Paragraph(new Run("INFORMAÇÕES DE COMO UTILZIAR ESTA TELA NO SISTEMA")) { FontWeight = FontWeights.Bold, Foreground =(Brush) new BrushConverter().ConvertFromString("Red")});
        }
    }
}
