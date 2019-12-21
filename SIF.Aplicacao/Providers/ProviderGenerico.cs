using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIF.Aplicacao
{
    public class ProviderGenerico<Provider> where Provider : class, new()
    {
        public ProviderGenerico()
        {
        }
    }
}
