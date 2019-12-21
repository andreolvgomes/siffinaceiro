using System;
using System.Collections.Generic;
using System.Data;
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

namespace SIF.PesquisaView
{
    /// <summary>
    /// Interaction logic for ViewScript.xaml
    /// </summary>
    public partial class ViewScript
    {
        private SqlConnection sqlConnection;

        public ViewScript(Window owner, string commandSelct, SqlConnection sqlConnection)
        {
            InitializeComponent();

            this.DataContext = this;

            txtComandos.Text = "";
            txtComandos.Text = commandSelct;
            this.sqlConnection = sqlConnection;
            this.Owner = owner;

            this.PreviewKeyDown += new KeyEventHandler(W_PreviewKeyDown);
            this.Loaded += new RoutedEventHandler(W_Loaded);
        }

        private void W_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable t = new DataTable();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = string.Format("SELECT VIEW_DEFINITION FROM [INFORMATION_SCHEMA].[VIEWS] WHERE TABLE_NAME = 'view_temppesquisa'");
                    using (SqlDataAdapter adp = new SqlDataAdapter(command))
                    {
                        adp.Fill(t);
                    }
                }
                txtScript.Text = "";
                txtScript.Text = t.Rows[0][0].ToString().Replace("CREATE VIEW view_temppesquisa AS ", "");
            }
            catch
            {
            }
        }

        private void W_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) this.Close();
        }
    }
}
