using System;
using System.Collections.Generic;
using System.Linq;
using Desktop.DataClass.Other;

namespace Desktop.FQL
{
    public class SelectQuery
    {
        public SelectQuery(string table, IEnumerable<string> fields = null, IEnumerable<Where> wheres = null)
        {
            Table = table;
            Fields = fields ?? new string [] { };
            Wheres = wheres ?? new Where [] { };
        }

        public IEnumerable<string> Fields { get; }

        public IEnumerable<Where> Wheres { get; }

        public string Table { get; }

        private static string WhereToString(Where where)
        {
            return $"{where.Key} {where.HumanOp()} {where.Value}";
        }

        public string String {
            // Compile Fields Into FQL (SQL) Query String
            get
            {
                var returnFields = "";
                var returnWheres= "";

                if (!Fields.Any())
                    returnFields = "ALL";
                else
                {
                    returnFields = Fields.Aggregate(returnFields, (current, field) => current + (field + ", "));
                    returnFields = returnFields.Substring(0, returnFields.Length - 2);
                }
                
                if (!Wheres.Any())
                    returnWheres = "TRUE";
                else
                {
                    returnWheres = Wheres.Aggregate(returnWheres, (current, where) => current + WhereToString(where) + " AND ");
                    returnWheres = returnWheres.Substring(0, returnWheres.Length - 5);
                }
                
                return $"SELECT {returnFields} FROM {Table} \nWHERE {returnWheres}";
            }
        }


        public static SelectQuery Decompile(string query)
        // Decompiles String Query To
        // - Wheres List
        // - Fields List
        // - Selected Table
        //
        // Just From One String 
        {
            string[] returnColumns;
            Where[] returnWheres;
            query = query.TrimInside();
            var fromLocation = query.IndexOf("FROM", StringComparison.Ordinal);
            var whereLocation = query.IndexOf("WHERE", StringComparison.Ordinal);
            var tableLocation = fromLocation + 5;

            var table = query.Substring(tableLocation, whereLocation - tableLocation);
            
            var columns = query.Substring(7, fromLocation - 8);
            if (columns == "ALL")
                returnColumns = SchoolData.GetMemberPublicFieldsNames(table).ToArray();
            else
            {
                returnColumns = columns.Split(',');
                for (var i = 0; i < returnColumns.Length; ++i)
                    returnColumns[i] = returnColumns[i].Trim();
            }

            var wheres = query.Substring(whereLocation + 6, query.Length - whereLocation - 6);
            
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
            
            return new SelectQuery(table, returnColumns, returnWheres);
        }
    }
}