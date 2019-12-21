using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIF
{
    public class PublicList : IDisposable
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<string> ListUFs
        {
            get
            {
                return new List<string>() { "AC", "AL", "AP", "AM", "BA", "CE", "DF", "ES", "GO", "MA", "MT", "MS", "MG", "PR", "PB", "PA", "PE", "PI", "RJ", "RN", "RS", "RO", "RR", "SC", "SE", "SP", "TO" };
            }
        }

        public List<string> ListSexo
        {
            get
            {
                return new List<string>() { "MASCULINO", "FEMININO" };
            }
        }

        public List<string> ListEstadoCivil
        {
            get
            {
                return new List<string>() { "SOLTEIRO", "CASADO", "SEPARADO", "DIVORCIADO", "VIÚVO" };
            }
        }
    }
}
