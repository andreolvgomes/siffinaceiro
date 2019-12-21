using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SIF.Dao.Abstracts;

namespace SIF.Dao
{
    public abstract class ConnectionDbAbstract : IDisposable
    {
        public override string ToString()
        {
            return _connectionString;
        }

        public string ConnectionString
        {
            get
            {
                return _connectionString;
            }
        }

        /// <summary>
        /// Conexao corrente
        /// </summary>
        public SqlConnection Connection
        {
            get
            {
                if (_connection != null)
                    if (_connection.State != System.Data.ConnectionState.Closed)
                        return _connection;

                return this.GetSqlConnection();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private SqlConnection _connection;
        //public event EventHandler<NotifyConnectionOffEventArgs> NotifyUsuarioConexaoOffEventHandler;
        private string _connectionString;

        public ConnectionDbAbstract(string connectionString)
        {
            this._connectionString = connectionString;
            this._connection = new SqlConnection(connectionString);
        }

        /// <summary>
        /// Abre conexão
        /// </summary>
        public void Open()
        {
            this.GetSqlConnection();
        }

        /// <summary>
        /// Get conexão corrente para trabalhar
        /// </summary>
        /// <returns></returns>
        public SqlConnection GetSqlConnection()
        {
            if (this._connection != null)
                if (this._connection.State != System.Data.ConnectionState.Open)
                    this._connection.Open();
            return _connection;
        }

        /// <summary>
        /// Fecha conexão corrente
        /// </summary>
        public void Close()
        {
            if (this._connection != null)
            {
                if (this._connection.State != System.Data.ConnectionState.Closed)
                {
                    this._connection.Close();
                    this._connection.Dispose();
                }
            }
        }

        public void Dispose()
        {
            this.Close();
            GC.SuppressFinalize(this);
        }

        public bool VerificaConexao()
        {
            try
            {
                if (this._connection.State != ConnectionState.Closed)
                    this._connection.Close();
                this._connection.Open();
                return true;
            }
            catch
            {
            }
            return false;
        }
        /// <summary>
        /// Verifica conexão com o banco BDSIC
        /// </summary>
        /// <returns></returns>
        //public bool VerificaConexao()
        //{
        //    SqlConnection conexaov = null;
        //    do
        //    {
        //        try
        //        {
        //            if (conexaov == null)
        //                conexaov = new SqlConnection(string.Format("{0}; Connection Timeout = 5", _connectionString));
        //            if (conexaov.State != System.Data.ConnectionState.Closed)
        //                conexaov.Close();

        //            conexaov.Open();

        //            /// as próxima linhas só será executa se houver conexão
        //            /// 
        //            /// Dispose
        //            /// 
        //            conexaov.Close();
        //            conexaov.Dispose();
        //            return true;
        //        }
        //        catch (SqlException ex)
        //        {
        //            /// notifica usuário a falta de conexão
        //            /// 
        //            //this.OnNotifyUsuarioConexaoOffEventHandler(new NotifyConnectionOffEventArgs(_connection.DataSource, ex));
        //            //System.Windows.Forms.Application.DoEvents();
        //            //System.Threading.Thread.Sleep(10000);
        //        }
        //    } while (true);
        //}

        /// <summary>
        /// Notifica solicitante do evento que a conexão está off
        /// </summary>
        /// <param name="e"></param>
        //private void OnNotifyUsuarioConexaoOffEventHandler(NotifyConnectionOffEventArgs e)
        //{
        //    EventHandler<NotifyConnectionOffEventArgs> handler = NotifyUsuarioConexaoOffEventHandler;
        //    if (handler != null)
        //    {
        //        handler(this, e);
        //    }
        //}

        /// <summary>
        /// /// Pega e retorna SqlCommand padrão CommandType.Text e CommandText vazio
        /// </summary>
        /// <returns></returns>
        public SqlCommand GetSqlCommand()
        {
            return GetSqlCommand("", CommandType.Text);
        }

        /// <summary>
        /// Pega e retorna SqlCommand padrão CommandType.Text
        /// </summary>
        /// <param name="commandText">script sql</param>
        /// <returns></returns>
        public SqlCommand GetSqlCommand(string commandText)
        {
            return this.GetSqlCommand(commandText, CommandType.Text);
        }

        /// <summary>
        /// Pega e retorna SqlCommand
        /// </summary>
        /// <param name="commandText">script sql</param>
        /// <param name="commandType">type transação</param>
        /// <returns></returns>
        public SqlCommand GetSqlCommand(string commandText, CommandType commandType)
        {
            SqlCommand command = this.GetSqlConnection().CreateCommand();
            command.CommandTimeout = 3600;
            command.CommandType = commandType;
            command.CommandText = commandText;

            return command;
        }

        /// <summary>
        /// Retorna uma List de DataRow
        /// </summary>
        /// <param name="commandText">sql SQL</param>
        /// <returns></returns>
        public List<DataRow> GetListDataRow(string commandText)
        {
            List<DataRow> listDataRow = new List<DataRow>();
            foreach (DataRow row in this.GetDataRow(commandText))
                listDataRow.Add(row);
            return listDataRow;
        }

        /// <summary>
        /// Retorna uma List de DataRow
        /// </summary>
        /// <param name="command">SqlCommand já construído</param>
        /// <returns></returns>
        public List<DataRow> GetListDataRow(SqlCommand command)
        {
            List<DataRow> listDataRow = new List<DataRow>();
            foreach (DataRow row in this.GetDataRow(command))
                listDataRow.Add(row);
            return listDataRow;
        }

        /// <summary>
        /// Retorna linhas DataRow do resultado de um script(select)
        /// </summary>
        /// <param name="commandText">script(select)</param>
        /// <returns></returns>
        public IEnumerable GetDataRow(string commandText)
        {
            foreach (DataRow row in this.GetDatatable(commandText).Rows)
                yield return row;
        }

        /// <summary>
        /// REtorna uma linha de select
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public DataRow GetDataRowFirst(string commandText)
        {
            return this.GetDataRowFirst(this.GetSqlCommand(commandText));
        }

        public DataRow GetDataRowFirst(SqlCommand command)
        {
            foreach (DataRow row in this.GetDatatable(command).Rows)
                return row;
            return null;
        }

        /// <summary>
        /// Retorna linhas DataRow do resultado de um script(select)
        /// </summary>
        /// <param name="command">SqlCommand já construído</param>
        /// <returns></returns>
        public IEnumerable GetDataRow(SqlCommand command)
        {
            foreach (DataRow row in this.GetDatatable(command).Rows)
                yield return row;
        }

        /// <summary>
        /// Pega, organiza e retorna obj model. Select convertido para obj model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public IEnumerable GetObjModel<T>(string commandText)
        {
            foreach (DataRow row in this.GetDataRow(commandText))
                yield return this.ToModel<T>(row);
        }

        /// <summary>
        /// Organiza o obj model de acordo com o DataRow
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <returns></returns>
        private T ToModel<T>(DataRow row)
        {
            T model = (T)Activator.CreateInstance(typeof(T));
            try
            {
                PropertyInfo[] properties = ((Type)model.GetType()).GetProperties();
                object value;
                foreach (PropertyInfo property in properties)
                {
                    if (row.Table.Columns.Contains(property.Name))
                    {
                        value = row[property.Name];
                        if (value == DBNull.Value)
                            value = null;
                        property.SetValue(model, value, null);
                    }
                }
            }
            catch
            {
                throw;
            }
            return model;
        }

        /// <summary>
        /// Retorna datatable
        /// </summary>
        /// <param name="commandText">script(select)</param>
        /// <returns></returns>
        public DataTable GetDatatable(string commandText)
        {
            //using (SqlCommand command = this.GetSqlConnection().CreateCommand())
            //{
            //    command.CommandText = commandText;
            //    command.CommandType = CommandType.Text;
            //    return this.GetDatatable(command);
            //}
            using (SqlCommand command = this.GetSqlCommand("sp_sqlexec", CommandType.StoredProcedure))
            {
                command.Parameters.AddWithValue("@p1", commandText);
                return this.GetDatatable(command);
            }
        }

        /// <summary>
        /// Retorna datatable
        /// </summary>
        /// <param name="scriptSql">SqlCommand já construído</param>
        /// <returns></returns>
        public DataTable GetDatatable(SqlCommand command)
        {
            DataTable datatable = new DataTable();

            try
            {
                using (SqlDataAdapter adp = new SqlDataAdapter(command))
                {
                    adp.Fill(datatable);
                    if (command.Transaction != null)
                        command.Transaction.Commit();
                }
            }
            catch
            {
                if (command.Transaction != null)
                    command.Transaction.Rollback();

                throw;
            }
            return datatable;
        }

        /// <summary>
        /// Retorna value(s)
        /// </summary>
        /// <typeparam name="T">Tipo de retorno</typeparam>
        /// <param name="row">DataRow do select</param>
        /// <param name="index">indice da coluna onde tá o dado</param>
        /// <returns></returns>
        public T GetValue<T>(DataRow row, int index)
        {
            return (T)row[index];
        }

        /// <summary>
        /// Retorna value
        /// </summary>
        /// <typeparam name="T">Tipo de retorno</typeparam>
        /// <param name="row">DataRow do select</param>
        /// <param name="index">nome da coluna onde tá o dado</param>
        /// <returns></returns>
        public T GetValue<T>(DataRow row, string nomeCampo)
        {
            return (T)row[nomeCampo];
        }

        /// <summary>
        /// Retorna um value direto do banco
        /// </summary>
        /// <typeparam name="T">tipo de retorno</typeparam>
        /// <param name="commandText">script sql (select)</param>
        /// <returns></returns>
        public T GetValue<T>(string commandText)
        {
            return this.GetValue<T>("", commandText);
        }

        /// <summary>
        /// Retorna um value direto do banco
        /// </summary>
        /// <typeparam name="T">Tipo de retorno</typeparam>
        /// <param name="command">SqlCommand já construído</param>
        /// <returns></returns>
        public T GetValue<T>(SqlCommand command)
        {
            return this.GetValue<T>("", command);
        }

        /// <summary>
        /// Retorna um value direto do banco
        /// </summary>
        /// <typeparam name="T">Tipo retorno</typeparam>
        /// <param name="nomeCampo">nome do campo get</param>
        /// <param name="commandText">Script sql</param>
        /// <returns></returns>
        public T GetValue<T>(string nomeCampo, string commandText)
        {
            using (SqlCommand command = this.GetSqlCommand(commandText))
            {
                return this.GetValue<T>(nomeCampo, command);
            }
        }

        /// <summary>
        /// Retorna um value direto do banco
        /// </summary>
        /// <typeparam name="T">tipo de retorno</typeparam>
        /// <param name="nomeCampo">nome do campo onde está o dado</param>
        /// <param name="scriptSql">script sql (select)</param>
        /// <returns></returns>
        public T GetValue<T>(string nomeCampo, SqlCommand command)
        {
            object value = null;
            try
            {
                foreach (DataRow row in this.GetDatatable(command).Rows)
                {
                    if (!string.IsNullOrEmpty(nomeCampo))
                    {
                        value = row[nomeCampo];
                        if (value == DBNull.Value)
                            value = null;
                        else
                            return (T)row[nomeCampo];
                    }
                    else
                    {
                        value = row[0];
                        if (value == DBNull.Value)
                            value = null;
                        else
                            return (T)row[0];
                    }
                }
            }
            catch
            {
                throw;
            }
            return (T)value;
        }

        /// <summary>
        /// Organiza e retorna um DataView do resultado de um CommandText(SELECT)
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public DataView GetDataView(string commandText)
        {
            return new DataView(this.GetDatatable(commandText));
        }

        /// <summary>
        /// Executa SqlCommand UPDATE, DELETE, INSERT in BeginTransaction.
        /// Dessa forma é mais seguro, pois se acontecer algum erro/problema durante o processo no banco
        /// será desfeito tudo e todas as alterações feito no dados. Assim evitando de deixar os dados inconsistente.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public bool ExecuteNonQueryTransaction(SqlCommand command)
        {
            using (SqlTransaction transaction = this.GetSqlConnection().BeginTransaction(IsolationLevel.Serializable, "t1"))
            {
                try
                {
                    command.Transaction = transaction;
                    command.ExecuteNonQuery();
                    transaction.Commit();
                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Executa SqlCommand UPDATE, DELETE, INSERT in BeginTransaction.
        /// Dessa forma é mais seguro, pois se acontecer algum erro/problema durante o processo no banco
        /// será desfeito tudo e todas as alterações feito no dados. Assim evitando de deixar os dados inconsistente.
        /// </summary>
        /// <param name="commandText"></param>
        public bool ExecuteNonQueryTransaction(string commandText)
        {
            using (SqlCommand command = this.GetSqlCommand(commandText))
            {
                return this.ExecuteNonQueryTransaction(command);
            }
        }

        /// <summary>
        /// Retorna a quantidade de registros.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Int64 GetCountRegistros<T>()
        {
            return this.GetCountRegistros<T>("");
        }

        /// <summary>
        /// Retorna a quantidade de registros. (WHERE) já declarado
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public Int64 GetCountRegistros<T>(string where)
        {
            object value = 0;
            StringBuilder sb = new StringBuilder();
            where = where.Trim();

            T model = (T)Activator.CreateInstance(typeof(T));
            PropertyInfo[] properties = ((Type)model.GetType()).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                sb.Append(string.Format("SELECT COUNT({0}) FROM dbo.{1}", property.Name, model.GetType().Name));
                break;
            }
            if (!string.IsNullOrEmpty(where))
                sb.Append(" WHERE ").Append(where);
            try
            {
                using (SqlCommand command = GetSqlCommand(sb.ToString()))
                {
                    value = command.ExecuteScalar();
                }
            }
            catch
            {
                throw;
            }
            return Convert.ToInt64(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public List<T> GetListModel<T>(string commandText)
        {
            List<T> listModel = new List<T>();

            foreach (DataRow row in this.GetDataRow(commandText))
                listModel.Add(ToModel<T>(row));

            return listModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scripSqlScalar"></param>
        /// <returns></returns>
        internal Int64 ExecutaSqlScalar(string scripSqlScalar)
        {
            object obj = null;
            try
            {
                using (SqlCommand command = GetSqlCommand(scripSqlScalar))
                {
                    obj = command.ExecuteScalar();
                }
                if (obj != null)
                    return Convert.ToInt64(obj);
            }
            catch
            {
                throw;
            }
            return 0;
        }

        /// <summary>
        /// Get uma lista com resultado de um campo: ex: select PRO_CODIGO from produtos.
        /// LEMBRETE: só retornará a lista com o primeiro campo do select
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public List<T> GetList<T>(string commandText)
        {
            List<T> listObj = new List<T>();
            foreach (DataRow row in GetDataRow(commandText))
            {
                listObj.Add((T)row[0]);
            }
            return listObj;
        }

        internal SqlTransaction GetBeginTransaction(string transactionName)
        {
            return this.GetSqlConnection().BeginTransaction(IsolationLevel.Serializable, transactionName);
        }
    }
}
