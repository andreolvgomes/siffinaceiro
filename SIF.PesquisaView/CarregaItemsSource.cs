using SIF.PesquisaView.DataGridControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIF.PesquisaView
{
    public class CarregaItemsSource : INotifyPropertyChanged, IDisposable
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        private DataView _ViewResult;
        /// <summary>
        /// Resultado da consulta(select)
        /// </summary>
        public DataView ViewResult
        {
            get { return _ViewResult; }
            set
            {
                if (_ViewResult != value)
                {
                    _ViewResult = value;
                    this.OnPropertyChangedEventHandler("ViewResult");
                }
            }
        }

        public bool TemRegistros
        {
            get
            {
                if (this.ViewResult == null)
                    return false;
                return this.ViewResult.Count > 0;
            }
        }

        private SqlCommand sqlCommand = null;
        private SqlDataAdapter sqlDataAdapter = null;

        private const string COMMAND_SELECT = "SELECT TOP 100 * FROM view_temppesquisa";
        public string CommandSelectFull { get; private set; }

        public CarregaItemsSource(SqlConnection sqlConnection)
        {
            this.sqlCommand = new SqlCommand("", sqlConnection);
            this.sqlDataAdapter = new SqlDataAdapter(this.sqlCommand);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChangedEventHandler(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        internal void CarregaDataView(Dictionary<TextBoxFilter, string> filters)
        {
            string where = this.GetWhere(filters);
            StringBuilder sb = new StringBuilder(COMMAND_SELECT);

            if (!string.IsNullOrEmpty(where))
            {
                sb.Append(string.Format(" WHERE {0}", where));
            }
            this.CarregaDataView(sb.ToString());
        }

        internal void CarregaDataView()
        {
            this.CarregaDataView(COMMAND_SELECT);
        }

        private void CarregaDataView(string commandSelectQuery)
        {
            this.CommandSelectFull = commandSelectQuery;

            DataTable dataTable = new DataTable();
            this.sqlCommand.CommandText = commandSelectQuery;
            this.sqlDataAdapter.Fill(dataTable);
            this.ViewResult = new DataView(dataTable);
        }

        private string GetWhere(Dictionary<TextBoxFilter, string> filters)
        {
            string _campo = "";
            string _filtro = "";
            StringBuilder sb = new StringBuilder();

            var list = filters.ToList();
            for (int i = 0; i < filters.Count; i++)
            {
                TextBoxFilter textBoxFilter = list[i].Key;
                if (i > 0)
                    sb.Append(" AND ");

                _campo = string.Format("[{0}]", textBoxFilter.FilterColumnInfo.PropertyPath);
                _filtro = list[i].Value.Replace("'", "''");

                if (textBoxFilter.FilterColumnInfo.PropertyType == typeof(decimal))
                    _filtro = _filtro.Replace(",", ".");
                else if (textBoxFilter.FilterColumnInfo.PropertyType == typeof(DateTime))
                    _campo = string.Format("CONVERT(VARCHAR, {0}, 103)", _campo);

                sb.Append(string.Format(" {0} LIKE '{1}%'", _campo, _filtro));
            }
            return sb.ToString();
        }
    }
}
