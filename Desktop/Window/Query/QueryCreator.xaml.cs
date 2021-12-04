using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Desktop.DataClass.Include;
using Desktop.DataClass.Other;
using Desktop.DataClass.Persons;
using Desktop.View.Table.Header;

namespace Desktop.Window.Query
{
    public partial class QueryCreator : System.Windows.Window
    {
        private SchoolData schoolData;
        private string tableSelected = "None";
        public QueryCreator(SchoolData tables)
        {
            schoolData = tables;
            
            InitializeComponent();
            InitializeTableList();
            
            TableList.SelectionChanged += (sender, args) =>
            {
                tableSelected = ((ComboBoxItem) args.AddedItems[0]).Content as string;
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
            var fields = "";
            var amount = ColumnList.SelectedItems.Count;
            if (amount > 0)
            {
                var newFields = ColumnList.SelectedItems.Cast<object>().Aggregate(fields,
                    (current, field) => current + ((field as ListBoxItem)?.Tag + ", "));
                fields = newFields.Substring(0, newFields.Length - 2);
            }
            else
                fields = "ALL";

            OutputQuery.Text = $"SELECT {fields} FROM {tableSelected}\nWHERE {"True"}";
        }

        private void UpdateTableFields()
        {
            var tableFields = SchoolData.GetMemberPublicFieldsNames(tableSelected);
            
            ColumnList.Items.Clear();
            if (tableSelected == "None") return;
            foreach (var field in tableFields)
            {
                ColumnList.Items.Add(new ListBoxItem
                {
                    Content = field,
                    Tag = field
                });
            }
        }

        private void ClearWheres()
        {
            
        }

        private void InitializeTableList()
        {
            foreach (var table in schoolData.GetTables())
            {
                TableList.Items.Add(new ComboBoxItem
                {
                    Content = table
                });
            }
        }
    }
}