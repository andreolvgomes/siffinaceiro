using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace SIF.Pesquisa
{
    public class CamposTipo
    {
        public CommandSqlCampos CamposBase { get; set; }
        public string Campo { get; set; }
        public Type Type { get; set; }
    }

    public class OptionColumnInfo
    {
        public DataGridColumn Column { get; set; }
        public string PropertyPath { get; set; }
        public IValueConverter Converter { get; set; }
        public object ConverterParameter { get; set; }
        public System.Globalization.CultureInfo ConverterCultureInfo { get; set; }
        public Type PropertyType { get; set; }
        public CommandSqlCampos CamposBase { get; private set; }

        public OptionColumnInfo(DataGridColumn column, List<CamposTipo> typesFields)
        {
            if (column == null)
                return;

            Column = column;
            var boundColumn = column as DataGridBoundColumn;
            if (boundColumn != null)
            {
                System.Windows.Data.Binding binding = boundColumn.Binding as System.Windows.Data.Binding;
                if (binding != null)
                {
                    PropertyPath = binding.Path.Path;
                    if (typesFields.Count > 0)
                    {
                        CamposTipo camposType = typesFields.FirstOrDefault(c => c.Campo == PropertyPath);
                        PropertyType = camposType.Type;
                        CamposBase = camposType.CamposBase;
                    }
                }
            }
        }
    }
}
