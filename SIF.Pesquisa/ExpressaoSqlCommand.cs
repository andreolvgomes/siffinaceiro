using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIF.Pesquisa
{
    public class ExpressaoSqlCommand
    {
        public string Fields { get; set; }
        public string From { get; set; }
        public string GroupBy { get; set; }
        public string Where { get; set; }
        public string OrderBy { get; set; }
        public string Having { get; set; }
        public string MessageError { get; set; }

        public List<CommandSqlCampos> FieldsList { get; set; }

        public ExpressaoSqlCommand()
        {
            this.FieldsList = new List<CommandSqlCampos>();

            Select = Fields = From = GroupBy = Where = OrderBy = Having = MessageError = "";
        }

        public string Select { get; set; }

        public string FieldsBase { get; set; }
    }
}
