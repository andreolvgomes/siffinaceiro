using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// 
    /// </summary>
    public static class DataRowViewGetValue
    {
        /// <summary>
        /// Pega e retorna um valor específico
        /// </summary>
        /// <typeparam name="T">Tipo de dado</typeparam>
        /// <param name="row">DataRowView onde está o dado</param>
        /// <param name="nameFiled">Nome do campo</param>
        /// <returns></returns>
        public static T Get<T>(this DataRowView row, string nameFiled)
        {
            return (T)row[nameFiled];
        }

        /// <summary>
        /// 
        ///         /// <summary>
        /// Pega e retorna um valor específico
        /// </summary>
        /// <typeparam name="T">Tipo de dado</typeparam>
        /// <param name="row">DataRowView onde está o dado</param>
        /// <returns></returns>
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static T GetValues<T>(this DataRowView row, int index)
        {
            return (T)row[index];
        }
    }
}
