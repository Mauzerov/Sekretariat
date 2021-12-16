using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Desktop.DataClass.Include;
using Desktop.DataClass.Other;
using Desktop.DataClass.Persons;
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

            ImageButtonHolder.Content = new ImageButton("NULL", null)
            {
                MaxHeight = int.MaxValue
            };

            foreach (var gender in Enum.GetValues(typeof(Gender)))
            {
                var btn = new RadioButton
                {
                    Content = gender,
                    GroupName = "GenderGroup"
                };
                if (((Gender) gender) == Gender.Other)
                    btn.IsChecked = true;
                GenderHolder_DataInput.Children.Add(btn);
            }

            var fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);

            foreach (var field in fields)
            {
                FrameworkElement element;

                if (field.FieldType == typeof(DateTime))
                {
                    Right.Children.Add(new Label
                        {
                            Content = field.Name.AddSpacesBeforeCapitalized()
                        });
                    element = new DatePicker
                    {
                        DisplayDate = DateTime.Now.Date,
                        Text = DateTime.Now.Date.ToString(CultureInfo.InvariantCulture),
                    };
                }
                else if (field.FieldType == typeof(Class))
                {
                    Right.Children.Add(new Label
                        {
                            Content = field.Name.AddSpacesBeforeCapitalized()
                        });
                    
                    // Complicated Stuff
                    element = new TeacherClassSelector();


                }
                else if (field.FieldType.BaseType == typeof(Enum))
                {
                    Right.Children.Add(
                        new Label
                        {
                            Content = field.Name.AddSpacesBeforeCapitalized()
                        });
                    if (Enum.GetUnderlyingType(field.FieldType) == typeof(long))
                    {
                        element = new ListBox
                        {
                            Name = field.Name + "_DataInput"
                        };
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
                        element = new ComboBox
                        {
                            Name = field.Name + "_DataInput"
                        };
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
                        HintText = field.Name.AddSpacesBeforeCapitalized(),
                    };
                }

                element.Name = field.Name + "_DataInput";
                Right.Children.Add(element);
            }

            foreach (FrameworkElement c in Right.Children.Cast<FrameworkElement>().Where(e => !(e is Label)))
            {
                c.Margin = new Thickness(0, 0, 0, 10);
            }
        }

        private void AddPerson(object sender = null, object e = null)
        {
            IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
            {
                if (depObj == null)
                    yield return null;
    
                for (var i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    var child = VisualTreeHelper.GetChild(depObj, i);

                    if (child is T dependencyObject)
                        yield return dependencyObject;
                
                    foreach (var childOfChild in FindVisualChildren<T>(child))
                        yield return childOfChild;
                }
            }
            
            var newPerson = Person.MakeNew(_type.Name);
            foreach (FrameworkElement element in
                FindVisualChildren<FrameworkElement>(this)
                    .Where(ee => ee.Name.EndsWith("_DataInput")))
            {
                IComparable valueElement = null;
                
                var currentName = element.Name;
                if (currentName.Replace("_DataInput", "").EndsWith("Holder"))
                {
                    if (!currentName.StartsWith("Gender")) continue;
                    
                    if (element is StackPanel frameworkElement)
                    {
                        var checkedValue = frameworkElement.Children.Cast<UIElement>()
                            .Where(ee => ee is RadioButton)
                            .Where(ee => ((RadioButton) ee).IsChecked == true);

                        var uiElements = checkedValue as UIElement[] ?? checkedValue.ToArray();
                        if (uiElements.Any())
                            valueElement = (Gender) ((RadioButton) uiElements.ToList()[0]).Content;
                        else
                            valueElement = Gender.Other;
                    }
                    else
                        valueElement = Gender.Other;
                }
                else
                {
                    switch (element)
                    {
                        case HintInput hintInput:
                            valueElement = hintInput.GetText();
                            break;
                        case DatePicker datePicker:
                            valueElement = DateTime.Parse(datePicker.Text).Date;
                            break;
                        case TeacherClassSelector selector:
                            valueElement = selector.Class;
                            break;
                        case ListBox listBox:
                            var enums = listBox.SelectedItems.Cast<UIElement>()
                                .Select(ee => ((ListBoxItem) ee).Tag.ToString())
                                .Aggregate("", (old, now) => old + now + ", ");
                            valueElement = (IComparable) Enum.Parse(typeof(SchoolGroup), enums);
                            break;
                        case ComboBox comboBox:
                            valueElement = null;
                            break;
                    }
                }
                newPerson[currentName
                    .Replace("_DataInput", "")
                    .Replace("Holder", "")] = valueElement;
            }
            // TODO: Add Image Source
            
            SchoolData[_type.Name].Add(newPerson.AsDict());
            MessageBox.Show(
                $"Successfully Created & Added New {_type.Name}!", "Success!");
        }
    }
}