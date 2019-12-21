using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SIF.Pesquisa.DataGrid
{
    /// <summary>
    /// Interaction logic for TextBoxFilter.xaml
    /// </summary>
    public partial class TextBoxFilter : UserControl, INotifyPropertyChanged
    {
        private string _FilterText;

        public string FilterText
        {
            get { return _FilterText; }
            set 
            {
                if (_FilterText != value)
                {
                    _FilterText = value;
                }
            }
        }

        public bool TemFilter
        {
            get
            {
                return (!string.IsNullOrEmpty(FilterText) && FilterText.Length > 0);
            }
        }

        public int IndexColuna { get; set; }
        public OptionColumnInfo FilterColumnInfo { get; set; }
        public DataGridPesquisa Grid { get; set; }

        public TextBoxFilter()
        {
            InitializeComponent();

            this.DataContext = this;

            this.Loaded += new RoutedEventHandler(TextBox_Loaded);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void TextBox_Loaded(object sender, RoutedEventArgs e)
        {
            DataGridColumn column = null;
            DataGridColumnHeader colHeader = null;

            UIElement parent = (UIElement)VisualTreeHelper.GetParent(this);
            while (parent != null)
            {
                parent = (UIElement)VisualTreeHelper.GetParent(parent);
                if (colHeader == null)
                    colHeader = parent as DataGridColumnHeader;

                if (Grid == null)
                    Grid = parent as DataGridPesquisa;

                if (colHeader != null && Grid != null)
                    break;
            }

            if (colHeader != null)
                column = colHeader.Column;

            if (column != null)
            {
                FilterColumnInfo = new OptionColumnInfo(column, Grid.TypeFields);
                Grid.RegisterTextBoxFilter(this);
                this.IndexColuna = Grid.CountColumns;
                Grid.CountColumns++;
            }
        }

        private void TextFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            OnPropertyChanged("FilterChanged");
        }        
    }
}
