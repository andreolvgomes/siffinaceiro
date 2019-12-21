using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SIF.Dao.Leitor
{
    public class LeitorDataReader// : AbsctractLeitor
    {
        //public override IEnumerable<T> GetObjModel<T>(SqlCommand command)
        //{
        //    using (SqlDataReader reader = command.ExecuteReader())
        //    {
        //        while (reader.Read())
        //        {
        //            T model = (T)Activator.CreateInstance(typeof(T));
        //            for (int j = 0; j < reader.FieldCount; j++)
        //            {
        //                PropertyInfo property = ((Type)model.GetType()).GetProperties().FirstOrDefault(c => c.Name.Equals(reader.GetName(j)));
        //                if (property != null)
        //                {
        //                    if (reader.IsDBNull(j) == false)
        //                        property.SetValue(model, reader[j], null);
        //                }
        //            }
        //            yield return model;
        //        }
        //    }
        //}
    }
}
