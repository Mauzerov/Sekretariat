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

                    UIElement element;
                    if (cell.Value is DateTime date)
                    {
                        element = new DatePicker()
                        {
                            SelectedDate = date,
                            IsEnabled = false
                        };
                    }
                    /*else if (cell.Value is Enum @enum)
                    {
                        List<object> GetSelected(Enum type)
                        {
                            return Enum.GetValues(type.GetType()).Cast<Enum>().Where(e => e.HasFlag(type)).Cast<object>().ToList();
                        }
                        
                        if (@enum.GetType().GetCustomAttributes(typeof(FlagsAttribute), false).Length > 0)
                        {
                            element = new ListBox
                            {
                                IsManipulationEnabled = false,
                                IsEnabled = false,
                                SelectionMode = SelectionMode.Multiple,
                            };
                            // TODO: Fix Selected Items Display
                            foreach (Enum e in Enum.GetValues(@enum.GetType()))
                                ((ListBox) element).Items.Add(new ListBoxItem {Tag = e, Content = e, IsSelected = false});

                            var s = GetSelected(@enum);
                            Debug.Assert(s.Count > 0);
                            foreach (ListBoxItem item in ((ListBox)element).Items)
                            {
                                if (s.Contains((Enum) item.Tag))
                                    item.IsSelected = true;
                            }
                        }
                        else
                        {
                            element = new ComboBox()
                            {
                                IsEnabled = false,
                                IsManipulationEnabled = false,
                            };
                            foreach (Enum e in Enum.GetValues(@enum.GetType()))
                                ((ComboBox) element).Items.Add(new ComboBoxItem() {Tag = e, Content = e, IsSelected = false});

                            var s = GetSelected(@enum);
                            foreach (ListBoxItem item in ((ComboBox)element).Items)
                            {
                                if (s.Contains((Enum) item.Tag))
                                    item.IsSelected = true;
                            }
                        }
                    }*/
                    else if (cell.Key == "Photo")
                    {
                        element = new ImageButton(cell.Value.ToString(), row);
                    }
                    else
                    {
                        element = new TextBox()
                        {
                        IsReadOnly = true,
                        Text = cell.Value.ToString()
                        };
                    }
                    Children.Add(element);
                    SetColumn(element, column++);
                    SetRow(element, index);
                }
            }

            VerticalAlignment = VerticalAlignment.Top;
        }
    }
}
