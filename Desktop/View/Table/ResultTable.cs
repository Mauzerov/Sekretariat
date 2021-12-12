using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Desktop.DataClass.Other;
using Desktop.View.Table.Header;
using TableRow = System.Collections.Generic.Dictionary<string, System.IComparable>;
namespace Desktop.View.Table
{
    public class ResultTable : Grid
    {
        private FQL.FQL _fqlData;
        private IEnumerable<string> _fields;
        private SortableHeader _header;
        private SchoolData _schoolData;
        public bool AskBeforeDelete { get; set; } = true;

        public ResultTable(IEnumerable<string> fields, FQL.FQL data, SchoolData schoolData)
        {
            VerticalAlignment = VerticalAlignment.Top;
            _schoolData = schoolData;
            _fields = fields;
            _fqlData = data;
            GenerateHeader();
            Generate();
        }
        
        private void GenerateHeader()
        // Generate Sortable Buttons For Each Column
        {
            Children.Clear();
            
            var column = 1;
            _header = new SortableHeader();
            ColumnDefinitions.Add(new ColumnDefinition { MaxWidth = 100 }); // Modify Button & Delete Button MAx Width
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
                        _fqlData = _fqlData.OrderBy("UUID");
                    else
                    {
                        bool IsReversed(SortableButton.ButtonState state) => state == SortableButton.ButtonState.Desc;
                        _fqlData = _fqlData.OrderBy(label.ColumnTitle, IsReversed(label.State));
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
            foreach (var row in _fqlData.Result)
            {
                index++;
                RowDefinitions.Add(new RowDefinition());
                
                // Create Deletion Button & Modify Button!
                var modRow = new ModifyCell(_schoolData, _fqlData.Result, row, this);
                
                Children.Add(modRow);
                SetColumn(modRow, 0);
                SetRow(modRow, index);
                
                var column = 1;
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
                        element = new ImageButton(cell.Value.ToString(), row);
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
                    modRow.Add(cell.Key, element);
                    SetColumn(element, column);
                    SetRow(element, index);
                    column++;
                }
            }
        }
    }
}
