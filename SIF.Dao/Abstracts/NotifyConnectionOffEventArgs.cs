using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIF.Dao.Abstracts
{
    public class NotifyConnectionOffEventArgs : EventArgs
    {
        public string DataSource { get; set; }
        public Exception Excecao { get; set; }

        public NotifyConnectionOffEventArgs(string datasource, Exception ex)
        {
        }
    }
}
