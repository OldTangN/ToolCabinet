using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ToolMgt.UI.ViewModel
{
    public class RelayCommand : ICommand
    {
        public RelayCommand()
        {

        }

        //public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        //{
        //    _execute = execute;
        //    _canExecute = canExecute;
        //}

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }



        private Action<object> _execute = null;
        private Func<object, bool> _canExecute = null;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute?.Invoke(parameter);
        }
    }
}
