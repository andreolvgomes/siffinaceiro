using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SIF.Dao.Leitor
{
    public class LeitorDatable //: AbsctractLeitor
    {
        //public override IEnumerable<T> GetObjModel<T>(SqlCommand command)
        //{
        //    DataTable table = new DataTable();
        //    using (SqlDataAdapter adp = new SqlDataAdapter(command))
        //    {
        //        adp.Fill(table);
        //        foreach (DataRow row in table.Rows)
        //        {
        //            T model = (T)Activator.CreateInstance(typeof(T));

        //            foreach (PropertyInfo property in ((Type)model.GetType()).GetProperties())
        //            {
        //                property.SetValue(model, row[property.Name], null);
        //            }

        //            yield return model;
        //        }
        //    }
        //}
    }
}
