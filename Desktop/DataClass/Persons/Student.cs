using System;
using System.Linq;
using System.Reflection;
using Desktop.DataClass.Include;

namespace Desktop.DataClass.Persons
{
    public partial class Student : Person
    {
        public string Class;
        public SchoolGroup Group;
    }
}