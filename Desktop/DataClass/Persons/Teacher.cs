using System;
using System.Linq;
using System.Reflection;
using Desktop.DataClass.Include;

namespace Desktop.DataClass.Persons
{
    public partial class Teacher : Person
    {
        public string Klasa;
        public string Przedmioty;
        public Class Klasy;
        public DateTime DataRozpoczecia;
    }
}