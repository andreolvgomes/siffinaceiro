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
    /// Interaction logic for Comprovantes.xaml
    /// </summary>
    public partial class Comprovantes
    {
        public Comprovantes(Window owner)
        {
            InitializeComponent();

            using (EffectWindow f = new EffectWindow())
            {
                f.SetEffectBackgound(owner, this);
            }
        }
    }
}
