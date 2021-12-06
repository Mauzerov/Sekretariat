using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Desktop.DataClass.Include;

namespace Desktop.DataClass.Persons
{
    public abstract class Person : IFieldComparable
    {
        public string Name, Surname, OldSurname, ParentsNames;
        public DateTime BirthDate = DateTime.MinValue;
        public string IdNumber;
        public object Photo;
        public Gender Gender;

        public Dictionary<string, IComparable> AsDict()
        {
            var dict = new Dictionary<string, IComparable>
            {
                ["UUID"] = System.Guid.NewGuid().ToString()
            };
            foreach (var field in GetPublicFieldsNames(GetType()))
            {
                dict[field] = this[field];
            }
            return dict;
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
        
        public static FieldInfo[] GetPublicFields(Type type) => type.GetFields().Where(e => e.IsPublic).ToArray();
        public static string[] GetPublicFieldsNames(Type type) => GetPublicFields(type).Select(t => t.Name).ToArray();

        #endregion
    }
}