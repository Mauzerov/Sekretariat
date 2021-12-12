
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;
using Desktop.DataClass.Other;
using Desktop.Window;
using TableRow = System.Collections.Generic.Dictionary<string, System.IComparable>;
namespace Desktop.View.Table
{

    public class ModifyCell : Grid
    {
        private Dictionary<string, UIElement> _elements = new Dictionary<string, UIElement>();
        private bool _modification = false;
        
        public ModifyCell(SchoolData data, List<TableRow> resultData, TableRow row, ResultTable wholeResult)
        {
            ColumnDefinitions.Add(new ColumnDefinition());
            ColumnDefinitions.Add(new ColumnDefinition());
            HorizontalAlignment = HorizontalAlignment.Stretch;
            MaxWidth = 100;
            var delete = new Button
            {
                Content = new Image {Source = new BitmapImage(new Uri("/trash.png", UriKind.RelativeOrAbsolute))}
            };
            var modify = new Button
            {
                Content = new Image {Source = new BitmapImage(new Uri("/edit.png", UriKind.RelativeOrAbsolute)),}
            };

            delete.Click += (sender, args) =>
            {
                if (wholeResult.AskBeforeDelete)
                {
                    var dialog = new NeverAskAgainDialog(
                        Application.Current.MainWindow,
                        "Deletion",
                        "Are You Sure You Want\nTo Delete This Record?",
                        () => { wholeResult.AskBeforeDelete = false; });
                    dialog.ShowDialog();
                    if (dialog.Result != true)
                        return;
                }
                
                foreach (var table in data.GetTables())
                {
                    // Remove Result Row From Data Table
                    foreach (var dataRow in data[table].Where(dataRow => Equals(dataRow["UUID"], row["UUID"])))
                    {
                        data[table].Remove(dataRow);
                        break;
                    }
                }
                resultData.Remove(row);
                wholeResult.Generate();
            };
            
            modify.Click += (sender, args) =>
            {
                if (!_modification)
                {
                    // Enable Inputs
                    foreach (var element in _elements)
                    {
                        element.Value.IsEnabled = true;
                        if (element.Value is TextBoxBase textBoxBase)
                            textBoxBase.IsReadOnly = false;
                    }
                }
                else
                {
                    // Disable Inputs 
                    foreach (var element in _elements)
                    {
                        if (element.Value is TextBoxBase textBoxBase)
                            textBoxBase.IsReadOnly = true;
                        else
                            element.Value.IsEnabled = false;
                    }
                    // Update Values For Each Table
                    foreach (var table in data.GetTables())
                    {
                        // Get Rows Where UUID Is Equal To Clicked Button  
                        foreach (var dataRow in data[table].Where(dataRow => Equals(dataRow["UUID"], row["UUID"])))
                        {
                            // For Each Element In Row Result Of Correct ID
                            foreach (var element in _elements)
                            {
                                // Update Data Values
                                switch (element.Value)
                                {
                                    case ImageButton imgBtn:
                                        dataRow[element.Key] = (IComparable) imgBtn.Source;
                                        break;
                                    case TextBox textBox:
                                        dataRow[element.Key] = (IComparable) textBox.Text;
                                        break;
                                    case ContentControl control:
                                        dataRow[element.Key] = (IComparable)control.Content;
                                        break;
                                }
                            }

                            foreach (var dataRowRow in
                                dataRow.Where(dataRowRow => row.ContainsKey(dataRowRow.Key)))
                            {
                                row[dataRowRow.Key] = dataRowRow.Value;
                            }
                            break;
                        }
                    }
                }
                _modification = !_modification;
                ((Image) modify.Content).Source = new BitmapImage(
                    new Uri( _modification ? "/save.png" : "/edit.png"
                        , UriKind.RelativeOrAbsolute));
            };
            
            Children.Add(delete);
            SetColumn(delete, 0);
            Children.Add(modify);
            SetColumn(modify, 1);
        }

        public void Add(string field, UIElement element)
        {
            _elements[field] = element;
        }
    }
}