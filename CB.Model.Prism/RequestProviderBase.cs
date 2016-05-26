using System;
using System.Threading;
using Prism.Interactivity.InteractionRequest;


namespace CB.Model.Prism
{
    public abstract class RequestProviderBase<T> where T: INotification
    {
        #region Fields
        private readonly SynchronizationContext _synchronizationContext = SynchronizationContext.Current;
        #endregion


        #region Abstract
        public abstract InteractionRequest<T> Request { get; }
        #endregion


        #region Implementation
        protected virtual void RunOnUiThread(Action action)
        {
            _synchronizationContext.Send(_ => action(), null);
        }
        #endregion
    }
}