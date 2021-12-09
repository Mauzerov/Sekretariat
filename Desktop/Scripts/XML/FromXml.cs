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
                            var row = Person.MakeNew(table);
                            Debug.WriteLine("Here");
                            
                            foreach (var node in SchoolData.GetMemberPublicFieldsNames(table))
                            {
                                var value = reader.GetAttribute(node);
                                if (value == null) continue;

                                var type = SchoolData.GetFieldType(table, node);
                                Debug.WriteLine($"{value} of type: {type}");
                                if (type == typeof(DateTime))
                                    row[node] = DateTime.Parse(value);
                                if (type == typeof(string))
                                    row[node] = value;
                                if (type.BaseType == typeof(Enum))
                                    row[node] = (IComparable)Enum.Parse(type, value);
                            }
                            
                            @return.Add(row.AsDict());
                            break;
                    }

                }
            }
            return @return;
        }
    }
}