using System;
using System.IO;
using System.Linq;
using System.Windows;
using Desktop.DataClass.Include;
using Desktop.DataClass.Other;
using Desktop.FQL;
using Desktop.DataClass.Persons;
using Desktop.Scripts.CSV;
using Desktop.Scripts.XML;
using Desktop.View.Table;
using Desktop.Window;
using Desktop.Window.Query;
using Microsoft.Win32;

namespace Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public const string DatabaseDestination = "database.xml";
        private SchoolData schoolData = new SchoolData();
        private SelectQuery query = null;
        private string queryTable = "None";
        public MainWindow()
        {
            InitializeComponent();
            
            OpenDatabase();
            /*
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
            FromXml.Create(ref schoolData, "temp.xml");*/
        }
        
        private void LoadData(bool @override)
        {
            var dialog = new OpenFileDialog();

            switch (dialog.ShowDialog())
            {
                case null:
                    return;
                case true:
                    switch (Path.GetExtension(dialog.FileName).Substring(1).ToLower())
                    {
                        case "xml":
                            FromXml.Create(ref schoolData, dialog.FileName, @override);
                            break;
                        case "csv":
                            FromCsv.Create(ref schoolData, dialog.FileName, @override);
                            break;
                        default:
                            MessageBox.Show("Exception!", "Unhandled File Extension!");
                            break;
                    }
                    break;
            }
        }
        
        #region Menu Buttons Click Events
        private void NewQueryButtonClick(object o = null, object e = null)
        {
            // Open Query Creator Window
            var win = new QueryCreator(schoolData) {Owner = this};
            win.ShowDialog();
            // Decompile Selected Query
            query = SelectQuery.Decompile(win.OutputQuery.Text.TrimInside());
            queryTable = win.TableSelected;
            if (win.TableSelected == "None")
                return;
            RefreshResults();
        }
        
        private void RefreshResults(object o = null, object e = null)
        {
            if (queryTable == "None")
                return;
            if (query != null)
                ContentControl.Content =
                    // When None of the fields are selected pass whole 'table' fields
                    new ResultTable(!query.Fields.Any()?SchoolData.GetMemberPublicFieldsNames(queryTable):query.Fields, 
                        // Pass A Filtered Selected Fields as a result
                        new FQL.FQL(schoolData[queryTable]).Filter(query.Wheres).Select(query), schoolData);
        }

        private void LoadData(object o = null, object e = null)
        {
            LoadData(true);
        }
        private void AddData(object o = null, object e = null)
        {
            LoadData(false);
        }
        private void SaveData(object o = null, object e = null)
        {
            FromXml.SaveTo(schoolData, DatabaseDestination);
            MessageBox.Show($"Data Was Successfully Saved To:\n\t{Directory.GetCurrentDirectory()}\\{DatabaseDestination}", "Success!");
        }
        private void SaveAsData(object o = null, object e = null)
        {
            var dialog = new SaveFileDialog
            {
                Filter = "XML-File | *.xml",
            };
            if (dialog.ShowDialog() == true)
                FromXml.SaveTo(schoolData, dialog.FileName);
        }

        private void OpenDatabase(object o = null, object e = null)
        {
            if (File.Exists(DatabaseDestination))
                FromXml.Create(ref schoolData, DatabaseDestination, true);
        }


        private void ReportSaveAsXml(object o = null, object e = null)
        {
            var dialog = new SaveFileDialog
            {
                Filter = "XML-File | *.xml",
            };
            if (dialog.ShowDialog() != true)
                return;
            
            if (queryTable == "None")
                return;
                
            if (ContentControl.Content is ResultTable resultTable)
                FromXml.SaveTo(resultTable.Result.Result, queryTable, dialog.FileName);
        }
        
        private void ReportSaveAsCsv(object o = null, object e = null)
        {
            var dialog = new SaveFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv|All Files | *.*"
            };
            if (dialog.ShowDialog() != true)
                return;
            
            if (queryTable == "None")
                return;
                
            if (ContentControl.Content is ResultTable resultTable)
                FromCsv.SaveTo(resultTable.Result.Result, queryTable, dialog.FileName);
        }

        private void ReportLoad(object o = null, object e = null)
        {
            
        }


        private void OpenStudentInput(object o = null, object e = null)
        {
            new InsertCreator(schoolData, typeof(Student))
            { Owner = this }.ShowDialog();
        }
        private void OpenTeacherInput(object o = null, object e = null)
        {
            new InsertCreator(schoolData, typeof(Teacher))
                { Owner = this }.ShowDialog();
        }
        private void OpenEmployeeInput(object o = null, object e = null)
        {
            new InsertCreator(schoolData, typeof(Employee))
                { Owner = this }.ShowDialog();
        }
        #endregion
    }
}