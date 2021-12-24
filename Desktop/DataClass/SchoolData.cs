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
        
        public List<TableRow> this[string name]
        {
            get
            {
                foreach (var field in GetPublicFields())
                {
                    if (!field.Name.StartsWith(name)) continue;

                    if (field.GetValue(this) is List<TableRow> fieldValue)
                        return fieldValue;
                    throw new NotSupportedException("Can't convert field to List<Person>");
                }
                throw new ArgumentException("Can't find field");
            }
            set
            {
                foreach (var field in GetPublicFields())
                {
                    if (!field.Name.StartsWith(name)) continue;

                    if (field.GetValue(this) is List<TableRow> fieldValue)
                    {
                        field.SetValue(this, value);
                        return;
                    }
                    //throw new NotSupportedException("Can't convert field to List<Person>");
                }
                throw new ArgumentException("Can't find field");
            }
        }

        private string[] GetPublicFieldsNames() => GetPublicFields().Select(t => t.Name).ToArray();
        private FieldInfo[] GetPublicFields() => GetType().GetFields().Where(e => e.IsPublic).ToArray();

        public static Type GetFieldType(string tableSelected, string field)
        {
            switch (tableSelected.ToLower())
            {
                case "students":
                case "student":
                    return Person.GetPublicFields(typeof(Student)).Where(e => e.Name == field).ToArray()[0].FieldType;
                case "teachers":
                case "teacher":
                    return Person.GetPublicFields(typeof(Teacher)).Where(e => e.Name == field).ToArray()[0].FieldType;
                case "employees":
                case "employee":
                    return Person.GetPublicFields(typeof(Employee)).Where(e => e.Name == field).ToArray()[0].FieldType;
                default:
                    return typeof(object);
            }
        }
        public static IEnumerable<string> GetMemberPublicFieldsNames(string tableSelected)
        {
            /* I don't want to think about this           */
            /* C# Doesn't Allow Compile Time Type Casting */
            switch (tableSelected.ToLower())
            {
                case "students":
                case "student":
                    return Person.GetPublicFieldsNames(typeof(Student));
                case "teachers":
                case "teacher":
                    return Person.GetPublicFieldsNames(typeof(Teacher));
                case "employees":
                case "employee":
                    return Person.GetPublicFieldsNames(typeof(Employee));
                default:
                    return new string[] { };
            }
        }
    }
}