using Microsoft.Data.Schema.ScriptDom.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIF.Pesquisa
{
    internal static class TSqlFragmentGetString
    {
        public static string GetString(this TSqlFragment statement)
        {
            string s = string.Empty;
            if (statement == null) return string.Empty;

            for (int i = statement.FirstTokenIndex; i <= statement.LastTokenIndex; i++)
            {
                s += statement.ScriptTokenStream[i].Text;
            }

            return s;
        }
    }
}
