using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Documents;

namespace EditorSQL
{
    public interface IParagraphProcessor
    {
        Regex SplitWordsRegex { get; }
        int GetWordTypeID(string word);
        Inline FormatInlineForID(Inline inline, int id);
    }
}
