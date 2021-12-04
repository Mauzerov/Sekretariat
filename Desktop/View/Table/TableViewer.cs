using Desktop.View.Table.Viewer;

namespace Desktop.View.Viewer
{
    public class TableViewer : ObservableObject
    {
        public Command StudentCommand  { get; set; }
        public Command TeacherCommand  { get; set; }
        public Command EmployeeCommand { get; set; }
        public StudentTable StudentViewer { get; set; }
        public TeacherTable TeacherViewer { get; set; }
        public EmployeeTable EmployeeViewer { get; set; }
        
        private object _currentView;

        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }
        
        public TableViewer()
        {
            StudentViewer = new StudentTable();
            TeacherViewer = new TeacherTable();
            EmployeeViewer = new EmployeeTable();
            CurrentView = StudentViewer;

            StudentCommand = new Command(
                o => {
                    CurrentView = StudentViewer;
                }
            );
            TeacherCommand = new Command(
                o => {
                    CurrentView = TeacherViewer;
                }
            );
            EmployeeCommand = new Command(
                o => {
                    CurrentView = EmployeeViewer;
                }
            );
        }
    }
}