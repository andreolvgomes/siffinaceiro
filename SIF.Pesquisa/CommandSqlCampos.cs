using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIF.Pesquisa
{
    public class CommandSqlCampos
    {
        public string NameFieldCompleto { get; set; }
        public string NameField { get; set; }
        public string ApelidoField { get; set; }
        public string FullName
        {
            get
            {
                if (NameFieldCompleto.IndexOf(".") != -1)
                {
                    string[] str = NameFieldCompleto.Split('.');
                    return string.Format("[{0}].[{1}]", str[0], str[1]);
                }
                return string.Format("[{0}]", NameField);
            }
        }
    }
}
