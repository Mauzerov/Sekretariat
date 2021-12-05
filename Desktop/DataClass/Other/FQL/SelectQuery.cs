using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;

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


        public static SelectQuery Decompile(string query)
        {
            string[] returnColumns;
            Where[] returnWheres;
            query = query.TrimInside();
            var fromLocation = query.IndexOf("FROM", StringComparison.Ordinal);
            var whereLocation = query.IndexOf("WHERE", StringComparison.Ordinal);

            var columns = query.Substring(7, fromLocation - 8);
            if (columns == "ALL")
                returnColumns = new string[] { };
            else
            {
                returnColumns = columns.Split(',');
                for (var i = 0; i < returnColumns.Length; ++i)
                    returnColumns[i] = returnColumns[i].Trim();
            }

            var wheres = query.Substring(whereLocation + 6, query.Length - whereLocation - 6);
            // TODO : Add A Where Generator And Check IF Where Converter Works
            if (wheres != "TRUE")
            {
                wheres = wheres.Replace(" AND ", "\n");
                var stringWheres = wheres.Split('\n');
                var stringWheresAmount = stringWheres.Length;
                returnWheres = new Where[stringWheresAmount];
                for (var i = 0; i < stringWheresAmount; ++i)
                {
                    var elements = stringWheres[i].Split(' ');

                    var where = new Where
                    {
                        Key = elements[0],
                        Op = Where.OperandFromString(elements[1]),
                        Value = elements[2]
                    };

                    returnWheres[i] = where;
                }
            }
            else
                returnWheres = new Where[] { };
            
            return new SelectQuery("None", returnColumns, returnWheres);
        }
    }
}