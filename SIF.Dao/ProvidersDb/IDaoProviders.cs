using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SIF.Dao
{
    //public abstract class IDaoProviders<T> : INotifyPropertyChanged, IDisposable where T : IModelProperty
    //{
    //    public event PropertyChangedEventHandler PropertyChanged;

    //    protected void NotifyPropertyChanged(string propertyName)
    //    {
    //        if (PropertyChanged != null)
    //            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    //    }

    //    public void Dispose()
    //    {
    //        this.Conexao.Dispose();

    //        GC.SuppressFinalize(this);
    //    }

    //    private int _index;

    //    public HashSet<T> ListaRecord { get; set; }
    //    public DataTable TableRegistros { get; set; }

    //    public ConnectioDb Conexao { get; private set; }

    //    public IDaoProviders(ConnectioDb conexao)
    //    {
    //        this.Conexao = conexao;
    //        this.ListaRecord = new HashSet<T>();
    //    }

    //    public bool Insert(T model)
    //    {
    //        using (SqlCommand command = Conexao.GetSqlConnection().CreateCommand())
    //        {
    //            command.CommandType = System.Data.CommandType.Text;
    //            using (CreateCommandsSql parameter = new CreateCommandsSql(model))
    //            {
    //                command.CommandText = parameter.GetInsert();
    //                parameter.AddParamters(command);
    //            }
    //            command.ExecuteNonQuery();

    //            ListaRecord.Add(model);

    //            _index = 0;
    //            return true;
    //        }
    //    }

    //    public bool Delete(T model)
    //    {
    //        using (SqlCommand command = Conexao.GetSqlConnection().CreateCommand())
    //        {
    //            command.CommandType = System.Data.CommandType.Text;
    //            using (CreateCommandsSql parameter = new CreateCommandsSql(model))
    //            {
    //                command.CommandText = parameter.GetDelete();
    //                parameter.AddParamtersKeys(command);
    //            }
    //            command.ExecuteNonQuery();

    //            ListaRecord.Remove(model);

    //            _index = 0;
    //            return true;
    //        }
    //    }

    //    public bool Update(T model)
    //    {
    //        return Update(model, null);
    //    }

    //    public bool Update(T model, T model_original)
    //    {
    //        using (SqlCommand command = Conexao.GetSqlConnection().CreateCommand())
    //        {
    //            command.CommandType = System.Data.CommandType.Text;

    //            using (CreateCommandsSql parameter = new CreateCommandsSql(model))
    //            {
    //                command.CommandText = parameter.GetUpdate(model_original != null);
    //                parameter.AddParamtersUpdate(command, model as IModelProperty, model_original as IModelProperty);
    //            }
    //            command.ExecuteNonQuery();
    //            return true;
    //        }
    //    }

    //    public void LoadedRecords()
    //    {
    //        LoadedRecords("");
    //    }

    //    public void LoadedRecords(string where)
    //    {
    //        try
    //        {
    //            using (SqlCommand command = Conexao.GetSqlConnection().CreateCommand())
    //            {
    //                using (CreateCommandsSql parameter = new CreateCommandsSql(((T)Activator.CreateInstance(typeof(T)))))
    //                {
    //                    command.CommandType = CommandType.Text;
    //                    command.CommandText = parameter.GetCommandTextSelect(where);

    //                    using (SqlDataAdapter adp = new SqlDataAdapter(command))
    //                    {
    //                        ListaRecord = new HashSet<T>();
    //                        TableRegistros = new DataTable();
    //                        adp.Fill(TableRegistros);

    //                        foreach (DataRow row in TableRegistros.Rows)
    //                        {
    //                            T model = (T)Activator.CreateInstance(typeof(T));

    //                            PropertyInfo[] properties = model.GetType().GetProperties(BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance);

    //                            foreach (PropertyInfo property in properties)
    //                            {
    //                                object[] obj = property.GetCustomAttributes(typeof(CamposSqlAttibutes), true);
    //                                if (obj.Length > 0)
    //                                {
    //                                    CamposSqlAttibutes att = obj[0] as CamposSqlAttibutes;
    //                                    if (att != null)
    //                                    {
    //                                        if (!row.IsNull(property.Name))
    //                                            property.SetValue(model, row[property.Name], null);
    //                                    }
    //                                }
    //                            }
    //                            ListaRecord.Add(model);
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }

    //    public void SetIndexNavigate(int index)
    //    {
    //        this._index = index;
    //    }

    //    public T First()
    //    {
    //        if (_index > 0)
    //            _index = 0;
    //        return ListaRecord.Skip(_index).FirstOrDefault();
    //    }

    //    public T Last()
    //    {
    //        if (_index < (ListaRecord.Count - 1))
    //            _index = ListaRecord.Count - 1;
    //        return ListaRecord.Skip(_index).FirstOrDefault();
    //    }

    //    public T Next()
    //    {
    //        if (_index < (ListaRecord.Count - 1))
    //            _index++;
    //        return ListaRecord.Skip(_index).FirstOrDefault();
    //    }

    //    public T Previous()
    //    {
    //        if (_index > 0)
    //            _index--;
    //        return ListaRecord.Skip(_index).FirstOrDefault();
    //    }

    //    public void Refresh(T model)
    //    {
    //        using (SqlCommand command = Conexao.GetSqlConnection().CreateCommand())
    //        {
    //            command.CommandType = CommandType.Text;
    //            using (CreateCommandsSql parameter = new CreateCommandsSql(model))
    //            {
    //                command.CommandText = parameter.GetSelectRefresh();
    //                parameter.AddParamtersKeys(command);
    //                using (SqlDataAdapter adp = new SqlDataAdapter(command))
    //                {
    //                    DataTable table = new DataTable();
    //                    adp.Fill(table);
    //                    if (table.Rows.Count > 0)
    //                    {
    //                        DataRow row = table.Rows[0];
    //                        if (row != null)
    //                        {
    //                            PropertyInfo[] properties = model.GetType().GetProperties(BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance);

    //                            foreach (PropertyInfo property in properties)
    //                            {
    //                                object[] obj = property.GetCustomAttributes(typeof(CamposSqlAttibutes), true);
    //                                if (obj.Length > 0)
    //                                {
    //                                    CamposSqlAttibutes att = obj[0] as CamposSqlAttibutes;
    //                                    if (att != null)
    //                                    {
    //                                        if (!row.IsNull(property.Name))
    //                                            property.SetValue(model, row[property.Name], null);
    //                                    }
    //                                }
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }

    //    public List<T> GetRegistrosFull()
    //    {
    //        return GetRegistrosWhere("");
    //    }

    //    public List<T> GetRegistrosWhere(string where)
    //    {
    //        List<T> listresult = new List<T>();
    //        TableRegistros = new DataTable();
    //        using (SqlCommand command = Conexao.GetSqlConnection().CreateCommand())
    //        {
    //            using (CreateCommandsSql parameter = new CreateCommandsSql((T)Activator.CreateInstance(typeof(T))))
    //            {
    //                command.CommandType = CommandType.Text;
    //                command.CommandText = parameter.GetCommandTextSelect(where);
    //                using (SqlDataAdapter adp = new SqlDataAdapter(command))
    //                {
    //                    adp.Fill(TableRegistros);
    //                }
    //            }
    //        }
    //        if (TableRegistros.Rows.Count > 0)
    //        {
    //            foreach (DataRow row in TableRegistros.Rows)
    //                listresult.Add(GetRow(row));
    //        }
    //        return listresult;
    //    }

    //    public void ExecutaCommand(string commandSql)
    //    {
    //        if (string.IsNullOrEmpty(commandSql))
    //            throw new Exception("commandSql não pode ser vazio");

    //        using (SqlCommand command = Conexao.GetSqlConnection().CreateCommand())
    //        {
    //            command.CommandType = CommandType.Text;
    //            command.CommandText = commandSql;
    //            command.ExecuteNonQuery();
    //        }
    //    }

    //    public TResult GetValueSelect<TResult>(string commandSql)
    //    {
    //        if (string.IsNullOrEmpty(commandSql))
    //            throw new Exception("commandSql não pode ser vazio");
    //        if(commandSql.IndexOf("*") != -1)
    //            throw new Exception("Não é permitido buscar todos os campos");

    //        DataTable table = new DataTable();
    //        using (SqlCommand command = Conexao.GetSqlConnection().CreateCommand())
    //        {
    //            command.CommandType = CommandType.Text;
    //            command.CommandText = commandSql;
    //            using (SqlDataAdapter adp = new SqlDataAdapter(command))
    //            {
    //                adp.Fill(table);
    //            }
    //        }
    //        if (table.Rows.Count > 0)
    //            return (TResult)table.Rows[0][0];
    //        object objIsnull = null;
    //        return (TResult)objIsnull;
    //    }

    //    private T GetRow(DataRow row)
    //    {
    //        T model = (T)Activator.CreateInstance(typeof(T));
    //        PropertyInfo[] properties = model.GetType().GetProperties(BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance);

    //        foreach (PropertyInfo property in properties)
    //        {
    //            object[] obj = property.GetCustomAttributes(typeof(CamposSqlAttibutes), true);
    //            if (obj.Length > 0)
    //            {
    //                CamposSqlAttibutes att = obj[0] as CamposSqlAttibutes;
    //                if (att != null)
    //                {
    //                    if (!row.IsNull(property.Name))
    //                        property.SetValue(model, row[property.Name], null);
    //                }
    //            }
    //        }
    //        return model;
    //    }

    //    public DataView SelectRows(string commandSql)
    //    {
    //        DataTable datatable = new DataTable();
    //        using (SqlCommand command = Conexao.GetSqlConnection().CreateCommand())
    //        {
    //            command.CommandType = CommandType.Text;
    //            command.CommandText = commandSql;
    //            using (SqlDataAdapter adp = new SqlDataAdapter(command))
    //            {
    //                adp.Fill(datatable);
    //            }
    //        }
    //        if (datatable.Rows.Count > 0)
    //            return new DataView(datatable);
    //        return null;
    //    }

    //    public DataRowView SelectRow(string commandSql)
    //    {
    //        DataView dataView = SelectRows(commandSql);
    //        if (dataView != null)
    //        {
    //            foreach (DataRowView row in dataView)
    //                return row;
    //        }
    //        return null;
    //    }

    //    public IEnumerable<DataRowView> GetRow(DataView dataView)
    //    {
    //        foreach (DataRowView row in dataView)
    //            yield return row;
    //    }

    //    public TResult GetValue<TResult>(DataRowView row, string nomecampo)
    //    {
    //        return (TResult)row[nomecampo];
    //    }

    //    public TResult GetValue<TResult>(DataRowView row, int index)
    //    {
    //        return (TResult)row[index];
    //    }
    //}
}
