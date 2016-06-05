using System;
using System.Threading.Tasks;
using System.Windows.Input;
using CB.Model.Common;
using Microsoft.Practices.Prism.Commands;


namespace CB.Model.Prism
{
    public class ProgressTaskViewModelBase: PrismViewModelBase
    {
        #region Fields
        private readonly Action _cancelAction;
        private readonly Func<Task> _executeAction;
        private string _name;
        private readonly Action _pauseAction;
        private Progress _progress;
        private readonly Action _resumeAction;
        private ProgressState _state = ProgressState.Stopped;
        #endregion


        #region  Constructors & Destructor
        protected ProgressTaskViewModelBase(Func<Task> executeAction, Action cancelAction = null,
            Action pauseAction = null, Action resumeAction = null)
        {
            _executeAction = executeAction;
            _cancelAction = cancelAction;
            _pauseAction = pauseAction;
            _resumeAction = resumeAction;
            ExecuteCommand = DelegateCommand.FromAsyncHandler(Execute, () => CanExecute);
        }
        #endregion


        #region  Commands
        public ICommand ExecuteCommand { get; }
        #endregion


        #region  Properties & Indexers
        public virtual bool CanCancel => _cancelAction != null && State != ProgressState.Stopped;
        public virtual bool CanExecute => State == ProgressState.Stopped;

        public virtual bool CanPause => _pauseAction != null && State == ProgressState.Running;

        public virtual bool CanResume => _resumeAction != null && State == ProgressState.Paused;

        public virtual string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public virtual Progress Progress
        {
            get { return _progress; }
            set { SetProperty(ref _progress, value); }
        }

        public virtual ProgressState State
        {
            get { return _state; }
            protected set
            {
                if (SetProperty(ref _state, value))
                {
                    NotifyPropertiesChanged(nameof(CanExecute), nameof(CanCancel), nameof(CanPause), nameof(CanResume));
                }
            }
        }
        #endregion


        #region Methods
        public virtual void Cancel()
        {
            if (!CanCancel) return;

            _cancelAction();
            State = ProgressState.Stopped;
        }

        public virtual async Task Execute()
        {
            if (!CanExecute) return;

            State = ProgressState.Running;
            await _executeAction();
            State = ProgressState.Stopped;
        }

        public virtual void Pause()
        {
            if (!CanPause) return;

            _pauseAction();
            State = ProgressState.Paused;
        }

        public virtual void Resume()
        {
            if (!CanResume) return;

            _resumeAction();
            State = ProgressState.Running;
        }
        #endregion
    }
}