using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIF.Dao
{
    public class ConnectionDb : ConnectionDbAbstract
    {
        public ConnectionDb(string connectionString)
            : base(connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new Exception("A instância não pode ser vazio.");
        }

        public List<T> GetRegistros<T>() where T : IModelProperty
        {
            return GetRegistros<T>("");
        }

        public List<T> GetRegistros<T>(string commandTextWhere) where T : IModelProperty
        {
            List<T> listresult = new List<T>();
            DataTable datatable = new DataTable();
            using (CreateCommandsSql2<T> parameter = new CreateCommandsSql2<T>())
            {
                foreach (DataRow row in this.GetDataRow(parameter.GetCommandTextSelect(parameter.CommandTextSelect, commandTextWhere)))
                    listresult.Add(this.ConvertDataRowToModel<T>(parameter, row));
            }
            return listresult;
        }

        /// <summary>
        /// Convert um DataRow para o Model do tipo corrente.
        /// </summary>
        /// <param name="dataRow">Record</param>
        /// <returns></returns>
        private T ConvertDataRowToModel<T>(CreateCommandsSql2<T> parameter, DataRow dataRow) where T : IModelProperty
        {
            T model = (T)Activator.CreateInstance(typeof(T));
            foreach (KeyValuePair<string, CamposPropertyInfo> entry in parameter.ParametersFull)
            {
                if (!dataRow.IsNull(entry.Value.PropertyInfo.Name))
                    entry.Value.PropertyInfo.SetValue(model, dataRow[entry.Value.PropertyInfo.Name], null);
            }
            return model;
        }
    }
}
