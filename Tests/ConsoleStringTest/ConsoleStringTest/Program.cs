using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleStringTest
{
    
    internal static class Program
    {
        public static IEnumerable<string> SplitNotInCommas(this string text)
        {
            var ret = new List<string>();
            var lastIndex = 0;
            var index = text.IndexOf(',');
            while (index >= 0)
            {
                var subS = text.Substring(lastIndex, index - lastIndex + 1);
                var count = subS.Count(f => f == '\"');
                if ((count & 1) == 0)
                {
                    ret.Add(subS);
                    lastIndex = index + 1;
                }
                index = text.IndexOf(',', index + 1);
            }
            ret.Add(text.Substring(lastIndex, text.Length - lastIndex));
            return ret.ToArray();
        }
        public static void Main(string[] args)
        {
            var e = "Commas Are,\"The, Fucin\', Best\", Thats For Sure, Kappa, Xd".SplitNotInCommas();
            foreach (var ee in e)
            {
                Console.WriteLine(ee);
            }
        }
    }
}