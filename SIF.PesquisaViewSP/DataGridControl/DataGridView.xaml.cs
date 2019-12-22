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
using System.Windows.Threading;

namespace SIF.PesquisaViewSP.DataGridControl
{
    /// <summary>
    /// Interaction logic for DataGridView.xaml
    /// </summary>
    public partial class DataGridView : System.Windows.Controls.DataGrid
    {
        public int FocusColunaIndex { get; set; }

        private List<TextBoxFilter> TextBoxFilters;
        public List<CamposTipo> TypeFields { get; set; }
        public int CountColumns { get; set; }

        private Dictionary<TextBoxFilter, string> FiltersWhere;

        private KeyEventHandler Event_PreviewKeyDownTextBoxFilter;
        private KeyboardFocusChangedEventHandler Event_KeyboardFocusChangedEventHandler;

        private ScrollViewer scrollViewer = null;
        private CarregaItemsSource carregaItemsSource = null;

        private readonly CreateColumnsDataGridView createColumnsDataGridView;

        public DataGridView()
        {
            InitializeComponent();

            this.TextBoxFilters = new List<TextBoxFilter>();
            this.TypeFields = new List<CamposTipo>();
            this.createColumnsDataGridView = new CreateColumnsDataGridView(this);
            this.FiltersWhere = new Dictionary<TextBoxFilter, string>();

            this.Event_PreviewKeyDownTextBoxFilter = new KeyEventHandler(TextFilter_PreviewKeyDown);
            this.Event_KeyboardFocusChangedEventHandler += new KeyboardFocusChangedEventHandler(TextFilter_KeyboardFocusChangedEventHandler);

            this.PreviewKeyDown += new KeyEventHandler(G_PreviewKeyDown);
            this.Loaded += new RoutedEventHandler(G_Loaded);
        }

        private void G_Loaded(object sender, RoutedEventArgs e)
        {
            new System.Threading.Thread(new System.Threading.ThreadStart(Sync_DoWork)).Start();
            this.Loaded -= new RoutedEventHandler(G_Loaded);
        }

        internal void ExecutaFiltro()
        {
            this.carregaItemsSource.CarregaDataView(this.FiltersWhere);
            this.TrataGrid();
        }

        private void G_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                ViewScript v = new ViewScript(Window.GetWindow(this), carregaItemsSource.CommandSelectFull, sqlConnection);
                v.ShowDialog();
            }
            else if (e.Key == Key.Up && this.SelectedIndex == 0)
            {
                if (e.OriginalSource is DataGridCell)
                {
                    TextBoxFilter filter = TextBoxFilters.FirstOrDefault(c => c.IndexTextFilter == this.CurrentCell.Column.DisplayIndex);
                    if (filter != null)
                    {
                        filter.TxtFilter.Focus();
                        filter.TxtFilter.SelectAll();
                    }
                }
            }
        }

        private void TextFilter_KeyboardFocusChangedEventHandler(object sender, KeyboardFocusChangedEventArgs e)
        {
            this.ScrollToHome(-1);
        }

        private void ScrollToHome(int selectedIndex)
        {
            try
            {
                this.SelectedIndex = selectedIndex;
                if (scrollViewer == null)
                {
                    Decorator decorator = VisualTreeHelper.GetChild(this, 0) as Decorator;
                    if (decorator != null)
                    {
                        this.scrollViewer = VisualTreeHelper.GetChild(decorator, 0) as ScrollViewer;
                    }
                }
            }
            catch
            {
            }
            if (this.scrollViewer != null)
            {
                this.scrollViewer.ScrollToTop();
            }
        }

        private void TextFilter_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left || e.Key == Key.Right)
            {
                TextBoxFilter filter = sender as TextBoxFilter;
                UIElement element = e.OriginalSource as UIElement;
                if (e.Key == Key.Left)
                {
                    if (filter.IndexTextFilter > 0)
                    {
                        this.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => { MoveScrollViewerLine(filter.IndexTextFilter - 1); }));
                        element.MoveFocus(new TraversalRequest(FocusNavigationDirection.Previous));
                    }
                }
                else if (e.Key == Key.Right)
                {
                    if (filter.IndexTextFilter < (CountColumns - 1))
                    {
                        this.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => { MoveScrollViewerLine(filter.IndexTextFilter + 1); }));
                        element.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    }
                }
            }
            else if ((e.Key == Key.Enter || e.Key == Key.Down) && this.Items.Count > 0)
            {
                this.Focus();
                this.SelectedIndex = -1;

                DataGridRow dataGridRow = (DataGridRow)this.ItemContainerGenerator.ContainerFromIndex(0);
                if (dataGridRow != null)
                {
                    dataGridRow.Focus();
                    dataGridRow.IsSelected = true;
                    dataGridRow.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }
                using (GetDataGridCell get = new GetDataGridCell())
                {
                    DataGridCell cell = get.GetCell(this, 0, (sender as TextBoxFilter).IndexTextFilter);
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

        internal bool RegisterTextBoxFilter(TextBoxFilter textBoxFilter)
        {
            if (textBoxFilter != null && !this.TextBoxFilters.Contains(textBoxFilter))
            {
                textBoxFilter.PreviewKeyDown += this.Event_PreviewKeyDownTextBoxFilter;
                textBoxFilter.GotKeyboardFocus += this.Event_KeyboardFocusChangedEventHandler;

                this.TextBoxFilters.Add(textBoxFilter);
                /// focus
                /// 
                if (FocusColunaIndex == 0)
                {
                    if (this.TextBoxFilters.Count == 1)
                        textBoxFilter.TxtFilter.Focus();
                }
                else
                {
                    if (this.TextBoxFilters.Count == this.FocusColunaIndex)
                        textBoxFilter.TxtFilter.Focus();
                }
                return true;
            }
            return false;
        }

        internal void RegisterFilterWhere(TextBoxFilter textBoxFilter, string textFilter)
        {
            if (!textBoxFilter.HasFilter)
            {
                if ((from filter in this.FiltersWhere where filter.Key.Equals(textBoxFilter) select filter.Value) != null)
                    this.FiltersWhere.Remove(textBoxFilter);
            }
            else
            {
                this.FiltersWhere[textBoxFilter] = textFilter.Replace("*", "%");
            }
        }

        private System.Data.SqlClient.SqlConnection sqlConnection = null;

        public void Initialize(System.Data.SqlClient.SqlConnection sqlConnection, string sql)
        {
            this.sqlConnection = sqlConnection;
            this.createColumnsDataGridView.CreateColumns(sqlConnection, sql);

            this.carregaItemsSource = new CarregaItemsSource(sqlConnection);
            this.DataContext = carregaItemsSource;
        }

        private void Sync_DoWork()
        {
            this.carregaItemsSource.CarregaDataView();
            this.TrataGrid();
        }

        private void TrataGrid()
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Background, (System.Threading.ThreadStart)delegate
            {
                {
                    if (this.carregaItemsSource.TemRegistros)
                    {
                        this.IsReadOnly = true;
                        this.CanUserAddRows = false;
                    }
                    else
                    {
                        this.IsReadOnly = false;
                        this.CanUserAddRows = true;
                    }
                }
            });
        }
    }
}
