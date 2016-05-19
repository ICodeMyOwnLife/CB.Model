using System;


namespace CB.Model.Common
{
    public abstract class ProgressReporter<TProgress, TReport>
        : BindableObject, IReportProgress<TProgress>, IProgress<TReport>
    {
        #region Fields
        protected TProgress _progress;
        #endregion


        #region Abstract
        protected abstract TProgress GetProgressFromReportValue(TReport value);
        public abstract void Report(TReport reportValue);
        #endregion


        #region  Properties & Indexers
        public virtual TProgress Progress
        {
            get { return _progress; }
            protected set { SetProperty(ref _progress, value); }
        }
        #endregion
    }
}