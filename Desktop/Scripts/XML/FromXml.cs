using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Dynamic;
using System.Runtime.Remoting.Messaging;
using System.Xml;
using Desktop.DataClass.Include;
using Desktop.DataClass.Other;
using Desktop.DataClass.Persons;
using TableRow = System.Collections.Generic.Dictionary<string, System.IComparable>;
namespace Desktop.Scripts.XML
{
    public static class FromXml
    {
        public static List<TableRow> Create(string table, string source)
        {
            var @return = new List<TableRow>();
            
            using (var reader = new XmlTextReader(source))
            {
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element when reader.AttributeCount != 0:
                            var row = new TableRow
                            {
                                ["UUID"] = Guid.NewGuid()
                            };
                            Debug.WriteLine("Here");
                            
                            foreach (var node in SchoolData.GetMemberPublicFieldsNames(table))
                            {
                                var value = reader.GetAttribute(node);
                                Debug.WriteLine(value);
                                var type = SchoolData.GetFieldType(table, node);
                                if (type == typeof(DateTime))
                                {
                                    if (value != null)
                                        row[node] = DateTime.Parse(value);
                                    else
                                        row[node] = null;
                                }
                                if (type == typeof(string))
                                {
                                    if (value != null)
                                        row[node] = value;
                                    else
                                        row[node] = "NULL";
                                }

                                if (type == typeof(Enum))
                                {
                                    // Todo: Enum Conversion
                                    (Enum) value;
                                }
                            }
                            
                            @return.Add(row);
                            break;
                    }

                }
            }
            return @return;
        }
    }
}