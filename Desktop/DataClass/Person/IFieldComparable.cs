using System;

namespace Desktop.DataClass.Person
{
    public interface IFieldComparable
    {
        int CompareTo (IFieldComparable other, string field);
        IComparable this[string name] { get; }
    }
}