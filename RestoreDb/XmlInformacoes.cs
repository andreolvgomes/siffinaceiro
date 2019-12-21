using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestoreDb
{
    public class XmlInformacoes
    {
        public string InstanciaDb { get; set; }
        public string Database { get; set; }
        public string CaminhoArquivo { get; set; }

        public string ConnectionString
        {
            get
            {
                return string.Format("Server = {0}; Database = {1}; Integrated Security = SSPI;", InstanciaDb, Database);
            }
        }
    }
}
