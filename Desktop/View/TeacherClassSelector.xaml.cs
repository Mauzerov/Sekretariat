using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Desktop.DataClass.Include;

namespace Desktop.View
{
    public partial class TeacherClassSelector : StackPanel
    {
        public Class Class = new Class();

        public TeacherClassSelector()
        {
            InitializeComponent();
        }

        private void AddElement(object sender = null, object e = null)
        {
            
            if (
                !ClassInput.Text.Contains(",") &&
                !ClassInput.Text.Contains(":") &&
                !ClassInput.Text.Contains("\"") &&
                !ClassInput.Text.Contains("\'") &&
                ClassInput.HintText != ClassInput.Text &&
                ClassInput.Text != "" &&
                int.TryParse(HoursInput.Text, out var number)
                )
            {
                if (Class.Classes.ContainsKey(ClassInput.Text))
                    return;
                var text = ClassInput.Text;
                Class[text] = number;

                var row = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    MaxHeight = 30
                };

                var remove = new Button()
                {
                    Content = new Image
                    {
                        Source = new BitmapImage(new Uri("/trash.png", UriKind.RelativeOrAbsolute))
                    }
                };

                remove.Click += (o, args) =>
                {
                    this.Children.Remove(row);
                    Debug.WriteLine(text);
                    Debug.WriteLine(Class.Classes.Count);
                    this.Class.Classes.Remove(text);
                };
                row.Children.Add(new Label
                    {
                        Content = $"{text}:\t{number}"
                    }
                );
                row.Children.Add(remove);
                this.Children.Add(row);

            }
            else
            {
                MessageBox.Show("Niepoporawna Warość!", "Nie Dodano!");
            }
        }
    }
}