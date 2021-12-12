using System;
using System.IO;
using System.Linq;
using System.Windows;
using Desktop.DataClass.Include;
using Desktop.DataClass.Other;
using Desktop.FQL;
using Desktop.DataClass.Persons;
using Desktop.Scripts.CSV;
using Desktop.Scripts.EXCEL;
using Desktop.Scripts.XML;
using Desktop.View.Table;
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
                        case "xlsx": case "xlsm": case "xlt": case "xls":
                            FromExcel.Create(ref schoolData, dialog.FileName, @override);
                            break;
                        default:
                            MessageBox.Show("Exception!", "Unhandled File Extension!");
                            break;
                    }
                    break;
            }
        }
        
        #region Menu Buttons Click Events
        private void NewQueryButtonClick(object sender, object e)
        {
            // Open Query Creator Window
            var win = new QueryCreator(schoolData) {Owner = this};
            win.ShowDialog();
            // Decompile Selected Query
            query = SelectQuery.Decompile(win.OutputQuery.Text.TrimInside());
            queryTable = win.TableSelected;
            if (win.TableSelected == "None")
                return;
            RefreshResults(null, null);
        }
        
        private void RefreshResults(object sender, object e)
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

        private void LoadData(object sender, object e)
        {
            LoadData(true);
        }
        private void AddData(object sender, object e)
        {
            LoadData(false);
        }
        private void SaveData(object sender, object e)
        {
            FromXml.SaveTo(schoolData, DatabaseDestination);
            MessageBox.Show($"Data Was Successfully Saved To:\n\t{Directory.GetCurrentDirectory()}\\{DatabaseDestination}", "Success!");
        }
        private void SaveAsData(object sender, object e)
        {
            var dialog = new SaveFileDialog();
            if (dialog.ShowDialog() == true)
                FromXml.SaveTo(schoolData, dialog.FileName);
        }

        private void OpenDatabase(object o = null, object e = null)
        {
            if (File.Exists(DatabaseDestination))
                FromXml.Create(ref schoolData, DatabaseDestination, true);
        }
        #endregion
    }
}