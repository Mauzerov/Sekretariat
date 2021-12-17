using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using TableRow = System.Collections.Generic.Dictionary<string, System.IComparable>;
namespace Desktop.FQL
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

        public static List<TableRow> Sort(List<TableRow> array, string field, bool reversed = false)
        {
            var n = array.Count;
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

            return array;
        }

        public FQL OrderBy(string field, bool reversed = false)
        {
            return new FQL(FQL.Sort(Result, field, reversed));
        }
        
        public FQL Filter(IEnumerable<Where> wheres)
        {
            var returnDict = new List<TableRow>();

            foreach (var row in Result)
            {
                var add = true;
                foreach (var where in wheres)
                {
                    var field = row[where.Key];
                    
                    if (field is string stringField && (where.Op == Where.Operand.Eq || where.Op == Where.Operand.Neq))
                    {
                        var regex = new Regex(where.Value.ToString()
                            .Replace("%", ".*")
                            .Replace(" ", "\\s+"),
                            RegexOptions.IgnoreCase
                        );
                        var match = regex.IsMatch(stringField);
                        if ((!match && where.Op == Where.Operand.Eq) || (match && where.Op == Where.Operand.Neq))
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