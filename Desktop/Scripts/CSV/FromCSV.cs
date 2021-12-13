using System.Collections.Generic;
using System.IO;
using System.Linq;
using Desktop.DataClass.Other;

using TableRow = System.Collections.Generic.Dictionary<string, System.IComparable>;
namespace Desktop.Scripts.CSV
{
    public static class FromCsv
    {
        public static void Create(ref SchoolData schoolData, string source, bool @override = false)
        {
            
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