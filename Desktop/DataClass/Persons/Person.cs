using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
                ["UUID"] = System.Guid.NewGuid()
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
            
            set {
                foreach (var field in GetPublicFields(this.GetType()))
                {
                    if (field.Name != name) continue;
                    Debug.WriteLine($"Setter:: {field.Name}");
                    field.SetValue(this, value);
                    return;
                }
                throw new ArgumentException($"Can't set field: {name}");
            }
        }

        public static Person MakeNew(string subClass)
        {
            switch (subClass.Remove(subClass.Length - 1))
            {
                case "Student":
                    return new Student();
                case "Teacher":
                    return new Teacher();
                case "Employee":
                    return new Employee();
            }

            throw new ArgumentException("Wrong SubClass Name");
        }
        public static FieldInfo[] GetPublicFields(Type type) => type.GetFields().Where(e => e.IsPublic).ToArray();
        public static string[] GetPublicFieldsNames(Type type) => GetPublicFields(type).Select(t => t.Name).ToArray();

        #endregion
    }
}