using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace SIF.PesquisaView.DataGridControl
{
    /// <summary>
    /// Interaction logic for TextBoxFilter.xaml
    /// </summary>
    public partial class TextBoxFilter : UserControl
    {
        public override string ToString()
        {
            if (this.FilterColumnInfo != null)
                return this.FilterColumnInfo.PropertyPath;

            return "TextBoxFilter IS NULL";
        }
        private string _TextFilter;
        /// <summary>
        /// Property Binding com TextFilter
        /// </summary>
        public string TextFilter
        {
            get { return _TextFilter; }
            set
            {
                if (_TextFilter != value)
                {
                    _TextFilter = value;
                    this._dataGridView.RegisterFilterWhere(this, value);
                }
            }
        }

        /// <summary>
        /// Se tem Caracter no TextFilter
        /// </summary>
        public bool HasFilter
        {
            get
            {
                if (string.IsNullOrEmpty(this._TextFilter))
                    return false;
                return this._TextFilter.Length > 0;
            }
        }

        public OptionColumnInfo FilterColumnInfo { get; private set; }
        public int IndexTextFilter { get; private set; }
        private DataGridView _dataGridView = null;
        private Thread sync = null;

        public TextBoxFilter()
        {
            InitializeComponent();

            this.DataContext = this;

            this.Loaded += new RoutedEventHandler(TextFilter_Loaded);
        }

        private void TextFilter_Loaded(object sender, RoutedEventArgs e)
        {
            DataGridColumn column = null;
            DataGridColumnHeader colHeader = null;

            UIElement parent = (UIElement)VisualTreeHelper.GetParent(this);
            while (parent != null)
            {
                parent = (UIElement)VisualTreeHelper.GetParent(parent);
                if (colHeader == null)
                    colHeader = parent as DataGridColumnHeader;

                if (_dataGridView == null)
                    _dataGridView = parent as DataGridView;

                if (colHeader != null && _dataGridView != null)
                    break;
            }

            if (colHeader != null)
                column = colHeader.Column;

            if (column != null)
            {
                FilterColumnInfo = new OptionColumnInfo(column, _dataGridView.TypeFields);
                _dataGridView.RegisterTextBoxFilter(this);
                this.IndexTextFilter = _dataGridView.CountColumns;
                _dataGridView.CountColumns++;
            }
        }

        private void TextFilterDelay_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sync != null)
                if (sync.IsAlive)
                    sync.Abort();
            sync = new Thread(new ThreadStart(this.ExecutaFiltro));
            sync.Start();

            //this.ExecutaFiltro();
        }

        private void ExecutaFiltro()
        {
            this._dataGridView.ExecutaFiltro();
        }
    }
}
