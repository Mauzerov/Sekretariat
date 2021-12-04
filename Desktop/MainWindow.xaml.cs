using System.Windows;
using Desktop.DataClass.Person;
using Desktop.Window.Query;

namespace Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var win = new QueryCreator(typeof(Student), new[] {"Name", "Surname", "Birth Day"});
            win.Show();
        }
    }
}