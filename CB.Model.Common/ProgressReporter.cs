namespace CB.Model.Common
{
    public class ProgressReporter<T>: BindableObject, IReportProgress<T>
    {
        #region Fields
        protected T _progress;
        #endregion


        #region  Properties & Indexers
        public virtual T Progress
        {
            get { return _progress; }
            protected set { SetProperty(ref _progress, value); }
        }
        #endregion


        #region Methods
        public virtual void Report(T value)
        {
            Progress = value;
        }
        #endregion
    }
}