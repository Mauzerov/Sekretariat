using System;
using System.Reflection;

namespace Desktop.DataClass.Person
{
    public interface IFieldComparable
    {
        int CompareTo (IFieldComparable other, string field);
        IComparable this[string name] { get; }

        FieldInfo[] GetPublicFields();
        string[] GetPublicFieldsNames();
    }
}
