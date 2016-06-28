using System;
using System.Windows.Input;
using Prism.Commands;


namespace CB.Model.Prism
{
    public class Disposer: IDisposable
    {
        #region Fields
        private readonly IDisposable _disposable;
        #endregion


        #region  Constructors & Destructor
        public Disposer(IDisposable disposable)
        {
            _disposable = disposable;
            DisposeCommand = new DelegateCommand(Dispose);
        }
        #endregion


        #region  Commands
        public ICommand DisposeCommand { get; }
        #endregion


        #region Methods
        public void Dispose()
        {
            _disposable.Dispose();
        }
        #endregion
    }
}