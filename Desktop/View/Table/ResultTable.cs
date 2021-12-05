using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Desktop.DataClass.Persons;
using Desktop.View.Table.Header;

namespace Desktop.View.Table
{
    public class ResultTable : Grid
    {
        public ResultTable(IEnumerable<string> fields, IEnumerable<IEnumerable<IComparable>> data)
        {
            Children.Clear();
            var column = 0;
            var header = new SortableHeader();
            RowDefinitions.Add(new RowDefinition());
            foreach (var r in fields)
            {
                ColumnDefinitions.Add(new ColumnDefinition());
                var label = new SortableButton(header)
                {
                    Content = r
                };
                Children.Add(label);
                SetColumn(label, column++);
            }

            var index = 0;
            foreach (var row in data)
            {
                RowDefinitions.Add(new RowDefinition());
                column = 0;
                index++;
                foreach (var cell in row)
                {
                    var element = new Label()
                    {
                        Content = cell.ToString()
                    };
                    Children.Add(element);
                    SetColumn(element, column++);
                    SetRow(element, index);
                }
            }

            VerticalAlignment = VerticalAlignment.Top;
        }
    }
}