using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
        
        public FQL Filter(IEnumerable<Where> wheres)
        {
            var returnDict = new List<TableRow>();

            foreach (var row in Result)
            {
                bool add = true;
                foreach (var where in wheres)
                {
                    var field = row[where.Key];
                    
                    if (field is string stringField && (where.Op == Where.Operand.Eq || where.Op == Where.Operand.Neq))
                    {
                        var regex = new Regex(where.Value.ToString()
                            .Replace("%", ".*")
                            .Replace(" ", "\\s+"));
                        var match = regex.IsMatch(stringField);
                        if ((!match && where.Op == Where.Operand.Eq) || (match && where.Op == Where.Operand.Neq))
                        {
                            add = false;
                            break;
                        }
                    }
                    else if (field is int intField)
                    {
                        if (!intField.Operand(where.Op, int.Parse(where.Value.ToString())))
                        {
                            add = false;
                            break;
                        }
                    }
                    else if (field is DateTime dateTimeField)
                    {
                        var compare = DateTime.Parse(where.Value.ToString());

                        if (!dateTimeField.ToBinary().Operand(where.Op, compare.ToBinary()))
                        {
                            add = false;
                            break;
                        }
                    }
                    else
                    {
                        
                    }
                }
                if (add)
                    returnDict.Add(row);
            }

            return new FQL(returnDict);
        }
    }
}