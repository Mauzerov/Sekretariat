using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleStringTest
{
    
    internal static class Program
    {
        public static IEnumerable<string> SplitNotInCommas(this string text, char delimiter)
        {
            return text.SplitNotInCommas(delimiter.ToString());
        }
        public static IEnumerable<string> SplitNotInCommas(this string text, string delimiter)
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
            return ret.ToArray();
        }
        public static void Main(string[] args)
        {
            var e = "Commas Are, \"The, Fucin\', Best\", Thats For Sure, Kappa, Xd".SplitNotInCommas(", ");
            foreach (var ee in e)
            {
                Console.WriteLine(ee);
            }
        }
    }
}