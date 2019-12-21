using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIF
{
    public partial class Faturamentos
    {
        private string _Cli_nomeString;

        public string Cli_nomeString
        {
            get { return _Cli_nomeString; }
            set
            {
                if (_Cli_nomeString != value)
                {
                    _Cli_nomeString = value;
                    NotifyPropertyChanged("Cli_nomeString");
                }
            }
        }

        private string _Fat_sequencialString;

        public string Fat_sequencialString
        {
            get { return _Fat_sequencialString; }
            set
            {
                if (_Fat_sequencialString != value)
                {
                    _Fat_sequencialString = value;
                    NotifyPropertyChanged("Fat_sequencialString");
                }
            }
        }

        partial void OnFat_sequencialChanged()
        {
            this.Fat_sequencialString = this.Fat_sequencial.ToString();
        }
    }
}
