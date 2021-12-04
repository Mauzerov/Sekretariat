using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Desktop.DataClass.Persons;

namespace Desktop.DataClass.Other
{
    public class SchoolData
    {
        public List<Student> Students = new List<Student>();
        public List<Teacher> Teachers = new List<Teacher>();
        public List<Employee> Employees = new List<Employee>();

        private string[] Tables = null;
        public IEnumerable<string> GetTables() => Tables;

        public SchoolData()
        {
            Tables = GetPublicFieldsNames();
        }
        
        public List<Teacher> this[string name]
        {
            get
            {
                Debug.Assert(Tables.Contains(name), $"Unable To Find \'{name}\'Table");
                foreach (var field in GetPublicFields())
                {
                    if (field.Name != name) continue;

                    if (field.GetValue(this) is IList fieldValue)
                        return fieldValue.Cast<Teacher>().ToList();
                    throw new NotSupportedException("Can't convert field to List<Person>");
                }
                throw new ArgumentException("Can't find field");
            }
        }
        private string[] GetPublicFieldsNames() => GetPublicFields().Select(t => t.Name).ToArray();
        private FieldInfo[] GetPublicFields() => GetType().GetFields().Where(e => e.IsPublic).ToArray();

        public static IEnumerable<string> GetMemberPublicFieldsNames(string tableSelected)
        {
            /* I don't want to think about this           */
            /* C# Doesn't Allow Compile Time Type Casting */
            switch (tableSelected)
            {
                case "Students":
                    return Person.GetPublicFieldsNames(typeof(Student));
                case "Teachers":
                    return Person.GetPublicFieldsNames(typeof(Teacher));
                case "Employees":
                    return Person.GetPublicFieldsNames(typeof(Employee));
                default:
                    return new string[] { };
            }
        }
    }
}