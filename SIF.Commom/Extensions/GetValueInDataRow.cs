using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class GetValueInDataRow
    {
        /// <summary>
        /// Get Value in DataRow no index 0
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <returns></returns>
        public static T Get<T>(this DataRow row)
        {
            return row.Get<T>(0);
        }

        /// <summary>
        /// Get Value in DataRow pelo nome da coluna
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static T Get<T>(this DataRow row, string columnName)
        {
            return (T)row[columnName];
        }

        /// <summary>
        /// Get value in DataRow pelo index da coluna
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        public static T Get<T>(this DataRow row, int columnIndex)
        {
            return (T)row[columnIndex];
        }
    }
}
