﻿using System;
using System.Linq;
using System.Reflection;
using Desktop.DataClass.Include;

namespace Desktop.DataClass.Person
{
    public abstract class Person : IFieldComparable
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
        
        #region IFieldComparable Implementation
        public int CompareTo(IFieldComparable other, string field)
        {
            return this[field].CompareTo(other[field]);
        }
        public IComparable this[string name]
        {
            get
            {
                foreach (var field in GetPublicFields())
                {
                    if (field.Name != name) continue;

                    if (field.GetValue(this) is IComparable fieldValue)
                        return fieldValue;
                    throw new NotSupportedException("Can't convert field to IComparable");
                }
                throw new ArgumentException("Can't find field");
            }
        }

        public FieldInfo[] GetPublicFields() => GetType().GetFields().Where(e => e.IsPublic).ToArray();
        public string[] GetPublicFieldsNames() => GetPublicFields().Select(t => t.Name).ToArray();

        #endregion
    }
}