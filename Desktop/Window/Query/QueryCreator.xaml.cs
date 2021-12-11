using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Desktop.DataClass.Other.FQL;
using Desktop.DataClass.Other;
using Desktop.View;
using Label = System.Windows.Controls.Label;

namespace Desktop.Window.Query
{
    public partial class QueryCreator : System.Windows.Window
    {
        private List<Where> wheres = new List<Where>();

        public SchoolData SchoolData { get; }

        public string TableSelected { get; private set; } = "None";

        public QueryCreator(SchoolData tables)
        {
            SchoolData = tables;
            
            InitializeComponent();
            InitializeTableList();

            TableList.SelectionChanged += (sender, args) =>
            {
                TableSelected = ((ComboBoxItem) args.AddedItems[0]).Content as string;
                {
                    ClearWheres();
                    UpdateTableFields();
                    UpdateOutputQuery();
                }
            };
            ColumnList.SelectionChanged += (sender, args) =>
            {
                UpdateOutputQuery();
            };
        }
        
        private void UpdateOutputQuery()
        {
            var query = new SelectQuery(TableSelected, ColumnList.SelectedItems.Cast<ListBoxItem>().Select(item => item.Tag).Cast<string>(), wheres);
            OutputQuery.Text = query.String;
        }

        private void UpdateTableFields()
        {
            var tableFields = SchoolData.GetMemberPublicFieldsNames(TableSelected);
            
            ColumnList.Items.Clear();
            FieldsInput.Items.Clear();
            if (TableSelected == "None") return;
            foreach (var field in tableFields)
            {
                ColumnList.Items.Add(new ListBoxItem
                {
                    Content = field,
                    Tag = field
                });
                FieldsInput.Items.Add(new ComboBoxItem
                {
                        Content = field
                });
            }

            
        }

        private void ClearWheres()
        {
            wheres.Clear();
            WhereRowOutput.Children.Clear();
        }

        private void InitializeTableList()
        {
            foreach (var table in SchoolData.GetTables())
            {
                TableList.Items.Add(new ComboBoxItem
                {
                    Content = table
                });
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            SelectQuery.Decompile(OutputQuery.Text.TrimInside());
        }

        private void AddWhereCondition(object sender, RoutedEventArgs e)
        {
            if (!(FieldsInput.SelectedItem != null && OperatorsInput.SelectedItem != null))
                return;
            if (InputParent.Children.Count <= 0)
                return;
            
            var whereRow = new StackPanel
            {
                Orientation = Orientation.Horizontal,
            };
            var field = (string) ((ComboBoxItem) FieldsInput.SelectedItem).Content;
            
            WhereRowOutput.RowDefinitions.Add(new RowDefinition());
            WhereRowOutput.Children.Add(whereRow);
            Grid.SetRow(whereRow, wheres.Count);
            
            var where = new Where
            {
                Key = field,
                Op = Where.OperandFromString(((ComboBoxItem) OperatorsInput.SelectedItem).Content as string),
            };
            
            var child = InputParent.Children[0];
            IComparable value;
            switch (child)
            {
                case DatePicker datePicker:
                    value = datePicker.SelectedDate;
                    break;
                case TextBox textBox:
                    value = textBox.Text;
                    break;
                default:
                    value = "NULL";
                    break;
            }
            where.Value = value;
            wheres.Add(where);

            var removeBtn = new Button
            {
                Content = new Image
                {
                    Source = new BitmapImage(new Uri("/trash.png", UriKind.RelativeOrAbsolute))
                },
                MaxHeight = 30
            };
            removeBtn.Click += (o, args) =>
            {
                WhereRowOutput.Children.Remove(whereRow);
                wheres.Remove(where);
                UpdateOutputQuery();
            };
            whereRow.Children.Add(removeBtn);
            
            whereRow.Children.Add(
                new Label()
                {
                    Content = where.Human()
                }
            );
            
            UpdateOutputQuery();
        }

        private void CreateValidInputType(object sender, SelectionChangedEventArgs e)
        {
            InputParent.Children.Clear();
            if (e.AddedItems.Count <= 0) return;
            
            switch (((ComboBoxItem)e.AddedItems[0]).Content as string)
            {
                case "Photo":
                    break;
                case "StartDate": case "BirthDate":
                    InputParent.Children.Add(
                        new DatePicker
                        {
                            Height = 30
                        });
                    break;
                default:
                    InputParent.Children.Add(
                        new TextBox
                        {
                            Height = 30,
                            AcceptsReturn = true,
                            AcceptsTab = true,
                            TextWrapping = TextWrapping.Wrap
                        });
                    break;
            }
        }
    }
}