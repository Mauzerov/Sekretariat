using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Desktop.Include
{
    public static class StringExtensions
    {
        public static string AddSpacesBeforeCapitalized(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";
            var newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (var i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]) && text[i - 1] != ' ')
                    newText.Append(' ');
                newText.Append(text[i]);
            }
            return newText.ToString();
        }

        public static List<string> SplitNotInCommas(this string text, char delimiter)
        {
            return text.SplitNotInCommas(delimiter.ToString());
        }
        public static List<string> SplitNotInCommas(this string text, string delimiter)
        {
            var ret = new List<string>();
            var lastIndex = 0;
            var index = text.IndexOf(delimiter, StringComparison.Ordinal);
            while (index >= 0)
            {
                var subS = text.Substring(lastIndex, index - lastIndex);
                var count = subS.Count(f => f == '\"');
                if ((count & 1) == 0)
                {
                    ret.Add(subS);
                    lastIndex = index + delimiter.Length;
                }
                index = text.IndexOf(delimiter, index + delimiter.Length, StringComparison.Ordinal);
            }
            ret.Add(text.Substring(lastIndex, text.Length - lastIndex));
            return ret;
        }
    }
}