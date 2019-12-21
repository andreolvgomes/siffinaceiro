using SIF.Dao;
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
    public class ContextoDb<T> : ContextoDbDAO<T> where T : IModelProperty
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="conexao"></param>
        public ContextoDb()
            : base(SistemaGlobal.Sis.Connection)
        {
        }
    }
}
