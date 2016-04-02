using System;
using System.Windows.Input;


namespace CB.Model.Common
{
    public class RelayCommand: ICommand
    {
        #region Fields
        private readonly Predicate<object> _canExecute;
        private readonly Action<object> _execute;
        #endregion


        #region  Constructors & Destructor
        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _canExecute = canExecute;
            _execute = execute;
        }
        #endregion


        #region Events
        public event EventHandler CanExecuteChanged
        {
            add { if (_canExecute != null) CommandManager.RequerySuggested += value; }
            remove { if (_canExecute != null) CommandManager.RequerySuggested -= value; }
        }
        #endregion


        #region Methods
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
        #endregion
    }
}