namespace CB.Model.Common
{
    public class FileProgressReporter: TimedProgressReporterBase<long>, IReportFileProgress
    {
        #region Fields
        private double _bytesPerSecond;
        private long _bytesTransferred;
        private string _fileName;
        private long _fileSize;
        private string _rate;
        private string _transferred;
        #endregion


        #region  Properties & Indexers
        public double BytesPerSecond
        {
            get { return _bytesPerSecond; }
            private set
            {
                if (SetProperty(ref _bytesPerSecond, value))
                {
                    Rate = FileCapacityHelper.NormalizeCapacity(value) + "/s";
                }
            }
        }

        public long BytesTransferred
        {
            get { return _bytesTransferred; }
            private set
            {
                if (!SetProperty(ref _bytesTransferred, value)) return;

                Transferred = FileCapacityHelper.NormalizeCapacity(value);
                var elapsedSecond = ElapsedTime.TotalSeconds;
                BytesPerSecond = elapsedSecond > 0 ? value / elapsedSecond : 0;
            }
        }

        public string FileName
        {
            get { return _fileName; }
            set { SetProperty(ref _fileName, value); }
        }

        public long FileSize
        {
            get { return _fileSize; }
            set { SetProperty(ref _fileSize, value); }
        }

        public virtual string Rate
        {
            get { return _rate; }
            protected set { SetProperty(ref _rate, value); }
        }

        public string Transferred
        {
            get { return _transferred; }
            private set { SetProperty(ref _transferred, value); }
        }
        #endregion


        #region Override
        protected override double GetProgressFromReportValue(long value)
        {
            return (double)value / FileSize;
        }

        protected override void SetProgress(double newProgress, long reportValue)
        {
            base.SetProgress(newProgress, reportValue);
            BytesTransferred = reportValue;
        }
        #endregion
    }
}