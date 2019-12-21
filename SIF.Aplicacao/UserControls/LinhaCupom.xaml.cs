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

namespace SIF.Aplicacao.UserControls
{
    /// <summary>
    /// Interaction logic for LinhaCupom.xaml
    /// </summary>
    public partial class LinhaCupom : UserControl
    {


        public string Linha
        {
            get { return (string)GetValue(LinhaProperty); }
            set { SetValue(LinhaProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Linha.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LinhaProperty =
            DependencyProperty.Register("Linha", typeof(string), typeof(LinhaCupom), new PropertyMetadata(new String('-', 300)));

        
        public LinhaCupom()
        {
            InitializeComponent();

            this.DataContext = this;
        }
    }
}
