using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Desktop.DataClass.Person;
using Desktop.View.Table.Header;

namespace Desktop.Window.Query
{
    public partial class QueryCreator : System.Windows.Window
    {
        public QueryCreator(Type type, string[] rows, System.Collections.Generic.List<Person> data = null)
        {
            
            Debug.Assert(type.IsSubclassOf(typeof(Person)), "Wanted Type Is Not Inherited From Person!");
            InitializeComponent();


            var column = 0;
            var header = new SortableHeader();
            foreach (var r in rows)
            {
                Grid.ColumnDefinitions.Add(new ColumnDefinition());
                var label = new SortableButton(header)
                {
                    Content = r
                };
                Grid.Children.Add(label);
                Grid.SetColumn(label, column++);
            }

            Grid.VerticalAlignment = VerticalAlignment.Top;
        }
    }
}