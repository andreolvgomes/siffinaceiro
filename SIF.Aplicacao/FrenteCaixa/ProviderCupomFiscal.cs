using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SIF.Aplicacao.Providers
{
    public class ProviderCupomFiscal : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public CornerRadius CornerRadius
        {
            get
            {
                return new CornerRadius(5);
            }
        }

        private Vendas _Venda;
        /// <summary>
        /// Venda
        /// </summary>
        public Vendas Venda
        {
            get { return _Venda; }
            set
            {
                if (_Venda != value)
                {
                    _Venda = value;
                    NotifyPropertyChanged("Venda");
                }
            }
        }

        public ImageSource Image
        {
            get
            {
                if (this.Venda != null && this.Venda.Itens.Count > 0)
                {
                    if (this.Venda.Itens.Count % 2 == 1)
                        return new BitmapImage(new Uri("/Images/produto1.jpg", UriKind.Relative));
                    return new BitmapImage(new Uri("/Images/produto2.jpg", UriKind.Relative));
                }
                return null;
            }
        }

        public ProviderCupomFiscal()
        {
        }

        internal void NovaVenda()
        {
            this.Venda = new Vendas();
        }

        internal void GravaItem()
        {
            int i = this.Venda.Itens.Count + 1;
            Item item = new Item();

            item.Pro_codigo = i.ToString().PadLeft(14, '0');
            item.Pro_descricao = "ITEM DE TESTE, LANÇAMENTOS DE ITENS";
            item.Pro_unidade = "PC";

            item.Ite_nritem = i.ToString().PadLeft(3, '0');
            item.Ite_quantidade = 4;
            item.Ite_vlunitario = 900.00M;
            this.Venda.Itens.Add(item);

            NotifyPropertyChanged("Image");
        }
    }
}
