using System.Windows;
using Desktop.DataClass.Other;
using Desktop.DataClass.Persons;
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
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var win = new QueryCreator(schoolData)
            {
                Owner = this
            };
            win.ShowDialog();
        }
    }
}