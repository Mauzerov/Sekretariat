using System;
using System.Linq;
using System.Reflection;
using Desktop.DataClass.Include;

namespace Desktop.DataClass.Persons
{
    public partial class Teacher : Person
    {
        public string Class;
        public string Subjects;
        public Class Classes;
        public DateTime StartDate;
    }
}