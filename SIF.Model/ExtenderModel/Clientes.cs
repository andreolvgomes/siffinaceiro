using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIF
{
    public partial class Clientes
    {
        private PublicList _listaCommom;
        public PublicList ListaCommom
        {
            get
            {
                if (_listaCommom == null)
                    return new PublicList();

                return _listaCommom;
            }
        }

        private string _datNascimento;
        public string DataNascimento
        {
            get { return _datNascimento; }
            set
            {
                if (_datNascimento != value)
                {
                    _datNascimento = value;
                    NotifyPropertyChanged("DataNascimento");
                }
            }
        }

        partial void NotifyPropertyChanged_Cli_tipopessoa()
        {
            TipoPessoa = (TipoPessoa)Cli_tipopessoa;
        }

        private TipoPessoa _tipoPessoa;
        public TipoPessoa TipoPessoa
        {
            get { return _tipoPessoa; }
            set 
            {
                if (_tipoPessoa != value)
                {
                    _tipoPessoa = value;

                    Cli_tipopessoa = (int)_tipoPessoa;
                    NotifyPropertyChanged("TipoPessoa");
                }
            }
        }
    }
}
