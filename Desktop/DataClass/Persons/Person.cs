using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Desktop.DataClass.Include;

namespace Desktop.DataClass.Persons
{
    public abstract class Person : IFieldComparable
    {
        public string Name, Surname, OldSurname, ParentsNames;
        public DateTime BirthDate;
        public string Id;
        public object Photo;
        public Gender Gender;
        protected int index = 0;

        protected Person()
        {
            
        }
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
                foreach (var field in GetPublicFields(this.GetType()))
                {
                    if (field.Name != name) continue;

                    if (field.GetValue(this) is IComparable fieldValue)
                        return fieldValue;
                    return "NULL";
                }
                throw new ArgumentException("Can't find field");
            }
        }

        public IEnumerable<IComparable> GetFieldsAsList(IEnumerable<string> fields)
        {
            IEnumerable<IComparable> some = new List<string>();
            return fields.Aggregate(some, (current, field) => current.Append(this[field]));
        }

        public static FieldInfo[] GetPublicFields(Type type) => type.GetFields().Where(e => e.IsPublic).ToArray();
        public static string[] GetPublicFieldsNames(Type type) => GetPublicFields(type).Select(t => t.Name).ToArray();

        #endregion
    }
}