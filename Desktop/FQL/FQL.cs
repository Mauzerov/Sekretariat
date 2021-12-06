using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Desktop.DataClass.Persons;
using Desktop.DataClass.Other;

using TableRow = System.Collections.Generic.Dictionary<string, System.IComparable>;
namespace Desktop.DataClass.Other.FQL
{
    public class FQL
    {
        private List<TableRow> _result;
        public List<TableRow> Result => _result;

        public FQL(IEnumerable<TableRow> data)
        {
            _result = data.ToList();
        }
        
        public FQL Select(SelectQuery query)
        {
            var newResult = new List<TableRow>();
            
            foreach (var row in Result)
            {
                var newRow = new TableRow
                {
                    ["UUID"] = row["UUID"]
                };
                var keys = row.Keys;
                foreach (var field in query.Fields)
                {
                    if (keys.Contains(field))
                    {
                        newRow[field] = row[field];
                    }
                }
                newResult.Add(newRow);
            }

            return new FQL(newResult);
        }

        public FQL OrderBy(string field, bool reversed = false)
        {
            var array = Result;
            var n = Result.Count;
            for (var i = 1; i < n; ++i)
            {
                var now = array[i];
                var j = i - 1;
            
                while (j >= 0 && reversed ^ (array[j][field].CompareTo(now[field]) > 0))
                {
                    array[j + 1] = array[j];
                    j--;
                }
                array[j + 1] = now;
            }
            return new FQL(array);
        }

        // public FQL<T> Filter(IEnumerable<T> array, IEnumerable<Where> wheres)
        // {
        //     bool Good(T element) => wheres.All(where => element[where.Key].CompareTo(where.Value).Operand(where.Op));
        //     return new FQL<T>(array.Where(Good));
        // }
        //
        // public FQL<T> OrderBy(IEnumerable<T> _array, string field, bool reversed)
        // {
        //     var array = _array.ToArray();
        //     var n = array.Length;
        //     for (var i = 1; i < n; ++i)
        //     {
        //         var now = array[i];
        //         var j = i - 1;
        //
        //         while (j >= 0 && reversed ^ (array[j].CompareTo(now, field) > 0))
        //         {
        //             array[j + 1] = array[j];
        //             j--;
        //         }
        //         array[j + 1] = now;
        //     }
        //     return new FQL<T>(array);
        // }
    }
}