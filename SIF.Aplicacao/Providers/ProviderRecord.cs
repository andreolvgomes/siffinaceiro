using SIF.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIF.Aplicacao
{
    public class ProviderRecord<T> : ProviderDbDAORecords<T> where T : IModelProperty
    {
        public ProviderRecord()
            : base(SistemaGlobal.Sis.Connection)
        {
        }
    }
}
