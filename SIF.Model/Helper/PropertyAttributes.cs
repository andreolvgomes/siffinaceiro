using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIF
{
    //[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class CamposSqlAttibutes : Attribute
    {
        public bool IsPrimaryKey { get; set; }
        public bool IsIdentity { get; set; }

        private string _field;
        public string Field
        {
            get
            {
                return _field;
            }
        }        

        public CamposSqlAttibutes(string field)
        {
            this._field = field;
        }
    }
}
