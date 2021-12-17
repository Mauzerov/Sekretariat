using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;
using Desktop.DataClass.Include;
using Desktop.DataClass.Other;
using Desktop.DataClass.Persons;
using TableRow = System.Collections.Generic.Dictionary<string, System.IComparable>;
namespace Desktop.Scripts.XML
{
    public static class FromXml
    {
        public static void Create(ref SchoolData schoolData, string source, bool @override = false)
        {
            var sd = new SchoolData();
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
                                if (type == typeof(Class))
                                    row[node] = Class.FromString(value);
                            }

                            if (@override)
                                sd[table].Add(row.AsDict());
                            else
                                schoolData[table].Add(row.AsDict());
                            break;
                    }

                }
            }
            if (@override)
                schoolData = sd;
        }

        public static IEnumerable<TableRow> CreateResult(
            string source,
            out IEnumerable<string> fields)
        {
            var reader = new XmlDocument();
            reader.Load(source);
            var root = reader.ChildNodes[1];
            IEnumerable<string> allAttributes = new List<string>();

            var schoolData = new List<TableRow>();
            string table = null;
            foreach (XmlElement child in root.ChildNodes)
            {
                if (table == null)
                {
                    table = child.Name;
                    allAttributes = child.Attributes.Cast<XmlAttribute>().Select(attr => attr.Name);
                }
                
                if (table != child.Name)
                    continue;

                var row = new TableRow();
                var collection = child.Attributes;
                foreach (XmlAttribute attr in collection)
                {
                    row[attr.Name] = attr.Value;
                }

                if (!row.ContainsKey("UUID"))
                    row["UUID"] = Guid.NewGuid();
                schoolData.Add(row);
            }
            fields = allAttributes;
            return schoolData;
        }

        private static void WriteElement(XmlWriter writer, string table, TableRow dataRow)
        {
            writer.WriteStartElement(table);
            foreach (var el in dataRow.Where(e => e.Key !=  "UUID" && e.Key != "Photo"))
            {
                writer.WriteStartAttribute(el.Key);
                writer.WriteValue(el.Value.ToString());
                writer.WriteEndAttribute();
            }
            writer.WriteEndElement();
            writer.WriteWhitespace("\n");
        }
        
        public static void SaveTo(SchoolData schoolData, string destination)
        {
            var writer = new XmlTextWriter(destination, Encoding.UTF8);
            if (writer.Settings != null)
                writer.Settings.Indent = true;
            writer.WriteStartDocument();
            writer.WriteWhitespace("\n");
            writer.WriteStartElement("Persons");
            writer.WriteWhitespace("\n");
            foreach (var row in schoolData.GetTables())
            {
                foreach (var element in schoolData[row])
                {
                    WriteElement(writer, row, element);
                }
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
        }
        public static void SaveTo(IEnumerable<TableRow> data, string table, string destination)
        {
            var writer = new XmlTextWriter(destination, Encoding.UTF8);
            writer.WriteStartDocument();
            writer.WriteWhitespace("\n");
            writer.WriteStartElement("Persons");
            writer.WriteWhitespace("\n");
            foreach (var row in data)
            {
                WriteElement(writer, table, row);
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
        }
    }
}