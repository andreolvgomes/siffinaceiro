using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace SIF.Pesquisa.DataGrid
{
    /// <summary>
    /// Interaction logic for DataGridPesquisa.xaml
    /// </summary>
    public partial class DataGridPesquisa : System.Windows.Controls.DataGrid
    {
        public List<TextBoxFilter> ListFilters { get; set; }
        public List<CamposTipo> TypeFields { get; set; }
        private PesquisaExecucaoSql Pesquisa { get; set; }
        public ExpressaoSqlCommand ExpressoesSql { get; set; }
        public SqlConnection _SqlConnection { get; private set; }
        private bool _disposeConnection = false;
        public int CountColumns { get; set; }
        private Dictionary<TextBoxFilter, string> FiltersWhere;

        private Window _window;

        private PropertyChangedEventHandler EventINotifyPropertyChanged;
        private KeyEventHandler EventPreviewKeyDownTextBoxFilter;

        public DataGridPesquisa()
        {
            InitializeComponent();

            this.ListFilters = new List<TextBoxFilter>();
            this.TypeFields = new List<CamposTipo>();
            this.FiltersWhere = new Dictionary<TextBoxFilter, string>();

            this.EventINotifyPropertyChanged = new PropertyChangedEventHandler(Filter_PropertyChanged);
            this.EventPreviewKeyDownTextBoxFilter = new KeyEventHandler(PreviewKeyDownTextBoxFilter);

            this.Loaded += new RoutedEventHandler(G_Loaded);
            this.PreviewKeyDown += new KeyEventHandler(G_PreviewKeyDown);
        }

        private void G_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up && this.SelectedIndex == 0)
            {
                if (e.OriginalSource is DataGridCell)
                {
                    TextBoxFilter filter = ListFilters.FirstOrDefault(c => c.IndexColuna == this.CurrentCell.Column.DisplayIndex);
                    if (filter != null) filter.txtFilter.Focus();
                }
            }
        }

        private void PreviewKeyDownTextBoxFilter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left || e.Key == Key.Right)
            {
                TextBoxFilter filter = sender as TextBoxFilter;
                UIElement element = e.OriginalSource as UIElement;
                if (e.Key == Key.Left)
                {
                    if (filter.IndexColuna > 0)
                    {
                        this.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => { MoveScrollViewerLine(filter.IndexColuna - 1); }));
                        element.MoveFocus(new TraversalRequest(FocusNavigationDirection.Previous));
                    }
                }
                else if (e.Key == Key.Right)
                {
                    if (filter.IndexColuna < (CountColumns - 1))
                    {
                        this.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => { MoveScrollViewerLine(filter.IndexColuna + 1); }));
                        element.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    }
                }
            }
            else if ((e.Key == Key.Enter || e.Key == Key.Down) && this.Items.Count > 0)
            {
                this.Focus();
                this.SelectedIndex = 0;

                DataGridRow row = (DataGridRow)this.ItemContainerGenerator.ContainerFromIndex(0);

                if (row != null)
                {
                    row.Focus();
                    row.IsSelected = true;
                    row.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }

                using (GetDataGridCell get = new GetDataGridCell())
                {
                    DataGridCell cell = get.GetCell(this, 0, (sender as TextBoxFilter).IndexColuna);
                    if (cell != null)
                        cell.Focus();
                }

                e.Handled = true;
            }
        }

        private void MoveScrollViewerLine(int index)
        {
            if (this.Items.Count > 0)
            {
                object row = this.Items[0]; //pega a primeira linha
                DataGridColumn col = this.Columns[index]; //pega coluna de acordo com o index
                this.ScrollIntoView(row, col); //set coluna "move"
            }
        }

        private void G_Loaded(object sender, RoutedEventArgs e)
        {
            _window = Window.GetWindow(this);
            if (_window != null)
            {
                /// vamos programar o Closing para desconectar do banco, se necessário, pois tem opção de iniciar o datagrid e passar um SqlConnection
                /// 
                _window.Closing += new CancelEventHandler(W_Closing);
            }
        }

        private void W_Closing(object sender, CancelEventArgs e)
        {
            if (_disposeConnection)
            {
                if (_SqlConnection != null)
                {
                    if (_SqlConnection.State != System.Data.ConnectionState.Closed)
                    {
                        _SqlConnection.Close();
                    }
                    _SqlConnection.Dispose();
                    _SqlConnection = null;
                }
            }
        }

        private void Filter_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "FilterChanged")
            {
                PesquisaFilter();
            }
        }

        private void PesquisaFilter()
        {
            if (this.Dispatcher.Thread != System.Threading.Thread.CurrentThread)
            {
                this.Dispatcher.BeginInvoke(DispatcherPriority.Send, (System.Threading.SendOrPostCallback)delegate { PesquisaFilter(); }, null);
            }
            else
            {
                bool execute = false;
                foreach (TextBoxFilter filter in ListFilters)
                {
                    if (filter.TemFilter)
                    {
                        AddFilterWhere(filter);
                        execute = true;
                    }
                    else
                    {
                        if (RemoveFilterWhere(filter)) execute = true;
                    }
                }
                if (execute)
                    SetItemsSource(Pesquisa.Pesquisa(ExpressoesSql.Select, FiltersWhere));
            }
        }

        private bool RemoveFilterWhere(TextBoxFilter filter)
        {
            bool result = ((from x in FiltersWhere where x.Key.Equals(filter) select x.Value) != null);
            if (result)
            {
                FiltersWhere.Remove(filter);
            }
            return result;
        }

        private void AddFilterWhere(TextBoxFilter filter)
        {
            if ((from x in FiltersWhere where x.Key.Equals(filter) select x.Value) == null)
            {
                FiltersWhere.Add(filter, filter.FilterText.Replace("*", "%"));
            }
            else
            {
                FiltersWhere[filter] = filter.FilterText.Replace("*", "%");
            }
        }

        internal void RegisterTextBoxFilter(TextBoxFilter textboxFilter)
        {
            if (textboxFilter != null && !ListFilters.Contains(textboxFilter))
            {
                textboxFilter.PropertyChanged += EventINotifyPropertyChanged;
                textboxFilter.PreviewKeyDown += EventPreviewKeyDownTextBoxFilter;
                ListFilters.Add(textboxFilter);
            }
        }

        public bool InicializaGrid(string commandSql, string connectionString)
        {
            _SqlConnection = new SqlConnection(connectionString);
            _SqlConnection.Open();
            _disposeConnection = _SqlConnection.State == System.Data.ConnectionState.Open;
            return Initialize(commandSql, _SqlConnection);
        }

        public bool Initialize(string sql, SqlConnection sqlConnection)
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Background, (System.Threading.ThreadStart)delegate
            {
                this._SqlConnection = sqlConnection;

                using (InicializacaoDataGrid s = new InicializacaoDataGrid(_SqlConnection))
                {
                    ExpressoesSql = s.Inicia(sql);
                    s.SetCampos(this.Columns, TypeFields);

                    Pesquisa = new PesquisaExecucaoSql(_SqlConnection, ExpressoesSql);
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                    {
                      SetItemsSource(Pesquisa.Pesquisa(ExpressoesSql.Select));
                        if (ListFilters.Count > 0)
                            ListFilters.FirstOrDefault().txtFilter.Focus();
                    }));
                }
                this.UpdateLayout();
            });
            return true;
        }

        private void SetItemsSource(System.Data.DataView dataView)
        {            
            this.ItemsSource = dataView;
            if (dataView.Count == 0)
            {
                this.IsReadOnly = false;
                this.CanUserAddRows = true;
            }
            else
            {
                this.IsReadOnly = true;
                this.CanUserAddRows = false;
            }
        }
    }
}
