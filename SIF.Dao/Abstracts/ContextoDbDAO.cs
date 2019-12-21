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
    public class ContextoDbDAO<T> : IDisposable where T : IModelProperty
    {
        /// <summary>
        /// Remover os recursos alocados que não estão mais processados
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public ConnectionDb Connection
        {
            get
            {
                return connectionDb;
            }
        }

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

        private ConnectionDb connectionDb;
        /// <summary>
        /// Construtor ProviderDbRecords
        /// </summary>
        /// <param name="connectionDb">Conexão com banco</param>
        public ContextoDbDAO(ConnectionDb connectionDb)
        {
            this.connectionDb = connectionDb;
            this.parameterAuto = new CreateCommandsSql2<T>();
        }

        /// <summary>
        /// Convert um DataRow para o Model do tipo corrente.
        /// </summary>
        /// <param name="dataRow">Record</param>
        /// <returns></returns>
        private T ConvertDataRowToModel(DataRow dataRow)
        {
            T model = (T)Activator.CreateInstance(typeof(T));
            foreach (KeyValuePair<string, CamposPropertyInfo> entry in parameterAuto.ParametersFull)
            {
                if (!dataRow.IsNull(entry.Value.PropertyInfo.Name))
                    entry.Value.PropertyInfo.SetValue(model, dataRow[entry.Value.PropertyInfo.Name], null);
            }
            return model;
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

            using (commandInsert.Transaction = Connection.GetSqlConnection().BeginTransaction("tInsert"))
            {
                try
                {
                    parameterAuto.AddParametersInsert(commandInsert, model);
                    commandInsert.ExecuteNonQuery();
                    OrganizaIdentity(commandInsert, model);
                    /// confirmar o insert no banco
                    /// 
                    commandInsert.Transaction.Commit();

                    return true;
                }
                catch (SqlException ex)
                {
                    ///desfaz o que foi feito
                    ///
                    commandInsert.Transaction.Rollback();
                }
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

            using (commandUpdate.Transaction = Connection.GetSqlConnection().BeginTransaction())
            {
                parameterAuto.AddParametersUpdate(commandUpdate, model as IModelProperty, model_original as IModelProperty);
                try
                {
                    commandUpdate.ExecuteNonQuery();
                    commandUpdate.Transaction.Commit();

                    return true;
                }
                catch (SqlException ex)
                {
                    commandUpdate.Transaction.Rollback();
                }
            }
            return false;
        }

        public bool Delete(T model)
        {
            if (commandDelete == null)
                commandDelete = Connection.GetSqlCommand(parameterAuto.CommandTextDelete);
            commandDelete.Parameters.Clear();

            using (commandDelete.Transaction = Connection.GetSqlConnection().BeginTransaction())
            {
                parameterAuto.AddParametersDelete(commandDelete, model);
                try
                {
                    commandDelete.ExecuteNonQuery();
                    commandDelete.Transaction.Commit();
                    return true;
                }
                catch (SqlException ex)
                {
                    commandDelete.Transaction.Rollback();
                }
            }
            return false;
        }

        /// <summary>
        /// Pega o value do campo Identity da tabela corrente e set na propriedade correspondente
        /// </summary>
        /// <param name="command"></param>
        /// <param name="model"></param>
        private void OrganizaIdentity(SqlCommand command, T model)
        {
            if (parameterAuto.HasFieldIdentity)
            {
                command.Parameters.Clear();

                command.CommandText = string.Format("SELECT IDENT_CURRENT('{0}') AS Value", parameterAuto.NomeTabela);
                DataTable datatable = new DataTable();
                using (SqlDataAdapter adp = new SqlDataAdapter(command))
                {
                    adp.Fill(datatable);
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
        }
    }
}
