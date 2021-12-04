using System;
using System.Windows.Input;

namespace Desktop.View
{
    public class Command : ICommand
    {
        private Action<object> excecute;
        private Func<object, bool> canExcecute;

        public Command(Action<object> excecute, Func<object, bool> canExcecute = null)
        {
            this.excecute = excecute;
            this.canExcecute = canExcecute;
        }

        public bool CanExecute(object parameter) => canExcecute == null || canExcecute(parameter);
        
        public void Execute(object parameter) => excecute(parameter);

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}