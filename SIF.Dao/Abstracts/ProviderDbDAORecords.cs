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
    public abstract class ProviderDbDAORecords<T> : IDisposable where T : IModelProperty
    {
        private DataTable _tabela;

        /// <summary>
        /// DataTable com os registros do banco
        /// </summary>
        public DataTable Tabela
        {
            get
            {
                return _tabela;
            }
        }

        /// <summary>
        /// Remover os recursos alocados que não estão mais processados
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        private int _indexNavigate = 0;
        /// <summary>
        /// Retorna o valor do index de onde está posicionado a navegação(first, next, previous ou last)
        /// </summary>
        public int IndexNavigate
        {
            get
            {
                return _indexNavigate;
            }
        }

        protected ConnectionDb Connection;
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

        /// <summary>
        /// Construtor ProviderDbRecords
        /// </summary>
        /// <param name="connectionDb">Conexão com banco</param>
        public ProviderDbDAORecords(ConnectionDb connectionDb)
        {
            this._tabela = new DataTable();
            this.Connection = connectionDb;
            this.parameterAuto = new CreateCommandsSql2<T>();
        }

        /// <summary>
        /// Carrega os registro e guarda em Tabela
        /// </summary>
        /// <returns></returns>
        public T LoadRecords()
        {
            return LoadRecords("");
        }

        /// <summary>
        /// Carrega os registro e guarda em Tabela
        /// </summary>
        /// <param name="commandText">CommandText SCRIPT(SQL)</param>
        /// <returns></returns>
        public T LoadRecords(string commandText)
        {
            _tabela = new DataTable();
            using (SqlCommand command = Connection.GetSqlCommand(((string.IsNullOrEmpty(commandText) ? parameterAuto.CommandTextSelect : commandText))))
            {
                using (SqlDataAdapter adp = new SqlDataAdapter(command))
                {
                    adp.Fill(_tabela);
                }
            }
            return this.First();
        }

        /// <summary>
        /// Organiza e retorna o primeiro registro
        /// </summary>
        /// <returns></returns>
        protected T First()
        {
            _indexNavigate = 0;
            return ConvertDataRowToModel(_tabela.Select().FirstOrDefault());
        }

        /// <summary>
        /// Organiza e retorna o registro anterior, se houver.
        /// </summary>
        /// <returns></returns>
        protected T Last()
        {
            _indexNavigate = _tabela.Rows.Count - 1;
            return ConvertDataRowToModel(_tabela.Select().LastOrDefault());
        }

        /// <summary>
        /// Organiza e retorna o próximo registro, se houver.
        /// </summary>
        /// <returns></returns>
        protected T Next()
        {
            if (_indexNavigate < _tabela.Rows.Count - 1)
                _indexNavigate++;
            return ConvertDataRowToModel(_tabela.Select().Skip(_indexNavigate).FirstOrDefault());
        }

        /// <summary>
        /// Organiza e retorna o registro anteriro, se houver.
        /// </summary>
        /// <returns></returns>
        protected T Previous()
        {
            if (_indexNavigate > 0)
                _indexNavigate--;
            return ConvertDataRowToModel(_tabela.Select().Skip(_indexNavigate).FirstOrDefault());
        }

        /// <summary>
        /// Convert um DataRow para o Model do tipo corrente.
        /// </summary>
        /// <param name="dataRow">Record</param>
        /// <returns></returns>
        private T ConvertDataRowToModel(DataRow dataRow)
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
                    /// confirmar o insert no banco
                    ///                     
                    commandInsert.Transaction.Commit();

                    this.OrganizaIdentity(model);                    
                    ConvertModelToDataRow(model);

                    return true;
                }
                catch (SqlException ex)
                {
                    ///desfaz o que foi feito
                    ///
                    commandInsert.Transaction.Rollback();
                    throw ex;
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
                    throw ex;
                }
            }
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
                    RemoveDataRow(model);

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
        /// Após um delete o datarow deve ser removido da tabela
        /// </summary>
        /// <param name="model"></param>
        private void RemoveDataRow(T model)
        {
            DataRow dataRow = _tabela.Select(parameterAuto.GetWhereDataRow(model)).FirstOrDefault();
            _tabela.Rows.Remove(dataRow);
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
                using (SqlCommand command = this.Connection.GetSqlCommand(string.Format("SELECT IDENT_CURRENT('{0}') AS Value", parameterAuto.NomeTabela)))
                {
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

        /// <summary>
        /// Covert um Model para DataRow. Após a conversão adiciona o DataRow na tabela.
        /// </summary>
        /// <param name="model"></param>
        private void ConvertModelToDataRow(T model)
        {
            System.Data.DataRow row = _tabela.NewRow();
            if (_tabela.Columns.Count > 0)
            {
                foreach (KeyValuePair<string, CamposPropertyInfo> entry in parameterAuto.ParametersFull)
                    if (_tabela.Columns.Contains(entry.Key))
                        row[entry.Key] = parameterAuto.GetValueModel(entry.Value.PropertyInfo, model);
                _tabela.Rows.Add(row);
            }
        }

        protected T FirstLastOrDefault()
        {
            if (_tabela.Rows.Count == 0)
                return (T)Activator.CreateInstance(typeof(T));
            else if (_tabela.Rows.Count == 1)
                _indexNavigate = 0;
            else if (_indexNavigate > 0)
                _indexNavigate--;
            else
                _indexNavigate++;
            return ConvertDataRowToModel(_tabela.Select().Skip(_indexNavigate).FirstOrDefault());
        }
    }
}
