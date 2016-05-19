using System;


namespace CB.Model.Common
{
    public class ProgressReporter<T> : BindableObject, IProgress<T>
    {
        #region Fields
        private T _progress;
        #endregion


        #region  Properties & Indexers
        public T Progress
        {
            get { return _progress; }
            private set { SetProperty(ref _progress, value); }
        }
        #endregion


        #region Implementation
        void IProgress<T>.Report(T value)
        {
            Progress = value;
        }
        #endregion
    }
}