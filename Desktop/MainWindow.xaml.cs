﻿using System;
using System.Diagnostics;
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
using Desktop.Window.Help;
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
        }
        
        private void LoadData(bool @override)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "XML-File | *.xml",
            };

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
                FromXml.SaveTo(resultTable.Result, queryTable, dialog.FileName);
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
                FromCsv.SaveTo(resultTable.Result, queryTable, dialog.FileName);
        }

        private void ReportLoad(object o = null, object e = null)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv|XML-File | *.xml|All Files | *.*",
                FilterIndex = 1
            };
            if (dialog.ShowDialog() != true)
                return;

            var report = new Report(dialog.FileName);

            ContentControl.Content = new ResultTable(report.Fields, report.Get(), null);
        }

        private void QueryLoad(object o = null, object e = null)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "FQL File | *.fql"
            };

            if (dialog.ShowDialog() != true)
                return;

            try
            {
                using (var r = new StreamReader(dialog.OpenFile()))
                {
                    query = SelectQuery.Decompile(r.ReadToEnd().TrimEnd());
                    queryTable = query.Table;
                }

                ContentControl.Content =
                    // When None of the fields are selected pass whole 'table' fields
                    new ResultTable(
                        !query.Fields.Any() ? SchoolData.GetMemberPublicFieldsNames(queryTable) : query.Fields,
                        // Pass A Filtered Selected Fields as a result
                        new FQL.FQL(schoolData[queryTable]).Filter(query.Wheres).Select(query), schoolData);
            } catch (Exception err)
            {
                MessageBox.Show("Unable To Load Query", "Exception!");
            }
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

        private void OpenHelp(object o = null, object e = null)
        {
            new Help{}.ShowDialog();
        }
        #endregion
    }
}