
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;
using Desktop.DataClass.Other;
using TableRow = System.Collections.Generic.Dictionary<string, System.IComparable>;
namespace Desktop.View.Table
{

    public class ModifyCell : Grid
    {
        private Button _delete;
        private Button _modify;
        private Dictionary<string, UIElement> _elements = new Dictionary<string, UIElement>();
        private bool _modification = false;
        
        public ModifyCell(SchoolData data, List<TableRow> resultData, TableRow row, ResultTable wholeResult)
        {
            ColumnDefinitions.Add(new ColumnDefinition());
            ColumnDefinitions.Add(new ColumnDefinition());
            HorizontalAlignment = HorizontalAlignment.Stretch;
            MaxWidth = 100;
            _delete = new Button
            {
                Content = new Image
                {
                    Source = new BitmapImage(new Uri("/trash.png", UriKind.RelativeOrAbsolute)),
                }
            };
            _modify = new Button
            {
                Content = new Image
                {
                    Source = new BitmapImage(new Uri("/edit.png", UriKind.RelativeOrAbsolute)),
                }
            };

            _delete.Click += (sender, args) =>
            {
                foreach (var table in data.GetTables())
                {
                    foreach (var dataRow in data[table])
                    {
                        if (Equals(dataRow["UUID"], row["UUID"]))
                        {
                            data[table].Remove(dataRow);
                            break;
                        }
                    }
                }
                resultData.Remove(row);
                wholeResult.Generate();
            };
            
            _modify.Click += (sender, args) =>
            {
                if (!_modification)
                {
                    foreach (var element in _elements)
                    {
                        element.Value.IsEnabled = true;
                        if (element.Value is TextBoxBase textBoxBase)
                            textBoxBase.IsReadOnly = false;
                    }
                }
                else
                {
                    foreach (var element in _elements)
                    {
                        if (element.Value is TextBoxBase textBoxBase)
                            textBoxBase.IsReadOnly = true;
                        else
                            element.Value.IsEnabled = false;
                    }

                    foreach (var table in data.GetTables())
                    {
                        foreach (var dataRow in data[table])
                        {
                            if (Equals(dataRow["UUID"], row["UUID"]))
                            {
                                foreach (var element in _elements)
                                {
                                    if (element.Value is ImageButton imgBtn)
                                    {
                                        dataRow[element.Key] = (IComparable) imgBtn.Source;
                                    }
                                    else if (element.Value is TextBox textBox)
                                    {
                                        dataRow[element.Key] = (IComparable) textBox.Text;
                                    }
                                    else if (element.Value is ContentControl control)
                                    {
                                        dataRow[element.Key] = (IComparable)control.Content;
                                    }
                                }

                                foreach (var dataRowRow in dataRow)
                                {
                                    if (row.ContainsKey(dataRowRow.Key))
                                        row[dataRowRow.Key] = dataRowRow.Value;
                                }
                                break;
                            }
                        }
                    }
                }
                _modification = !_modification;
                ((Image) _modify.Content).Source = new BitmapImage(
                    new Uri( _modification ? "/save.png" : "/edit.png"
                        , UriKind.RelativeOrAbsolute));
            };
            
            Children.Add(_delete);
            SetColumn(_delete, 0);
            Children.Add(_modify);
            SetColumn(_modify, 1);
        }

        public void Add(string field, UIElement element)
        {
            _elements[field] = element;
        }
    }
}