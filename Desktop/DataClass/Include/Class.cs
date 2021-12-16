using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Desktop.DataClass.Include
{
    public class Class : IComparable
    {
        public Dictionary<string, int> Classes;

        public int Lenght => Classes.Count;
        public Class(Dictionary<string, int> classes) =>  Classes = classes;
        public Class() => Classes = new Dictionary<string, int>();

        public int this[string index]
        {
            set => Classes[index] = value;
            get => Classes[index];
        }
        
        public override string ToString()
        {
            if (Classes.Count == 0)
                return "NULL";
            
            var ret = Classes.Aggregate("",
                (current, @class) =>
                    current + $"{@class.Key}:{@class.Value}, ");
            return ret.Substring(0, ret.Length - 2);
        }

        public int CompareTo(object obj)
        {
            if (!(obj is Class @class))
                throw new NotImplementedException();
            return string.Compare(ToString(), @class.ToString(), StringComparison.Ordinal);
        }

        public static Class FromString(string s)
        {
            var @class = new Class();

            foreach (var split in s.Split(','))
            {
                var components = split.Split(':');
                if (components.Length < 2)
                    continue;

                var className = components[0];
                if (!int.TryParse(components[1], out var hours))
                    continue;
                
                if (!@class.Classes.ContainsKey(className))
                    @class[className] = hours;
            }
            
            return @class;
        }
    }
}