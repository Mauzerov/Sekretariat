using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Desktop.DataClass.Include;
using Desktop.DataClass.Other;
using Desktop.Include;
using Desktop.View;

namespace Desktop.Window
{
    public partial class InsertCreator : System.Windows.Window
    {
        private SchoolData SchoolData { get; set; }
        private System.Type _type;

        public InsertCreator(SchoolData schoolData, System.Type type)
        {
            InitializeComponent();
            SchoolData = schoolData;
            _type = type;
            // BindingFlags.DeclaredOnly |
            // BindingFlags.Public |
            // BindingFlags.Instance;

            ImageButtonHolder.Content = new ImageButton("NULL", null)
            {
                MaxHeight = int.MaxValue
            };

            var fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);

            foreach (var field in fields)
            {
                UIElement element;

                if (field.FieldType == typeof(DateTime))
                {
                    element = new DatePicker
                    {
                        DisplayDate = DateTime.Now.Date,
                        Text = DateTime.Now.Date.ToString(CultureInfo.InvariantCulture),
                        Name = field.Name
                    };
                }
                else if (field.FieldType == typeof(Class))
                {
                    // Complicated Stuff
                    element = new StackPanel();
                }
                else if (field.FieldType.BaseType == typeof(Enum))
                {
                    // TODO: ListBox / ComboBox
                    if (Enum.GetUnderlyingType(field.FieldType) == typeof(long))
                    {
                        element = new ListBox();
                        foreach (var value in Enum.GetValues(field.FieldType))
                            (element as ListBox).Items.Add(
                                new ListBoxItem
                                {
                                    Tag = value,
                                    Content = value
                                }
                            );
                    }
                    else
                    {
                        element = new ComboBox();
                        foreach (var value in Enum.GetValues(field.FieldType))
                            (element as ComboBox).Items.Add(
                                new ComboBoxItem
                                {
                                    Tag = value,
                                    Content = value
                                }
                            );
                    }
                    
                }
                else
                {
                    element = new HintInput
                    {
                        HintText = field.Name.AddSpacesBeforeCapitalized()
                    };
                }

                Right.Children.Add(element);
            }

            foreach (FrameworkElement c in Right.Children.Cast<FrameworkElement>().Where(e => !(e is Label)))
            {
                c.Margin = new Thickness(0, 0, 0, 10);
            }
        }
    }
}