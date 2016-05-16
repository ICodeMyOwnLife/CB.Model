using System;
using System.Windows.Input;


namespace CB.Model.Common
{
    public abstract class ViewModelBase: NotifiableViewModelBase
    {
        #region Fields
        private double _progress;
        private string _state;
        #endregion


        #region  Properties & Indexers
        public virtual double Progress
        {
            get { return _progress; }
            protected set { SetPropertySync(ref _progress, value); }
        }

        public virtual string State
        {
            get { return _state; }
            protected set { SetProperty(ref _state, value); }
        }
        #endregion


        #region Override
        protected virtual ICommand GetCommand(ref ICommand command, Action<object> execute,
            Predicate<object> canExecute = null)
            => command ?? (command = new RelayCommand(execute, canExecute));
        #endregion
    }
}