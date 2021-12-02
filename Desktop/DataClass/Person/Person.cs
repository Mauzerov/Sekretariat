using System;
using Desktop.DataClass.Include;

namespace Desktop.DataClass.Person
{
    public abstract class Person
    {
        public string Name, Surname, OldSurname, ParentsNames;
        public DateTime BirthDate;
        public string Id;
        public object Photo;
        public Gender Gender;
        protected int index = 0;
        protected Person(params object[] args)
        {
            Name = (string) args[index++];
            Surname = (string) args[index++];
            OldSurname = (string) args[index++];
            ParentsNames = (string) args[index++];
            BirthDate = (DateTime) args[index++];
            Photo = args[index++];
            Gender = (Gender) args[index++];
        }
    }
}