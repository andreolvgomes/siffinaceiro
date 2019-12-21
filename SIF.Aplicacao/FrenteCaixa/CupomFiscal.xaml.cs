using SIF.Aplicacao.Providers;
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

namespace SIF.Aplicacao.FrenteCaixa
{
    /// <summary>
    /// Interaction logic for CupomFiscal.xaml
    /// </summary>
    public partial class CupomFiscal// : Window
    {
        private ProviderCupomFiscal provider = null;

        public CupomFiscal(Window owner)
        {
            InitializeComponent();

            this.provider = new ProviderCupomFiscal();
            this.provider.NovaVenda();

            this.DataContext = this.provider;

            using (EffectWindow e = new EffectWindow())
            {
                e.SetEffectBackgound(owner, this);
            }
        }

        private IEnumerable<Item> GetItens()
        {
            List<Item> ls = new List<Item>();
            for (int i = 0; i < 10; i++)
            {
                ls.Add(new Item() { Ite_nritem = i.ToString().PadLeft(3, '0'), Pro_codigo = i.ToString().PadLeft(14, '0'), Pro_descricao = "COMPUTADOR POSITIVO 500 GM", Ite_quantidade = i, Ite_vlunitario = i * 10, Pro_unidade = "UN" });
            }
            return ls;
        }

        private void txtPro_codigoTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.provider.GravaItem();                
            }
        }
    }
}
