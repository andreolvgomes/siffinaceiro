using SIF.Pesquisa.DataGrid;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIF.Pesquisa
{
    internal class PesquisaExecucaoSql : IDisposable
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        private SqlConnection sqlConnection;
        private ExpressaoSqlCommand expressoesSql;

        public PesquisaExecucaoSql(SqlConnection sqlConnection, ExpressaoSqlCommand expressoesSql)
        {
            this.sqlConnection = sqlConnection;
            this.expressoesSql = expressoesSql;
        }

        public DataView Pesquisa(string sqlcommand)
        {
            return Pesquisa(sqlcommand, new Dictionary<TextBoxFilter, string>());
        }

        public DataView Pesquisa(string sqlcommand, Dictionary<TextBoxFilter, string> filtersWhere)
        {
            DataTable datatable = new DataTable();
            using (SqlCommand command = sqlConnection.CreateCommand())
            {
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = GetCommandText(filtersWhere);
                using (SqlDataAdapter adp = new SqlDataAdapter(command))
                {
                    adp.Fill(datatable);
                }
            }
            if (datatable.Rows.Count > 0)
                return new DataView(datatable);
            return null;
        }

        private string GetCommandText(Dictionary<TextBoxFilter, string> filtersWhere)
        {
            return string.Format("{0} {1} {2}", expressoesSql.Select, GetWhere(filtersWhere), expressoesSql.OrderBy);
        }

        private string GetWhere(Dictionary<TextBoxFilter, string> filtersWhere)
        {
            string _where = "";
            string _campo = "";
            string _filtro = "";

            if (!string.IsNullOrEmpty(expressoesSql.Where))
            {
                _where = string.Format("WHERE ({0})", expressoesSql.Where.Replace("WHERE", "").Trim());
                if (filtersWhere.Count > 0)
                    _where += " AND (";
            }
            else if (filtersWhere.Count > 0)
            {
                _where = " WHERE (";
            }
            var list = filtersWhere.ToList();
            for (int i = 0; i < filtersWhere.Count; i++)
            {
                TextBoxFilter f = list[i].Key;
                if (f.FilterColumnInfo.CamposBase != null)
                    _campo = string.Format("{0}", f.FilterColumnInfo.CamposBase.FullName);
                else
                    _campo = string.Format("[{0}]", f.FilterColumnInfo.PropertyPath);
                _filtro = list[i].Value.Replace("'", "''");

                if (f.FilterColumnInfo.PropertyType == typeof(decimal))
                    _filtro = _filtro.Replace(",", ".");
                else if (f.FilterColumnInfo.PropertyType == typeof(DateTime))
                    _campo = string.Format("CONVERT(varchar, {0}, 103)", _campo);

                if (i > 0)
                    _where += " AND ";
                _where += string.Format(" {0} LIKE '{1}%'", _campo, _filtro);
                if ((i + 1) == filtersWhere.Count)
                    _where += ")";
            }
            return _where.Replace("  ", " ").ToUpper();
        }
    }
}
