using System.Collections.Generic;
using System.Linq;

namespace Desktop.DataClass.Include
{
    public class Class
    {
        public Dictionary<string, int> Classes;

        public int Lenght => Classes.Count;
        public int this[string c] => Classes[c];

        public Class(Dictionary<string, int> classes)
        {
            Classes = classes;
        }

        public override string ToString()
        {
            if (Classes.Count == 0)
                return "NULL";
            
            var ret = Classes.Aggregate("", (current, @class) => current + (@class + ", "));
            return ret.Substring(0, ret.Length - 2);
        }
    }
}