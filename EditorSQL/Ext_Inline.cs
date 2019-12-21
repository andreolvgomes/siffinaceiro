using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;

namespace EditorSQL
{
    public static class Ext_Inline
    {
        public static Paragraph GetParagraph(this Inline inline)
        {
            var contentElement = ((ContentElement)inline.Parent);
            while (contentElement != null)
            {
                var p = contentElement as Paragraph;
                if (p != null)
                {
                    return p;
                }
                contentElement = ((ContentElement)inline.Parent);
            }
            return null;
        }
    }
}
