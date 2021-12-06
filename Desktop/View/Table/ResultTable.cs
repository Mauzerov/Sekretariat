using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Desktop.DataClass.Include;
using Desktop.DataClass.Other.FQL;
using Desktop.DataClass.Persons;
using Desktop.View.Table.Header;

using TableRow = System.Collections.Generic.Dictionary<string, System.IComparable>;
namespace Desktop.View.Table
{
    public class ResultTable : Grid
    {
        public FQL FqlData;
        public IEnumerable<string> Fields;
        private SortableHeader _header;
        
        public ResultTable(IEnumerable<string> fields, FQL data)
        {
            Fields = fields;
            FqlData = data;
            GenerateHeader();
            Generate();
        }

        private void GenerateHeader()
        {
            Children.Clear();
            
            var column = 0;
            _header = new SortableHeader();
            RowDefinitions.Add(new RowDefinition());
            
            foreach (var title in Fields)
            {
                ColumnDefinitions.Add(new ColumnDefinition());
                var label = new SortableButton(_header)
                {
                    ColumnTitle = title,
                    Content = title,
                };
                label.Click += (sender, args) =>
                {
                    if (label.State == SortableButton.ButtonState.None)
                        FqlData = FqlData.OrderBy("UUID");
                    else
                    {
                        bool IsReversed(SortableButton.ButtonState state) => state == SortableButton.ButtonState.Desc;
                        FqlData = FqlData.OrderBy(label.ColumnTitle, IsReversed(label.State));
                    }
                    Generate();
                };
                Children.Add(label);
                SetColumn(label, column++);
            }
        }

        private void ClearDataRows()
        {
            var tmp = Children.Cast<UIElement>().Where(item => !(item is SortableButton)).ToList();

            foreach (var t in tmp)
            {
                Children.Remove(t);
            }
            
            RowDefinitions.Clear();
            RowDefinitions.Add(new RowDefinition());
        }
        
        private void Generate()
        {
            ClearDataRows();
            var index = 0;
            foreach (var row in FqlData.Result)
            {
                index++;
                RowDefinitions.Add(new RowDefinition());
                var column = 0;
                foreach (var cell in row)
                {
                    if (cell.Key == "UUID")
                        continue;

                    var element = new TextBox()
                    {
                        IsReadOnly = true
                    };
                    if (cell.Value is DateTime dateTime)
                        element.Text = dateTime.ToString("dd MMMM yyyy");
                    else
                        element.Text = cell.Value.ToString();

                    Children.Add(element);
                    SetColumn(element, column++);
                    SetRow(element, index);
                }
            }

            VerticalAlignment = VerticalAlignment.Top;
        }
    }
}