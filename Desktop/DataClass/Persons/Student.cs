using System;
using System.Linq;
using System.Reflection;
using Desktop.DataClass.Include;

namespace Desktop.DataClass.Persons
{
    public class Student : Person
    {
        public string Class;
        public SchoolGroup Group;

        public Student()
        {
        }

        public Student(params object[] args) : base(args)
        {
            Class = (string)args[index++];
            Group = (SchoolGroup)args[index++];
        }
    }
}