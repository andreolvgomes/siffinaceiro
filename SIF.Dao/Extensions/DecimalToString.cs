using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class DecimalToString
    {
        public static String ConvertToString(this decimal value)
        {
            return value.ToString("N2").Replace(".", "");
        }
    }
}
