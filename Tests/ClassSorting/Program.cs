using System;
using System.Collections;
using System.Collections.Generic;

namespace ClassSorting
{
    internal static class Where
    {
        public static int CompareTo(this int value, Where where)
        {
            
        }
    }
    internal class FQL<T> : IEnumerable<T> where T : IFieldComparable
    /* FQL - Functional Query Language */
    {
        class Where
        {
            public string Key;
            public IComparable<T> Value;
            
        }

        private readonly IEnumerable<T> data;

        public FQL(IEnumerable<T> array)
        {
            data = array;
        }
        public IEnumerator<T> GetEnumerator()
        {
            return data.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public FQL<T> Filter(IEnumerable<string> array, IEnumerable<Where> wheres)
        {
            bool Good(T element)
            {
                foreach (var where in wheres)
                    if (!(element[where.Key].CompareTo(where.Value) == where.Other))
                        return false;
                return true;
            }
        }
    }

    internal interface IFieldComparable
    {
        int CompareTo (IFieldComparable other, string field);
        IComparable this[string name] { get; }
    }
    
    internal class Person : IFieldComparable
    {
        public string Name;
        public string Surname;
        public uint Age;

        public IComparable this[string name] {
            get
            {
                var fields = this.GetType()
                    .GetFields();

                foreach (var field in fields)
                {
                    if (field.Name != name) continue;

                    if (field.GetValue(this) is IComparable fieldValue)
                        return fieldValue;
                    throw new NotSupportedException("Can't convert field to IComparable");
                }
                throw new ArgumentException("Can't find field");
            }
        }

        public Person(params object[] args)
        {
            Name = (string)args[0];
            Surname = (string)args[1];
            Age = (uint)args[2];
        }

        public int CompareTo(IFieldComparable other, string field)
        {
            return this[field].CompareTo(other[field]);
        }
    }

    internal class Program
    {
        private static Person[] persons = new Person[5];

        public static T[] Insert<T>(T[] array, string field, bool reversed = false)
            where T : IFieldComparable
        {
            var n = array.Length;
            for (var i = 1; i < n; ++i)
            {
                var now = array[i];
                var j = i - 1;

                while (j >= 0 && reversed ^ (array[j].CompareTo(now, field) > 0))
                {
                    array[j + 1] = array[j];
                    j--;
                }
                array[j + 1] = now;
            }
            return array;
        }
        
        
        public static void Main(string[] args)
        {
            persons[0] = new Person("Andrew", "Stanley", 33u);
            persons[1] = new Person("Josh", "Gryn", 75u);
            persons[2] = new Person("Jerry", "Gryn", 12u);
            persons[3] = new Person("Andrew", "Skowron", 76u);
            persons[4] = new Person("Juan", "Pablo", 100u);
            
            //persons[0].CompareTo(persons[1], null);

            foreach (var e in Insert(persons, "Age", false))
            {
                Console.WriteLine($"{e.Name} {e.Surname} is {e.Age} yo.");
            }
        }
    }
}