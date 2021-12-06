using System;
using System.Linq;
using System.Windows;
using Desktop.DataClass.Include;
using Desktop.DataClass.Other;
using Desktop.DataClass.Other.FQL;
using Desktop.DataClass.Persons;
using Desktop.View.Table;
using Desktop.Window.Query;

namespace Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private SchoolData schoolData = new SchoolData();
        public MainWindow()
        {
            InitializeComponent();
            
            schoolData.Students.Add(new Student
            {
                Name = "Jerry",
                Surname = "Gryn",
                BirthDate = new DateTime(2002, 5, 12),
                Class = "3pr",
            });
            schoolData.Students.Add(new Student
            {
                Name = "Josh",
                Surname = "Gryn",
                BirthDate = new DateTime(2002, 2, 23),
                Class = "3pr",
                Group = SchoolGroup.English | SchoolGroup.Italian
            });
            schoolData.Students.Add(new Student
            {
                Name = "Andrew",
                Surname = "Potato",
                BirthDate = new DateTime(2003, 4, 12),
                Class = "2pr",
            });
            schoolData.Students.Add(new Student
            {
                Name = "Spadino",
                Surname = "Panacletti",
                BirthDate = new DateTime(2004, 5, 23),
                Class = "1ai",
            });
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var win = new QueryCreator(schoolData)
            {
                Owner = this
            };
            win.ShowDialog();

            SelectQuery query = SelectQuery.Decompile(win.OutputQuery.Text.TrimInside());

            switch (win.TableSelected)
            {
                case "Students":
                    ContentControl.Content =
                        new ResultTable(!query.Fields.Any()?SchoolData.GetMemberPublicFieldsNames(win.TableSelected):query.Fields, 
                            new FQL(win.SchoolData.Students).Select(query));
                    break;
                case "Teachers":
                    ContentControl.Content =
                        new ResultTable(!query.Fields.Any()?SchoolData.GetMemberPublicFieldsNames(win.TableSelected):query.Fields, 
                            new FQL(win.SchoolData.Teachers).Select(query));
                    break;
                case "Employees":
                    ContentControl.Content =
                        new ResultTable(!query.Fields.Any()?SchoolData.GetMemberPublicFieldsNames(win.TableSelected):query.Fields, 
                            new FQL(win.SchoolData.Employees).Select(query));
                    break;
            }
            
        }
    }
}