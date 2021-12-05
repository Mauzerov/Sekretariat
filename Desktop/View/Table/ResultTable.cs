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
        public IEnumerable<IEnumerable<IComparable>> Result;
        public IEnumerable<string> Fields;
        
        public ResultTable(IEnumerable<string> fields, IEnumerable<IEnumerable<IComparable>> data)
        {
            Fields = fields;
            Result = data;
            Generate();
        }

        public void Generate()
        {
            Children.Clear();
            var column = 0;
            var header = new SortableHeader();
            RowDefinitions.Add(new RowDefinition());
            foreach (var r in Fields)
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
            foreach (var row in Result)
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