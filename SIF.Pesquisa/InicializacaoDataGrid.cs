using SIF.WPF.Styles.Windows.Controls;
using Microsoft.Data.Schema.ScriptDom;
using Microsoft.Data.Schema.ScriptDom.Sql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SIF.Pesquisa
{
    internal class InicializacaoDataGrid : IDisposable
    {
        public ExpressaoSqlCommand ExpressoesSql { get; set; }

        private SqlConnection sqlConnection;

        public InicializacaoDataGrid(SqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        internal ExpressaoSqlCommand Inicia(string commandSql)
        {
            ExpressoesSql = Parse(commandSql);
            return ExpressoesSql;
        }

        private ExpressaoSqlCommand Parse(string sqlCommand)
        {
            ExpressaoSqlCommand result = new ExpressaoSqlCommand();
            sqlCommand = sqlCommand.ToUpper();

            TSql100Parser parser = new TSql100Parser(false);
            System.IO.TextReader rd = new System.IO.StringReader(sqlCommand);
            IList<ParseError> errors;
            IScriptFragment fragments = parser.Parse(rd, out errors);

            if (errors.Count > 0)
            {
                string retMessage = string.Empty;
                foreach (var error in errors)
                    retMessage += error.Identifier + " - " + error.Message + " - position: " + error.Offset + "; ";
                result.MessageError = retMessage;
            }
            try
            {
                QueryExpression query = ((fragments as TSqlScript).Batches[0].Statements[0] as SelectStatement).QueryExpression;

                result.From = (query as QuerySpecification).FromClauses[0].GetString();
                result.Where = (query as QuerySpecification).WhereClause.GetString();
                result.GroupBy = (query as QuerySpecification).GroupByClause.GetString();
                result.Having = (query as QuerySpecification).HavingClause.GetString();
                result.OrderBy = ((fragments as TSqlScript).Batches[0].Statements[0] as SelectStatement).OrderByClause.GetString();
                List<string> fieldsListCompleto = new List<string>();
                foreach (TSqlFragment f in (query as QuerySpecification).SelectElements)
                    fieldsListCompleto.Add((f as TSqlFragment).GetString());

                result.Fields = string.Join(", ", fieldsListCompleto.ToArray());
                result.FieldsList = GetParseFields(fieldsListCompleto);

                List<string> fieldsList = new List<string>();
                foreach (CommandSqlCampos f in result.FieldsList)
                    fieldsList.Add(f.NameFieldCompleto);
                result.FieldsBase = string.Join(", ", fieldsList.ToArray());
                result.Select = string.Format("SELECT TOP 100 {0} FROM {1}", string.Join(", ", fieldsList.ToArray()), result.From);
            }
            catch (Exception ex)
            {
                result.MessageError = ex.ToString();
            }
            return result;
        }

        private List<CommandSqlCampos> GetParseFields(List<string> fieldsListCompleto)
        {
            List<CommandSqlCampos> fields = new List<CommandSqlCampos>();
            foreach (string c in fieldsListCompleto)
            {
                CommandSqlCampos campo = new CommandSqlCampos();
                string[] r = Splist(c, " AS ");
                if (c.IndexOf("CAST") != -1 || c.IndexOf("CONVERT") != -1)
                {
                    campo.NameFieldCompleto = r[1];
                    campo.NameField = Splist(r[1], ".")[1];
                }
                else
                {
                    campo.NameFieldCompleto = r[0];
                    campo.NameField = Splist(r[0], ".")[1];
                }
                campo.ApelidoField = Splist(r[1], ".")[1];
                fields.Add(campo);
            }
            return fields;
        }

        private string[] Splist(string description, string separator)
        {
            string[] result = new string[2];
            if (description.IndexOf(separator) != -1)
            {
                result[0] = description.Substring(0, description.IndexOf(separator));
                result[1] = description.Substring(description.IndexOf(separator) + separator.Length, description.Length - (description.IndexOf(separator) + separator.Length));
            }
            else
            {
                result[0] = result[1] = description;
            }
            return result;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        internal void SetCampos(System.Collections.ObjectModel.ObservableCollection<System.Windows.Controls.DataGridColumn> columns, List<CamposTipo> typeFields)
        {
            DataTable datatable = new DataTable();
            using (SqlCommand command = sqlConnection.CreateCommand())
            {
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = string.Format("SELECT TOP 0 {0} FROM {1}", ExpressoesSql.FieldsBase, ExpressoesSql.From);
                using (SqlDataAdapter adp = new SqlDataAdapter(command))
                {
                    adp.Fill(datatable);
                }
            }
            foreach (DataColumn dataColumn in datatable.Columns)
            {
                DataGridTextColumn dataGridTextColumn = new DataGridTextColumn();

                if (dataColumn.DataType == typeof(decimal))
                    dataGridTextColumn.Binding = new Binding(dataColumn.ColumnName) { StringFormat = "{0:0.000}", ConverterCulture = System.Globalization.CultureInfo.GetCultureInfo("pt-BR") };
                else if (dataColumn.DataType == typeof(DateTime))
                    dataGridTextColumn.Binding = new Binding(dataColumn.ColumnName) { StringFormat = "{0:dd/MM/yyyy}", ConverterCulture = System.Globalization.CultureInfo.GetCultureInfo("pt-BR") };
                else
                    dataGridTextColumn.Binding = new Binding(dataColumn.ColumnName);

                CommandSqlCampos b = ExpressoesSql.FieldsList.FirstOrDefault(c => c.NameField.Trim() == dataColumn.ColumnName.Trim());
                if (b != null)
                    dataGridTextColumn.Header = ConvertStringToLower(b.ApelidoField);
                else
                    dataGridTextColumn.Header = dataColumn.ColumnName;

                if (datatable.Columns.Count <= 5)
                    dataGridTextColumn.MinWidth = 250;
                else if (datatable.Columns.Count > 5 && datatable.Columns.Count <= 10)
                    dataGridTextColumn.MinWidth = 150;
                else
                    dataGridTextColumn.MinWidth = 100;

                typeFields.Add(new CamposTipo() { Campo = dataColumn.ColumnName, Type = dataColumn.DataType, CamposBase = b });
                columns.Add(dataGridTextColumn);
            }
        }

        private object ConvertStringToLower(string description)
        {
            description = description.Replace("[", "").Replace("]", "").ToLower().Trim();
            string s = description.Substring(0, 1).ToUpper();
            description = description.Substring(1, description.Length - 1);
            description = s + description;
            return description;
        }
    }
}
