using System;
using System.Linq;
using System.Reflection;
using Desktop.DataClass.Include;

namespace Desktop.DataClass.Persons
{
    public class Teacher : Person
    {
        public string Class;
        public Subject Subjects;
        public Class Classes;
        public DateTime StartDate;
        
        public Teacher(params object[] args) : base(args)
        {
            Class = (string)args[index++];
            Subjects = (Subject)args[index++];
            Classes = (Class)args[index++];
            StartDate = (DateTime)args[index++];
        }
    }
}