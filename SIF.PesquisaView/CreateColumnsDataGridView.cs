using SIF.WPF.Styles.Windows.Controls;
using SIF.PesquisaView.DataGridControl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SIF.PesquisaView
{
    public class CreateColumnsDataGridView : IDisposable
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        private DataGridView datagridView = null;
        private SqlConnection sqlConnection = null;

        public CreateColumnsDataGridView(DataGridView datagridView)
        {
            this.datagridView = datagridView;
        }

        public void CreateColumns(SqlConnection sqlConnection, string commandSelectQuery)
        {
            this.sqlConnection = sqlConnection;

            this.PreparaViewTemp(commandSelectQuery);
            DataTable datatable = new DataTable();

            using (SqlCommand command = sqlConnection.CreateCommand())
            {
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "SELECT TOP 0 * FROM view_temppesquisa";
                using (SqlDataAdapter adp = new SqlDataAdapter(command))
                {
                    adp.Fill(datatable);
                }
            }
            int count = datatable.Columns.Count;
            string columnName = "";
            foreach (DataColumn column in datatable.Columns)
            {
                DataGridTextColumn dataGridTextColumn = new DataGridTextColumn();
                columnName = column.ColumnName;

                if (column.DataType == typeof(decimal))
                    dataGridTextColumn.Binding = new Binding(columnName) { StringFormat = "{0:0.000}", ConverterCulture = System.Globalization.CultureInfo.GetCultureInfo("pt-BR") };
                else if (column.DataType == typeof(DateTime))
                    dataGridTextColumn.Binding = new Binding(columnName) { StringFormat = "{0:dd/MM/yyyy}", ConverterCulture = System.Globalization.CultureInfo.GetCultureInfo("pt-BR") };
                else
                    dataGridTextColumn.Binding = new Binding(columnName);

                dataGridTextColumn.Header = columnName;

                if (count <= 5)
                    dataGridTextColumn.MinWidth = 250;
                else if (count > 5 && count <= 10)
                    dataGridTextColumn.MinWidth = 150;
                else
                    dataGridTextColumn.MinWidth = 100;

                datagridView.TypeFields.Add(new CamposTipo() { Campo = columnName, Type = column.DataType });
                datagridView.Columns.Add(dataGridTextColumn);
            }
        }

        private void PreparaViewTemp(string commandSelectQuery)
        {
            /// SELECT * FROM Clientes
            /// SELECT TOP 100 FROM Clientes
            /// 
            if (commandSelectQuery.ToUpper().IndexOf(" TOP ") != -1)
            {
            }
            commandSelectQuery = commandSelectQuery.Substring(7, commandSelectQuery.Length - 7);
            commandSelectQuery = "SELECT TOP 99.99 PERCENT " + commandSelectQuery;
            using (SqlCommand command = this.sqlConnection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = "IF EXISTS(SELECT name FROM sys.sysobjects WHERE name = 'view_temppesquisa' AND type = 'V')	DROP VIEW view_temppesquisa";
                command.ExecuteNonQuery();

                string commandCreateView = string.Format("CREATE VIEW view_temppesquisa AS {0}", commandSelectQuery);
                command.CommandText = commandCreateView;
                command.ExecuteNonQuery();
            }
        }
    }
}
