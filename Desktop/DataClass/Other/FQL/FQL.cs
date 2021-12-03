﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Desktop.DataClass.Person;
using Desktop.DataClass.Other;

namespace Desktop.DataClass.Other
{
    public class FQL<T> : IEnumerable<T> where T : IFieldComparable
    {
        private readonly IEnumerable<T> _data;

        public List<T> ToList()
        {
            return _data.ToList();
        }
        public FQL(IEnumerable<T> array)
        {
            _data = array;
        }
        public IEnumerator<T> GetEnumerator()
        {
            return _data.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public FQL<T> Filter(IEnumerable<T> array, IEnumerable<Where> wheres)
        {
            bool Good(T element) => wheres.All(where => element[where.Key].CompareTo(where.Value).Operand(where.Op));
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