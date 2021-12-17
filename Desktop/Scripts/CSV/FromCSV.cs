using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Desktop.DataClass.Other;
using Desktop.Include;
using TableRow = System.Collections.Generic.Dictionary<string, System.IComparable>;
namespace Desktop.Scripts.CSV
{
    public static class FromCsv
    {
        public static IEnumerable<TableRow> CreateResult(string source, out IEnumerable<string> fields)
        {
            var schoolData = new List<TableRow>();
            
            using (var reader = new StreamReader(source))
            {
                var columns = reader.ReadLine().SplitNotInCommas(',').Select(e => e.Replace('\"', '\0'));
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    Debug.WriteLine(line);
                    var content = line.SplitNotInCommas(',').Select(e => e.Replace('\"', '\0')).ToList();

                    var row = new TableRow();
                    var index = 0;
                    foreach (var column in columns)
                    {
                        row[column] = content[index++];
                    }

                    if (!row.ContainsKey("UUID"))
                        row["UUID"] = Guid.NewGuid();
                    schoolData.Add(row);
                }
                fields = columns;
            }
            Debug.WriteLine(schoolData.Count);
            return schoolData;
        }

        public static void SaveTo(IEnumerable<TableRow> data, string table, string destination)
        {
            using (var writer = new StreamWriter(destination))
            {
                if (!data.Any())
                    return;
                var keys = data.ToList()[0].Keys;
                
                var @string = keys.Where(s => s != "UUID").Aggregate((workingSentence, next) => $"\"{next}\",{workingSentence}");
                writer.WriteLine(@string);
                
                foreach (var row in data)
                {
                    var line = row
                        .Where(c => c.Key != "UUID")
                        .Select(c => c.Value)
                        .Aggregate((workingSentence, next) => $"\"{next}\",{workingSentence}");
                    writer.WriteLine(line);
                }
            }
        }
    }
}