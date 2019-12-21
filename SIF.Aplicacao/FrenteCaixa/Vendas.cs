using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIF
{
    public class Vendas : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private ObservableCollection<Item> _Itens;
        /// <summary>
        /// Lista de Itens
        /// </summary>
        public ObservableCollection<Item> Itens
        {
            get { return _Itens; }
            set
            {
                if (_Itens != value)
                {
                    _Itens = value;
                }
            }
        }

        public string ItensNr
        {
            get
            {
                if (this.Itens == null)
                    return "000";
                if (this.Itens.Count == 0)
                    return "000";
                return this.Itens.Count.ToString().PadLeft(3, '0');
            }
        }

        public Vendas()
        {
            this.Itens = new ObservableCollection<Item>();
            this.Itens.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(ItensCollectionChanged);
        }

        private void ItensCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged("ItensNr");
        }
    }
}
