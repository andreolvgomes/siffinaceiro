using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIF.Aplicacao
{
    /// <summary>
    /// Para execuções ExecucaoCommandEventHandler
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="model"></param>
    public delegate void ExecucaoCommandEventHandler<T>(T model);
}
