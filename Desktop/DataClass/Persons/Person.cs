using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Desktop.DataClass.Include;

namespace Desktop.DataClass.Persons
{
    public abstract class Person : IFieldComparable
    {
        public string Name, Surname, FamilyName, ParentsNames;
        public DateTime BirthDate = DateTime.MinValue.Date;
        public string IdNumber;
        public object Photo;
        public Gender Gender;

        public Dictionary<string, IComparable> AsDict()
        // Inheritable -> Adds Inherited Fields As Well
        {
            // Generate new Unique Id For Each Person 
            var dict = new Dictionary<string, IComparable> { ["UUID"] = Guid.NewGuid() };
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
                foreach (var field in GetPublicFields(GetType()))
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
                    //Debug.WriteLine($"Setter:: {field.Name}");
                    field.SetValue(this, value);
                    return;
                }
                throw new ArgumentException($"Can't set field: {name}");
            }
        }

        public static Person MakeNew(string subClass)
        {
            switch (subClass)
            {
                case "Students":
                case "Student":
                    return new Student();
                case "Teachers":
                case "Teacher":
                    return new Teacher();
                case "Employees":
                case "Employee":
                    return new Employee();
            }
            throw new ArgumentException("Wrong SubClass Name");
        }
        public static IEnumerable<FieldInfo> GetPublicFields(Type type) => type.GetFields().Where(e => e.IsPublic).ToArray();
        public static IEnumerable<string> GetPublicFieldsNames(Type type) => GetPublicFields(type).Select(t => t.Name).ToArray();

        #endregion
    }
}