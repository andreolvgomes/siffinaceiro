using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// Para organização de script(SQL)
    /// </summary>
    public static class ListOrganizaWhere
    {
        /// <summary>
        /// Organiza um where
        /// </summary>
        /// <param name="listFilters"></param>
        /// <returns></returns>
        public static string OrganizaFiltrosWhere(this List<string> listFilters)
        {
            StringBuilder sb = new StringBuilder();
            int count = listFilters.Count;
            for (int i = 0; i < count; i++)
            {
                if (i > 0)
                    sb.Append(" AND ");
                sb.Append(listFilters[i]);
            }
            return sb.ToString();
        }
    }
}
