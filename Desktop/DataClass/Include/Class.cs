using System.Collections.Generic;

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
    }
}