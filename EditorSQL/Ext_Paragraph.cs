using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace EditorSQL
{
    public static class Ext_Paragraph
    {
        public static string GetText(this Paragraph paragraph)
        {
            var range = new TextRange(paragraph.ContentStart, paragraph.ContentEnd);
            return range.Text;
        }

        public static string GetTextBefore(this Paragraph paragraph, TextPointer pointer)
        {
            var range = new TextRange(paragraph.ContentStart, pointer);
            return range.Text;
        }

        public static string GetTextAfter(this Paragraph paragraph, TextPointer pointer)
        {
            var range = new TextRange(pointer, paragraph.ContentEnd);
            return range.Text;
        }
    }
}
