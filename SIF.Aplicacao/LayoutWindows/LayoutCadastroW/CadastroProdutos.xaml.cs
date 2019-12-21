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

namespace SIF.Aplicacao.LayoutCadastroW
{
    /// <summary>
    /// Interaction logic for CadastroProdutos.xaml
    /// </summary>
    public partial class CadastroProdutos : ModernWindow
    {
        public CadastroProdutos(Window owner, ConnectionDb conexao)
        {
            InitializeComponent();

            using (EffectWindow e = new EffectWindow())
            {
                e.SetEffectBackgound(owner, this);
            }
        }
    }
}
