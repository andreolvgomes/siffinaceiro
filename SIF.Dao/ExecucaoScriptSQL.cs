using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIF.Dao
{
    public class ExecucaoScriptSQL
    {
        public ExecucaoScriptSQL()
        {
        }

        //public void ExecutaScriptAutomatico(string instancia, string banco)
        //{
        //    ExecutaScriptAutomatico(instancia, banco, "", "");
        //}

        public void CreateDatabseIfNotExist(string instancia, string banco, string usuario, string senha)
        {
            string _script = System.IO.Path.Combine(Environment.CurrentDirectory, "Scriptdatabase.sql");
            if (System.IO.File.Exists(_script))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(string.Format(" -S {0}", instancia));
                sb.Append(string.Format(" -d {0}", banco));
                sb.Append(string.Format(" -i {0}", _script));
                /// add autentication
                /// 
                if (!string.IsNullOrEmpty(usuario) && !string.IsNullOrEmpty(senha))
                {
                    sb.Append(string.Format(" -U {0}", usuario));
                    sb.Append(string.Format(" -P {0}", senha));
                }

                System.Diagnostics.ProcessStartInfo processInf = new System.Diagnostics.ProcessStartInfo()
                {
                    Arguments = sb.ToString(),
                    WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    FileName = System.IO.Path.Combine(Environment.CurrentDirectory, "OSQL.exe")
                };
                System.Diagnostics.Process.Start(processInf);
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}
