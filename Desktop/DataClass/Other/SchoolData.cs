using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Desktop.DataClass.Persons;


using TableRow = System.Collections.Generic.Dictionary<string, System.IComparable>;
namespace Desktop.DataClass.Other
{
    public static class SchoolDataExtensions
    {
        public static void Add(this List<TableRow> table, Person value)
        {
            table.Add(value.AsDict());
        }
    }

    public class SchoolData
    {
        public List<TableRow> Students = new List<TableRow>();
        public List<TableRow> Teachers = new List<TableRow>();
        public List<TableRow> Employees = new List<TableRow>();
        
        
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

        public static Type GetFieldType(string tableSelected, string field)
        {
            switch (tableSelected)
            {
                case "Students":
                    return Person.GetPublicFields(typeof(Student)).Where(e => e.Name == field).ToArray()[0].GetType();
                case "Teachers":
                    return Person.GetPublicFields(typeof(Teacher)).Where(e => e.Name == field).ToArray()[0].GetType();
                case "Employees":
                    return Person.GetPublicFields(typeof(Employee)).Where(e => e.Name == field).ToArray()[0].GetType();
                default:
                    return typeof(object);
            }
        }
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