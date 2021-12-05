using System;
using System.Linq;
using System.Reflection;

namespace Desktop.DataClass.Persons
{
    public class Employee : Person
    {
        public string JobTime, Info;
        public DateTime StartDate;
        
        public Employee() {}
        public Employee(params object[] args) : base(args)
        {
            JobTime = (string) args[index++];
            Info = (string) args[index++];
            StartDate = (DateTime) args[index++];
        }
    }
}