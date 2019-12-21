using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SIF
{
    public class Item : INotifyPropertyChanged
    {
        public string Pro_codigo { get; set; }
        public string Pro_descricao { get; set; }
        public string Pro_unidade { get; set; }

        private string _Ite_nritem;
        /// <summary>
        /// Nº do item 
        /// </summary>
        public string Ite_nritem
        {
            get { return _Ite_nritem; }
            set
            {
                if (_Ite_nritem != value)
                {
                    _Ite_nritem = value;
                    NotifyPropertyChanged("Ite_nritem");
                }
            }
        }

        public decimal Ite_quantidade { get; set; }
        public decimal Ite_vlunitario { get; set; }

        public decimal Total
        {
            get
            {
                return this.Ite_quantidade * this.Ite_vlunitario;
            }
        }

        public Brush ItemBrush
        {
            get
            {
                //try
                //{
                //    int item = Convert.ToInt16(this._Ite_nritem);
                //    if (item % 2 == 1)
                //        return (Brush)new BrushConverter().ConvertFromString("#FFDCDCDC");
                //}
                //catch
                //{
                //}
                //return (Brush)new BrushConverter().ConvertFromString("White");

                int NumeroItem = Convert.ToInt32(this._Ite_nritem);
                //if (cancela)
                //{
                //    if (NumeroItem % 2 == 1)
                //    {
                //        return new SolidColorBrush((Color)new ColorConverter().ConvertFromInvariantString("#FFF4F4"));
                //    }
                //    else
                //    {
                //        return new SolidColorBrush((Color)new ColorConverter().ConvertFromInvariantString("#FFEFEF"));
                //    }
                //}
                //else
                //{
                if (NumeroItem % 2 == 1)
                {
                    return Brushes.White;
                }
                else
                {
                    return new SolidColorBrush((Color)new ColorConverter().ConvertFromInvariantString("#EEF6FF"));
                }
                //}
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
