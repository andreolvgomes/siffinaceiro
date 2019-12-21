using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SIF.Commom
{
    public static class PegaDescricaoEnum
    {
        public static String GetDescription(this Enum enumCurrent)
        {
            try
            {
                Type type = enumCurrent.GetType();
                DescriptionAttribute[] att = { };
                if (Enum.IsDefined(type, enumCurrent))
                {
                    FieldInfo info = type.GetField(enumCurrent.ToString());
                    att = (DescriptionAttribute[])info.GetCustomAttributes(typeof(DescriptionAttribute), false);
                }
                return (att.Length > 0) ? att[0].Description ?? "" : enumCurrent.ToString();
            }
            catch
            {
            }
            return "";
        }
    }
}
