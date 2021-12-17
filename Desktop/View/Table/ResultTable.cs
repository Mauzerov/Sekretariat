using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Desktop.Annotations;
using Desktop.DataClass.Other;
using Desktop.View.Table.Header;
using TableRow = System.Collections.Generic.Dictionary<string, System.IComparable>;
namespace Desktop.View.Table
{
    public class ResultTable : Grid
    {
        private IEnumerable<string> _fields;
        private SortableHeader _header;
        private SchoolData _schoolData;
        public bool AskBeforeDelete { get; set; } = true;
        public List<TableRow> Result { get; private set; }

        public ResultTable(IEnumerable<string> fields, List<TableRow> data, [CanBeNull] SchoolData schoolData)
        {
            VerticalAlignment = VerticalAlignment.Top;
            _schoolData = schoolData;
            _fields = fields;
            Result = data;
            GenerateHeader();
            Generate();
        }

        public ResultTable(IEnumerable<string> fields, FQL.FQL data, [CanBeNull] SchoolData schoolData)
            : this(fields, data.Result, schoolData)
        {
            
        }

        private void GenerateHeader()
        // Generate Sortable Buttons For Each Column
        {
            Children.Clear();
            
            var column = Convert.ToInt32(_schoolData != null);
            _header = new SortableHeader();
            if (_schoolData != null)
                ColumnDefinitions.Add(new ColumnDefinition { MaxWidth = 100 }); // Modify Button & Delete Button Max Width
            RowDefinitions.Add(new RowDefinition()); // Row For Sortable Buttons
            
            foreach (var title in _fields)
            {
                ColumnDefinitions.Add(new ColumnDefinition());
                var label = new SortableButton(_header)
                {
                    ColumnTitle = title, // Static Value used To Change Button Content After Click
                    Content = title,
                };
                label.Click += (sender, args) => // Sort Result & Chane Name To Indicate Sortable Direction
                {
                    if (label.State == SortableButton.ButtonState.None)
                        Result = FQL.FQL.Sort(Result, "UUID");
                    else
                    {
                        bool IsReversed(SortableButton.ButtonState state) => state == SortableButton.ButtonState.Desc;
                        Result = FQL.FQL.Sort(Result, label.ColumnTitle, IsReversed(label.State));
                    }
                    Generate(); // ReGenerate Result Rows
                };
                Children.Add(label); // Add Sortable Button To Top Row
                SetColumn(label, column++); // Set Correct Row
            }
        }

        private void ClearDataRows()
        {
            // Remove Every Element Except SortableButton Header 
            var tmp = Children.Cast<UIElement>().Where(item => !(item is SortableButton)).ToList();

            foreach (var t in tmp) { Children.Remove(t); }
            
            RowDefinitions.Clear(); // Remove All Rows
            RowDefinitions.Add(new RowDefinition()); // Add Back Sortable Header Row 
        }
        
        public void Generate()
        {
            ClearDataRows();
            var index = 0;
            foreach (var row in Result)
            {
                index++;
                RowDefinitions.Add(new RowDefinition());
                
                // Create Deletion Button & Modify Button!
                var modRow = new ModifyCell(_schoolData, Result, row, this);
                if (_schoolData != null)
                {
                    Children.Add(modRow);
                    SetColumn(modRow, 0);
                    SetRow(modRow, index);
                }

                var column = Convert.ToInt32(_schoolData != null);
                // Generate Ech Cell Independently Based On Its Type
                foreach (var cell in row.Where(cell => cell.Key != "UUID"))
                {
                    UIElement element;
                    if (cell.Value is DateTime date)
                    {
                        element = new DatePicker()
                        {
                            SelectedDate = date,
                            IsEnabled = false
                        };
                    }
                    else if (cell.Key == "Photo")
                    {
                        element = new ImageButton(
                            File.Exists(cell.Value.ToString())?cell.Value.ToString(): "NULL"
                            , row);
                        element.IsEnabled = false;
                    }
                    else
                    {
                        element = new TextBox()
                        {
                            IsReadOnly = true,
                            Text = cell.Value.ToString().Replace(", ", ",\n"),
                            TextWrapping = TextWrapping.Wrap,
                            AcceptsReturn = true,
                            AcceptsTab = true,
                        };
                    }
                    Children.Add(element);
                    if (_schoolData != null)
                        modRow.Add(cell.Key, element);
                    SetColumn(element, column);
                    SetRow(element, index);
                    column++;
                }
            }
        }
    }
}
