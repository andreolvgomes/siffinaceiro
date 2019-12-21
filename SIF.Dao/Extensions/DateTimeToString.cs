using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class DateTimeToString
    {
        public static String ConvertToString(this DateTime date)
        {
            return date.ToString("dd/MM/yyyy");
        }
    }
}
