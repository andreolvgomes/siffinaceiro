using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIF.Aplicacao
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="model"></param>
    /// <param name="sucesso"></param>
    public delegate void ExecutaCommandsBemSucedidaEventHandler<T>(T model, bool sucesso);
}
