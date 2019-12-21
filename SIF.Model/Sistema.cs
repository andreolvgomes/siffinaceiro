using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIF.Model
{
    public class Sistema
    {
        [CamposSqlAttibutes("Sis_sequencialdb", IsIdentity = true)]
        public int Sis_sequencialdb { get; set; }
    }
}
