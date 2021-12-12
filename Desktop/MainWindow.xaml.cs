using System;
using System.Linq;
using System.Windows;
using Desktop.DataClass.Include;
using Desktop.DataClass.Other;
using Desktop.FQL;
using Desktop.DataClass.Persons;
using Desktop.Scripts.XML;
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
            schoolData.Students.Add(new Student
            {
                Name = "No classer",
                Surname = "classito",
                BirthDate = new DateTime(2304, 2, 13),
            });
            schoolData.Students.Add(new Student
            {
                Name = "Noname",
                Surname = "NoClass",
                BirthDate = new DateTime(2049, 2, 27),
            });
            FromXml.Create(schoolData, "temp.xml");
        }
        #region Menu Buttons Click Events
        private void NewQueryButtonClick(object sender, RoutedEventArgs e)
        {
            // Open Query Creator Window
            var win = new QueryCreator(schoolData) {Owner = this};
            win.ShowDialog();
            // Decompile Selected Query
            var query = SelectQuery.Decompile(win.OutputQuery.Text.TrimInside());
            
            if (win.TableSelected == "None")
                return;
            // Generate Result Table
            ContentControl.Content =
                // When None of the fields are selected pass whole 'table' fields
                new ResultTable(!query.Fields.Any()?SchoolData.GetMemberPublicFieldsNames(win.TableSelected):query.Fields, 
                    // Pass A Filtered Selected Fields as a result
                    new FQL.FQL(win.SchoolData[win.TableSelected]).Filter(query.Wheres).Select(query), schoolData);
        }
        #endregion
    }
}