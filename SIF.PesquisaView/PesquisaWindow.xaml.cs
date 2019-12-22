using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Windows.Threading;

namespace SIF.PesquisaView
{
    /// <summary>
    /// Interaction logic for PesquisaWindow.xaml
    /// </summary>
    public partial class PesquisaWindow //: Window
    {
        private System.Data.DataRowView _dataRowView;

        public PesquisaWindow(Window owner, string commandSelect, SqlConnection sqlConnection)
            : this(owner, "CONSULTA", commandSelect, sqlConnection)
        {
        }
        public PesquisaWindow(Window owner, string titulo, string commandSelect, SqlConnection sqlConnection)
        {
            InitializeComponent();

            dgv.Initialize(sqlConnection, commandSelect);

            this.Title = "CONSULTA";
            if (!string.IsNullOrEmpty(titulo))
                this.Title = titulo.ToUpper();

            this.Owner = owner;

            this.PreviewKeyDown += new KeyEventHandler(W_PreviewKeyDown);
            this.Loaded += new RoutedEventHandler(W_Loaded);
        }

        private void W_Loaded(object sender, RoutedEventArgs e)
        {
            SetFocusWindow();
        }

        private void SetFocusWindow()
        {
            if (this.Dispatcher.Thread != System.Threading.Thread.CurrentThread)
            {
                this.Dispatcher.BeginInvoke(DispatcherPriority.Send, (System.Threading.SendOrPostCallback)delegate { SetFocusWindow(); }, null);
            }
            else
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    this.Activate();
                    Keyboard.Focus(this);
                    this.Focus();
                    this.Topmost = true;
                    this.Topmost = false;
                }));
            }
        }

        private void W_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }

        public System.Data.DataRowView PegaPesquisaSelected()
        {
            this.ShowDialog();
            return this._dataRowView;
        }

        private void dgv_PreviewKeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && dgv.SelectedItem != null)
            {
                this._dataRowView = dgv.SelectedItem as System.Data.DataRowView;
                this.Close();
                e.Handled = true;
            }
        }
    }
}
