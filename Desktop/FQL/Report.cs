using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Desktop.DataClass.Other;
using Desktop.Scripts.CSV;
using Desktop.Scripts.XML;
using TableRow = System.Collections.Generic.Dictionary<string, System.IComparable>;
namespace Desktop.FQL
{
    public class Report
    {
        //private string table;
        public IEnumerable<string> Fields = new List<string>();

        public IEnumerable<TableRow> Data;

        public Report(string source)
        {
            switch (System.IO.Path.GetExtension(source).Substring(1).ToLower())
            {
                case "xml":
                    Data = FromXml.CreateResult(source, out Fields);
                    Debug.WriteLine(Fields.Count());
                    break;
                case "csv":
                    Data = FromCsv.CreateResult(source, out Fields);
                    break;
                default:
                    MessageBox.Show("Exception!", "Unhandled File Extension!");
                    break;
            }
        }

        public List<TableRow> Get()
        {
            return Data.ToList();
        }
    }
}