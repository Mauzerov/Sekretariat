using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Desktop.DataClass.Persons;
using Desktop.View.Table.Header;

namespace Desktop.View.Table
{
    public class ResultTable : Grid
    {
        public ResultTable(Type type, string[] rows, System.Collections.Generic.List<Person> data = null)
        {
            
            Debug.Assert(type.IsSubclassOf(typeof(Person)), "Wanted Type Is Not Inherited From Person!");
            
            var column = 0;
            var header = new SortableHeader();
            foreach (var r in rows)
            {
                ColumnDefinitions.Add(new ColumnDefinition());
                var label = new SortableButton(header)
                {
                    Content = r
                };
                Children.Add(label);
                SetColumn(label, column++);
            }

            VerticalAlignment = VerticalAlignment.Top;
        }
    }
}