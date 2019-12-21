using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SIF.Dao
{
    //    select * from Produtos

    //;WITH Results_CTE AS
    //(
    //    SELECT
    //        ROW_NUMBER() OVER (ORDER BY Pro_codigo) AS RowNum, *
    //    FROM dbo.Produtos
    //)
    //SELECT * FROM Results_CTE
    //WHERE RowNum = 3

    public abstract class ProviderDbDAORecords2<T> : IDisposable where T : IModelProperty
    {
        /// <summary>
        /// Remover os recursos alocados que não estão mais processados
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public int CountRegistros { get; private set; }
        private int _indexNavigate = 0;

        public ConnectionDb Connection { get; private set; }
        private CreateCommandsSql2<T> parameterAuto;

        /// <summary>
        /// SqlCommand dos comandos de Insert no banco.
        /// </summary>
        private SqlCommand commandInsert;

        /// <summary>
        /// SqlCommand dos comandos de update no banco
        /// </summary>
        private SqlCommand commandUpdate;

        /// <summary>
        /// commandDelete dos comandos de delete no banco
        /// </summary>
        private SqlCommand commandDelete;

        private SqlCommand commandSelect;

        private string commandText = "";

        /// <summary>
        /// Construtor ProviderDbRecords
        /// </summary>
        /// <param name="connectionDb">Conexão com banco</param>
        public ProviderDbDAORecords2(ConnectionDb connectionDb)
        {
            this.Connection = connectionDb;
            this.parameterAuto = new CreateCommandsSql2<T>();
        }

        public T LoadRecords(string tabela, string campo_ordernacao)
        {
            return this.LoadRecords(tabela, campo_ordernacao, "");
        }

        public T LoadRecords(string tabela, string campo_ordernacao, string where)
        {
            if (!string.IsNullOrEmpty(where))
            {
                where = where.ToUpper().Replace("WHERE", "");
                commandText = string.Format(@";WITH Results AS
(
    SELECT
        ROW_NUMBER() OVER (ORDER BY {0}) AS ID, *
    FROM dbo.{1} WHERE {2}
)", campo_ordernacao, tabela, where);
            }
            else
            {
                commandText = string.Format(@";WITH Results AS
(
    SELECT
        ROW_NUMBER() OVER (ORDER BY {0}) AS ID, *
    FROM dbo.{1}
)", campo_ordernacao, tabela);
            }
            commandText += "\nSELECT COUNT(ID) FROM Results";

            using (SqlCommand command = this.Connection.GetSqlCommand("sp_sqlexec", CommandType.StoredProcedure))
            {
                command.Parameters.AddWithValue("@p1", commandText);
                this.CountRegistros = this.Connection.GetValue<int>(command);
            }
            return this.First(null);
        }

        /// <summary>
        /// Organiza e retorna o primeiro registro
        /// </summary>
        /// <returns></returns>
        protected T First(T model)
        {
            this._indexNavigate = 1;
            return this.ToModel(this._indexNavigate);
        }

        /// <summary>
        /// Organiza e retorna o registro anterior, se houver.
        /// </summary>
        /// <returns></returns>
        protected T Last(T model)
        {
            this._indexNavigate = this.CountRegistros;
            return this.ToModel(this._indexNavigate);
        }

        /// <summary>
        /// Organiza e retorna o próximo registro, se houver.
        /// </summary>
        /// <returns></returns>
        protected T Next(T model)
        {
            if (this._indexNavigate < this.CountRegistros)
                this._indexNavigate++;
            return this.ToModel(this._indexNavigate);
        }

        /// <summary>
        /// Organiza e retorna o registro anteriro, se houver.
        /// </summary>
        /// <returns></returns>
        protected T Previous(T model)
        {
            if (this._indexNavigate > 1)
                this._indexNavigate--;
            return this.ToModel(this._indexNavigate);
        }

        /// <summary>
        /// Convert um DataRow para o Model do tipo corrente.
        /// </summary>
        /// <param name="dataRow">Record</param>
        /// <returns></returns>
        private T ToModel(DataRow dataRow)
        {
            if (dataRow != null)
            {
                T model = (T)Activator.CreateInstance(typeof(T));
                foreach (KeyValuePair<string, CamposPropertyInfo> entry in parameterAuto.ParametersFull)
                {
                    if (!dataRow.IsNull(entry.Value.PropertyInfo.Name))
                        entry.Value.PropertyInfo.SetValue(model, dataRow[entry.Value.PropertyInfo.Name], null);
                }
                return model;
            }
            return null;
        }

        private T ToModel(int index_navigate)
        {
            //this.commandText = string.Format("{0} WHERE ID = @ID", commandText).Replace("COUNT(ID)", " * ");
            //if (this.commandSelect == null)
            //{
            //    this.commandSelect = this.Connection.GetSqlCommand(this.commandText);
            //}
            //this.commandSelect.Parameters.Clear();
            //this.commandSelect.Parameters.AddWithValue("@ID", index_navigate);
            //return this.ToModel(this.Connection.GetDataRowFirst(this.commandSelect));

            string text = string.Format("{0} WHERE ID = {1}", commandText, index_navigate).Replace("COUNT(ID)", " * ");
            if (this.commandSelect == null)
            {
                this.commandSelect = this.Connection.GetSqlCommand("sp_sqlexec", CommandType.StoredProcedure);
            }
            this.commandSelect.Parameters.Clear();
            this.commandSelect.Parameters.AddWithValue("@p1", text);
            return this.ToModel(this.Connection.GetDataRowFirst(this.commandSelect));
        }

        /// <summary>
        /// Faz insert do modelo no banco
        /// </summary>
        /// <param name="model">model(dados)</param>
        /// <returns></returns>
        public bool Insert(T model)
        {
            if (commandInsert == null)
                commandInsert = Connection.GetSqlCommand(parameterAuto.CommandTextInsert);
            commandInsert.Parameters.Clear();
            parameterAuto.AddParametersInsert(commandInsert, model);

            if (this.Connection.ExecuteNonQueryTransaction(this.commandInsert))
            {
                this.CountRegistros++;
                this.OrganizaIdentity(model);

                return true;
            }
            return false;
        }

        public bool Update(T model)
        {
            return this.Update(model, model);
        }

        public bool Update(T model, T model_original)
        {
            if (commandUpdate == null)
                commandUpdate = Connection.GetSqlCommand(parameterAuto.CommandTextUpdate);
            commandUpdate.Parameters.Clear();
            parameterAuto.AddParametersUpdate(commandUpdate, model as IModelProperty, model_original as IModelProperty);

            return this.Connection.ExecuteNonQueryTransaction(commandUpdate);
        }

        public bool Delete(T model)
        {
            if (commandDelete == null)
                commandDelete = Connection.GetSqlCommand(parameterAuto.CommandTextDelete);
            commandDelete.Parameters.Clear();
            parameterAuto.AddParametersDelete(commandDelete, model);

            if (this.Connection.ExecuteNonQueryTransaction(commandDelete))
            {
                this.CountRegistros--;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Pega o value do campo Identity da tabela corrente e set na propriedade correspondente
        /// </summary>
        /// <param name="command"></param>
        /// <param name="model"></param>
        private void OrganizaIdentity(T model)
        {
            if (parameterAuto.HasFieldIdentity)
            {
                DataTable datatable = this.Connection.GetDatatable(string.Format("SELECT IDENT_CURRENT('{0}') AS Value", parameterAuto.NomeTabela));
                if (datatable.Rows.Count > 0)
                {
                    object value = datatable.Rows[0][0];
                    foreach (KeyValuePair<string, CamposPropertyInfo> entry in parameterAuto.ParametersFieldsIdentity)
                    {
                        PropertyInfo propertyInfo = entry.Value.PropertyInfo;
                        if (propertyInfo.PropertyType.Name.ToUpper() == "INT16")
                            propertyInfo.SetValue(model, Convert.ToInt16(value), null);
                        else if (propertyInfo.PropertyType.Name.ToUpper() == "INT32")
                            propertyInfo.SetValue(model, Convert.ToInt32(value), null);
                        else if (propertyInfo.PropertyType.Name.ToUpper() == "INT64")
                            propertyInfo.SetValue(model, Convert.ToInt64(value), null);
                    }
                }
            }
        }

        protected T FirstLastOrDefault()
        {
            if (this.CountRegistros == 0)
                return (T)Activator.CreateInstance(typeof(T));
            else if (this.CountRegistros == 1)
                this._indexNavigate = 1;
            else if (this._indexNavigate > 1)
                this._indexNavigate--;
            else
                this._indexNavigate++;
            return this.ToModel(this._indexNavigate);
        }
    }
}
