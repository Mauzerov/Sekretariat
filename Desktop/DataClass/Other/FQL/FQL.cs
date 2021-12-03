using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Desktop.DataClass.Person;
using Desktop.DataClass.Other;

namespace Desktop.DataClass.Other
{
    public class FQL<T> : IEnumerable<T> where T : IFieldComparable
    {
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

        public FQL<T> Filter(ICollection<T> array, ICollection<Where> wheres)
        {
            bool Good(T element)
            {
                foreach (var where in wheres)
                    if (!element[where.Key].CompareTo(where.Value).Operand(where.Op))
                        return false;
                return true;
            }

            return new FQL<T>(array.Where(Good));
        }

        public FQL<T> OrderBy(IEnumerable<T> _array, string field, bool reversed)
        {
            var array = _array.ToArray();
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
            return new FQL<T>(array);
        }
    }
}