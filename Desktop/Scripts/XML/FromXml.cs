using System;
using System.Xml;
using Desktop.DataClass.Other;
using Desktop.DataClass.Persons;
using TableRow = System.Collections.Generic.Dictionary<string, System.IComparable>;
namespace Desktop.Scripts.XML
{
    public static class FromXml
    {
        public static void Create(SchoolData schoolData, string source)
        {
            using (var reader = new XmlTextReader(source))
            {
                while (reader.Read())
                {
                    // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element when reader.AttributeCount != 0:
                            var table = reader.Name;
                            var row = Person.MakeNew(table);

                            foreach (var node in SchoolData.GetMemberPublicFieldsNames(table))
                            {
                                var value = reader.GetAttribute(node);
                                if (value == null) continue;

                                var type = SchoolData.GetFieldType(table, node);
                                if (type == typeof(DateTime))
                                    row[node] = DateTime.Parse(value);
                                if (type == typeof(string))
                                    row[node] = value;
                                if (type.BaseType == typeof(Enum))
                                    row[node] = (IComparable)Enum.Parse(type, value);
                            }
                            
                            schoolData[table].Add(row.AsDict());
                            break;
                    }

                }
            }
        }
    }
}