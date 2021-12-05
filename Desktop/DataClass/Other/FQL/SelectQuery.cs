using System.Collections.Generic;
using System.Linq;

namespace Desktop.DataClass.Other.FQL
{
    public class SelectQuery
    {
        private readonly string _table;
        private readonly IEnumerable<string> _fields;
        private readonly IEnumerable<Where> _wheres;


        public SelectQuery(string table, IEnumerable<string> fields = null, IEnumerable<Where> wheres = null)
        {
            _table = table;
            _fields = fields ?? new string [] { };
            _wheres = wheres ?? new Where [] { };
        }

        private static string WhereToString(Where where)
        {
            return $"{where.Key} {where.HumanOp()} {where.Value}";
        }

        public string String {
            get
            {
                var returnFields = "";
                var returnWheres= "";

                if (!_fields.Any())
                    returnFields = "ALL";
                else
                {
                    returnFields = _fields.Aggregate(returnFields, (current, field) => current + (field + ", "));
                    returnFields = returnFields.Substring(0, returnFields.Length - 2);
                }
                
                if (!_wheres.Any())
                    returnWheres = "TRUE";
                else
                {
                    returnWheres = _wheres.Aggregate(returnWheres, (current, where) => current + WhereToString(where) + " AND ");
                    returnWheres = returnWheres.Substring(0, returnWheres.Length - 5);
                }
                
                return $"SELECT {returnFields} FROM {_table} \nWHERE {returnWheres}";
            }
        }
    }
}