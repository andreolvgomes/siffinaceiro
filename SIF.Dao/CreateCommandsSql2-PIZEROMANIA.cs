using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SIF.Dao
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CreateCommandsSql2<T> : IDisposable where T : IModelProperty
    {
        private string _commandTextSelect;
        /// <summary>
        /// Retorna um CommandText de SELECT * FROM dbo.Table
        /// </summary>
        public string CommandTextSelect
        {
            get
            {
                if (string.IsNullOrEmpty(this._commandTextSelect)) this._commandTextSelect = this.GetCommandTextSelect();
                return this._commandTextSelect;
            }
        }

        private string _commandTextInsert;
        /// <summary>
        /// Retorna um CommandText de INSERT
        /// </summary>
        public string CommandTextInsert
        {
            get
            {
                if (string.IsNullOrEmpty(this._commandTextInsert)) this._commandTextInsert = this.GetCommandTextInsert();
                return this._commandTextInsert;
            }
        }

        private string _commandTextUpdate;
        /// <summary>
        /// Retona um CommandText de UPDATE
        /// </summary>
        public string CommandTextUpdate
        {
            get
            {
                if (string.IsNullOrEmpty(this._commandTextUpdate)) this._commandTextUpdate = this.GetCommandTextUpdate();
                return this._commandTextUpdate;
            }
        }

        private string _commandTextDelete;
        /// <summary>
        /// Retorna um CommandText de DELETE
        /// </summary>
        public string CommandTextDelete
        {
            get
            {
                if (string.IsNullOrEmpty(this._commandTextDelete)) this._commandTextDelete = this.GetCommandTextDelete();
                return this._commandTextDelete;
            }
        }

        public bool HasFieldIdentity
        {
            get
            {
                return this.ParametersFieldsIdentity.Count > 0;
            }
        }

        public Dictionary<string, CamposPropertyInfo> ParametersFull { get; private set; }
        public Dictionary<string, CamposPropertyInfo> ParametersKeys { get; private set; }
        public Dictionary<string, CamposPropertyInfo> ParametersFields { get; private set; }
        public Dictionary<string, CamposPropertyInfo> ParametersFieldsIdentity { get; private set; }

        public string NomeTabela { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public CreateCommandsSql2()
        {
            this.NomeTabela = (Activator.CreateInstance(typeof(T)) as IModelProperty).NomeTabela;

            this.ParametersFields = new Dictionary<string, CamposPropertyInfo>();
            this.ParametersFull = new Dictionary<string, CamposPropertyInfo>();
            this.ParametersKeys = new Dictionary<string, CamposPropertyInfo>();
            this.ParametersFieldsIdentity = new Dictionary<string, CamposPropertyInfo>();

            this.OrganizaParametersFields();
        }

        /// <summary>
        /// Organiza as propriedades
        /// </summary>
        private void OrganizaParametersFields()
        {
            PropertyInfo[] propertiesInfo = Activator.CreateInstance(typeof(T)).GetType().GetProperties(BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance);

            foreach (PropertyInfo propertyInfo in propertiesInfo)
            {
                object[] obj = propertyInfo.GetCustomAttributes(typeof(CamposSqlAttibutes), true);
                if (obj.Length > 0)
                {
                    CamposSqlAttibutes att = obj[0] as CamposSqlAttibutes;
                    if (att != null)
                    {
                        this.ParametersFull[att.Field] = new CamposPropertyInfo(propertyInfo, att);
                        if (att.IsIdentity)
                            this.ParametersFieldsIdentity[att.Field] = new CamposPropertyInfo(propertyInfo, att);
                        if (att.IsPrimaryKey)
                            this.ParametersKeys[att.Field] = new CamposPropertyInfo(propertyInfo, att);
                        else
                            this.ParametersFields[att.Field] = new CamposPropertyInfo(propertyInfo, att);
                    }
                }
            }
        }

        /// <summary>
        /// Organiza e retorna um select * from tabela
        /// </summary>
        /// <returns></returns>
        private string GetCommandTextSelect()
        {
            return string.Format("SELECT * FROM dbo.{0}", ((T)Activator.CreateInstance(typeof(T)) as IModelProperty).NomeTabela);
        }

        /// <summary>
        /// Organiza e retorna um CommandText de insert
        /// </summary>
        /// <returns></returns>
        private string GetCommandTextInsert()
        {
            StringBuilder sb = new StringBuilder(string.Format("INSERT INTO {0} ", NomeTabela));
            int i = 0;
            sb.Append("(");
            foreach (KeyValuePair<string, CamposPropertyInfo> entry in ParametersFull)
            {
                if (!entry.Value.Attibute.IsIdentity)
                {
                    if (i > 0)
                        sb.Append(", ");
                    sb.Append(string.Format("{0}", entry.Key));
                    i++;
                }
            }
            sb.Append(") VALUES (");
            i = 0;
            foreach (KeyValuePair<string, CamposPropertyInfo> entry in ParametersFull)
            {
                if (!entry.Value.Attibute.IsIdentity)
                {
                    if (i > 0)
                        sb.Append(", ");
                    sb.Append(string.Format("@{0}", entry.Key));
                    i++;
                }
            }
            sb.Append(")");
            return sb.ToString();
        }

        /// <summary>
        /// Organiza e retorna um CommandText de UPDATE
        /// </summary>
        /// <returns></returns>
        private string GetCommandTextUpdate()
        {
            StringBuilder sb = new StringBuilder(string.Format("UPDATE dbo.{0} SET ", NomeTabela));
            int index = 0;
            foreach (KeyValuePair<string, CamposPropertyInfo> entry in ParametersFull)
            {
                if (!entry.Value.Attibute.IsIdentity)
                {
                    if (index > 0)
                        sb.Append(", ");
                    /// Pro_codigo = @Pro_codigo
                    /// 
                    sb.Append(string.Format("{0} = @{0} ", entry.Key));
                    index++;
                }                
            }
            /// organiza WHERE
            /// 
            index = 0;
            if (ParametersKeys.Count > 0)
            {
                sb.Append(" WHERE ");
                foreach (KeyValuePair<string, CamposPropertyInfo> entry in ParametersKeys)
                {
                    if (index > 0)
                        sb.Append("AND ");
                    /// Pro_codigo1 = @Key1
                    /// Pro_codigo2 = @Key2
                    sb.Append(string.Format("{0} = @Key{1} ", entry.Key, index));
                    index++;
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Organiza e retorna um CommandText de DELETE
        /// </summary>
        /// <returns></returns>
        private string GetCommandTextDelete()
        {
            StringBuilder sb = new StringBuilder(string.Format("DELETE FROM dbo.{0} WHERE ", NomeTabela));
            int index = 0;
            foreach (KeyValuePair<string, CamposPropertyInfo> entry in ParametersKeys)
            {
                if (index > 0)
                    sb.Append("AND ");
                sb.Append(string.Format("{0} = @{0}", entry.Key));
                index++;
            }
            return sb.ToString();
        }

        /// <summary>
        /// Adiciona os SqlParameter com seus respectivos valores para que seja possível efetuar o insert
        /// </summary>
        /// <param name="command"></param>
        /// <param name="model"></param>
        internal void AddParametersInsert(System.Data.SqlClient.SqlCommand command, IModelProperty model)
        {
            command.Parameters.Clear();

            foreach (KeyValuePair<string, CamposPropertyInfo> entry in ParametersFull)
            {
                if (!entry.Value.Attibute.IsIdentity)
                {
                    SqlParameter parameter = new SqlParameter(string.Format("@{0}", entry.Key), GetValueModel(entry.Value.PropertyInfo, model));
                    if (entry.Value.PropertyInfo.PropertyType.Name == "Byte[]")
                        parameter.SqlDbType = System.Data.SqlDbType.Image;
                    command.Parameters.Add(parameter);
                }
            }
        }

        /// <summary>
        /// Adiciona os SqlParameter com seus respectivos valores para que seja possível efetuar o update com sucesso
        /// </summary>
        /// <param name="command"></param>
        /// <param name="model"></param>
        /// <param name="model_old"></param>
        internal void AddParametersUpdate(System.Data.SqlClient.SqlCommand command, IModelProperty model, IModelProperty model_old)
        {
            int i = 0;
            foreach (KeyValuePair<string, CamposPropertyInfo> entry in ParametersFull)
            {
                if (!entry.Value.Attibute.IsIdentity)
                {
                    SqlParameter parameter = new SqlParameter(string.Format("@{0}", entry.Key), GetValueModel(entry.Value.PropertyInfo, model));
                    if (entry.Value.PropertyInfo.PropertyType.Name == "Byte[]")
                        parameter.SqlDbType = System.Data.SqlDbType.Image;
                    command.Parameters.Add(parameter);
                }
            }
            /// organiza WHERE
            /// 
            foreach (KeyValuePair<string, CamposPropertyInfo> entry in ParametersKeys)
            {
                command.Parameters.AddWithValue(string.Format("@Key{0}", i), GetValueModel(entry.Value.PropertyInfo, model_old));
                i++;
            }
        }

        /// <summary>
        /// Adiciona os SqlParameter com seus respectivos valors para que seja possível efetuar o delete com sucesso
        /// </summary>
        /// <param name="command"></param>
        /// <param name="model"></param>
        internal void AddParametersDelete(SqlCommand command, IModelProperty model)
        {
            foreach (KeyValuePair<string, CamposPropertyInfo> entry in ParametersKeys)
                command.Parameters.AddWithValue(string.Format("@{0}", entry.Key), GetValueModel(entry.Value.PropertyInfo, model));
        }

        /// <summary>
        /// Pega e retorna o Value de IModelProperty
        /// </summary>
        /// <param name="propertyInfo">PropertyInfo do model</param>
        /// <param name="model">Modelo</param>
        /// <returns></returns>
        public object GetValueModel(PropertyInfo propertyInfo, IModelProperty model)
        {
            object value = propertyInfo.GetValue(model, null) ?? DBNull.Value;
            if (propertyInfo.PropertyType.Name.ToUpper() == "STRING")
            {
                if (value.ToString() == string.Empty)
                    return "";
            }
            return value;
        }

        internal string GetWhereDataRow(IModelProperty model)
        {
            StringBuilder sb = new StringBuilder();
            int index = 0;
            foreach (KeyValuePair<string, CamposPropertyInfo> entry in ParametersKeys)
            {
                if (index > 0)
                    sb.Append("AND ");
                sb.Append(string.Format("{0} = {1}", entry.Key, GetValueModel(entry.Value.PropertyInfo, model)));
                index++;
            }
            return sb.ToString();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        internal string GetCommandTextSelect(string commandTextSelect, string commandTextWhere)
        {
            if (!string.IsNullOrEmpty(commandTextWhere))
            {
                commandTextWhere = commandTextWhere.ToUpper().Replace("WHERE", "").Trim();
                commandTextSelect = string.Format("{0} WHERE {1}", commandTextSelect, commandTextWhere);
            }
            return commandTextSelect;
        }
    }
}
