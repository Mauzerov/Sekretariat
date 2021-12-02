using System;

namespace Desktop.DataClass.Person
{
    public class Employee : Person
    {
        public string JobTime, Info;
        public DateTime StartDate;

        public Employee(params object[] args) : base(args)
        {
            JobTime = (string) args[index++];
            Info = (string) args[index++];
            StartDate = (DateTime) args[index++];
        }
    }
}