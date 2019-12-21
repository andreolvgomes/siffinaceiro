using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SIF.Dao
{
    public class CamposPropertyInfo
    {
        public PropertyInfo PropertyInfo { get; set; }
        public CamposSqlAttibutes Attibute { get; set; }

        public CamposPropertyInfo(PropertyInfo property, CamposSqlAttibutes att)
        {
            this.PropertyInfo = property;
            this.Attibute = att;
        }
    }

    public class CreateCommandsSql : IDisposable
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        private Dictionary<string, CamposPropertyInfo> ParametersFull { get; set; }
        private Dictionary<string, CamposPropertyInfo> ParametersKeys { get; set; }
        private Dictionary<string, CamposPropertyInfo> ParametersFields { get; set; }

        private IModelProperty _modelExample;

        public CreateCommandsSql(IModelProperty model)
        {
            this._modelExample = model;
            OrganizaParametersFields();
        }

        private void OrganizaParametersFields()
        {
            PropertyInfo[] properties = _modelExample.GetType().GetProperties(BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance);
            ParametersFields = new Dictionary<string, CamposPropertyInfo>();
            ParametersFull = new Dictionary<string, CamposPropertyInfo>();
            ParametersKeys = new Dictionary<string, CamposPropertyInfo>();

            foreach (PropertyInfo property in properties)
            {
                object[] obj = property.GetCustomAttributes(typeof(CamposSqlAttibutes), true);
                if (obj.Length > 0)
                {
                    CamposSqlAttibutes att = obj[0] as CamposSqlAttibutes;
                    if (att != null)
                    {
                        ParametersFull[att.Field] = new CamposPropertyInfo(property, att);

                        if (att.IsPrimaryKey)
                            ParametersKeys[att.Field] = new CamposPropertyInfo(property, att);
                        else
                            ParametersFields[att.Field] = new CamposPropertyInfo(property, att);
                    }
                }
            }
        }

        public string GetCommandTextSelect()
        {
            return GetCommandTextSelect("");
        }

        public string GetCommandTextSelect(string where)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ");
            int i = 0;
            foreach (KeyValuePair<string, CamposPropertyInfo> entry in ParametersFull)
            {
                if (i > 0)
                    sb.Append(", ");
                sb.Append(entry.Key);
                i++;
            }

            sb.Append(" FROM ").Append(_modelExample.NomeTabela);
            if (!string.IsNullOrEmpty(where.Trim()))
            {
                if (where.ToUpper().IndexOf("WHERE") != -1)
                    where = where.Replace("Where", "");
                sb.Append(" WHERE ").Append(where);
            }
            return sb.ToString();
        }

        internal string GetInsert()
        {
            StringBuilder sb = new StringBuilder(string.Format("INSERT INTO {0} ", _modelExample.NomeTabela));
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

        internal string GetDelete()
        {
            StringBuilder sb = new StringBuilder("DELETE FROM ");
            sb.Append(_modelExample.NomeTabela);

            int i = 0;
            List<string> ls = (from x in ParametersFull where x.Value.Attibute.IsPrimaryKey select x.Key).ToList();
            if (ls.Count > 0)
            {
                i = 0;
                sb.Append(" WHERE ");
                foreach (string campo in ls)
                {
                    if (i > 0)
                        sb.Append("AND ");
                    sb.Append(string.Format("{0} = @{0}", campo));
                }
            }
            return sb.ToString();
        }

        internal string GetSelectRefresh()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ");
            int i = 0;
            foreach (KeyValuePair<string, CamposPropertyInfo> entry in ParametersFull)
            {
                if (i > 0)
                    sb.Append(", ");
                sb.Append(entry.Key);
                i++;
            }

            sb.Append(" FROM ").Append(_modelExample.NomeTabela);

            foreach (KeyValuePair<string, CamposPropertyInfo> entry in ParametersKeys)
            {
                i = 0;
                sb.Append(" WHERE ");
                if (i > 0)
                    sb.Append("AND ");
                sb.Append(string.Format("{0} = @{0}", entry.Key));
            }
            return sb.ToString();
        }

        public object GetValue(PropertyInfo property, IModelProperty entidade)
        {
            object value = property.GetValue(entidade, null) ?? DBNull.Value;
            if (property.PropertyType.Name.ToUpper() == "STRING")
            {
                if (value.ToString() == string.Empty)
                    return "";
            }
            return value;
        }

        public object GetValue(PropertyInfo property)
        {
            object value = property.GetValue(_modelExample, null) ?? DBNull.Value;
            if (property.PropertyType.Name.ToUpper() == "STRING")
            {
                if (value.ToString() == string.Empty)
                    return "";
            }
            return value;
        }        

        internal string GetUpdate(bool alterKey)
        {
            StringBuilder sb = new StringBuilder(string.Format("UPDATE {0} SET ", _modelExample.NomeTabela));
            int i = 0;
            foreach (KeyValuePair<string, CamposPropertyInfo> entry in ParametersFull)
            {
                if (!entry.Value.Attibute.IsIdentity)
                {
                    if (i > 0)
                        sb.Append(", ");
                    sb.Append(string.Format("{0} = @{0}", entry.Key));
                    i++;
                }
            }
            if (!alterKey)
            {
                List<string> ls = (from x in ParametersFull where x.Value.Attibute.IsPrimaryKey select x.Key).ToList();
                if (ls.Count > 0)
                {
                    i = 0;
                    sb.Append(" WHERE ");
                    foreach (string campo in ls)
                    {
                        if (i > 0)
                            sb.Append(" AND ");
                        sb.Append(string.Format("{0} = @{0}", campo));
                        i++;
                    }
                }
            }
            else
            {
                i = 0;
                sb.Append(" WHERE ");
                foreach (KeyValuePair<string, CamposPropertyInfo> entry in ParametersKeys)
                {
                    if (i > 0)
                        sb.Append(" AND ");
                    sb.Append(string.Format("{0} = @Key{1}", entry.Key, i));
                    i++;
                }
            }

            return sb.ToString();
        }

        internal void AddParamters(SqlCommand command)
        {
            AddParamters(command, false);
        }

        internal void AddParamters(SqlCommand command, bool addIdentity)
        {
            bool prossegue = true;
            foreach (KeyValuePair<string, CamposPropertyInfo> entry in ParametersFull)
            {
                prossegue = true;
                if (!addIdentity && entry.Value.Attibute.IsIdentity)
                    prossegue = false;
                if (prossegue)
                    if (!JaAdd(command, entry.Key))
                    {
                        SqlParameter parameter = new SqlParameter(string.Format("@{0}", entry.Key), GetValue(entry.Value.PropertyInfo));
                        if (entry.Value.PropertyInfo.PropertyType.Name == "Byte[]")
                        {
                            parameter.SqlDbType = System.Data.SqlDbType.Image;
                        }
                        command.Parameters.Add(parameter);
                    }
            }
        }

        private bool JaAdd(SqlCommand command, string filed)
        {
            filed = "@" + filed;
            return ((from x in command.Parameters.Cast<SqlParameter>() where x.ParameterName == filed select x).FirstOrDefault() != null);
        }

        internal void AddParamtersKeys(SqlCommand command)
        {
            foreach (KeyValuePair<string, CamposPropertyInfo> entry in ParametersKeys)
            {
                command.Parameters.Add(new SqlParameter(string.Format("@{0}", entry.Key), GetValue(entry.Value.PropertyInfo)));
            }
        }

        internal void AddParamtersUpdate(SqlCommand command, IModelProperty entidade, IModelProperty entidade_original)
        {
            int i = 0;
            foreach (KeyValuePair<string, CamposPropertyInfo> entry in ParametersFull)
            {
                command.Parameters.AddWithValue(string.Format("@{0}", entry.Key), GetValue(entry.Value.PropertyInfo));
            }
            if (entidade_original != null)
            {
                foreach (KeyValuePair<string, CamposPropertyInfo> entry in ParametersKeys)
                {
                    command.Parameters.AddWithValue(string.Format("@Key{0}", i), GetValue(entry.Value.PropertyInfo, entidade_original));
                    i++;
                }
            }
        }
    }
}
