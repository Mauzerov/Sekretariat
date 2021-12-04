using System;
using System.Reflection;

namespace Desktop.DataClass.Persons
{
    public interface IFieldComparable
    {
        int CompareTo (IFieldComparable other, string field);
        IComparable this[string name] { get; }
    }
}
