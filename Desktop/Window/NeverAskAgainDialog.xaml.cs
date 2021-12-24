using System;
using System.Windows;

namespace Desktop.Window
{
    public partial class NeverAskAgainDialog : System.Windows.Window
    {
        public bool ShowAgain { get; private set; } = true;
        public bool? Result { get; private set; } = null;
        private Action NeverAskAgainAction;

        public NeverAskAgainDialog(System.Windows.Window owner, string title, string message, Action neverAskAgainAction)
        {
            InitializeComponent();
            Owner = owner;
            Title = title;
            Message.Text = message;
            NeverAskAgainAction = neverAskAgainAction;
        }

        private void SetYes(object o, object e)
        {
            ShowAgain = Check.IsChecked == true;
            if (Check.IsChecked == true)
                NeverAskAgainAction.Invoke();
            Result = true;
            Close();
        }
        private void SetNo(object o, object e)
        {
            ShowAgain = Check.IsChecked == true;
            if (Check.IsChecked == true)
                NeverAskAgainAction.Invoke();
            Result = false;
            Close();
        }
        
    }
}