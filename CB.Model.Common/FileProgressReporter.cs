using System;


namespace CB.Model.Common
{
    public class FileProgressReporter: TimedProgressReporterBase<long>, IReportFileProgress
    {
        #region Fields
        private double _bytesPerSecond;
        private long _bytesTransferred;
        private string _fileName;
        private long _fileSize;
        private string _speed;
        private SpeedType _speedType;
        private string _transferred;
        #endregion


        #region  Constructors & Destructor
        public FileProgressReporter()
        {
            SpeedType = SpeedType.MBps;
        }
        #endregion


        #region  Properties & Indexers
        public double BytesPerSecond
        {
            get { return _bytesPerSecond; }
            private set { SetProperty(ref _bytesPerSecond, value); }
        }

        public long BytesTransferred
        {
            get { return _bytesTransferred; }
            private set
            {
                if (SetProperty(ref _bytesTransferred, value))
                {
                    Transferred = FileCapacityHelper.Normalize(value);
                }
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

        public virtual string Speed
        {
            get { return _speed; }
            protected set { SetProperty(ref _speed, value); }
        }

        public SpeedType SpeedType
        {
            get { return _speedType; }
            set
            {
                _speedType = value;
                switch (value)
                {
                    case SpeedType.Bps:
                        _unit = FileCapacityHelper.ONE_BYTE;
                        break;
                    case SpeedType.KBps:
                        _unit = FileCapacityHelper.ONE_KB;
                        break;
                    case SpeedType.MBps:
                        _unit = FileCapacityHelper.ONE_MB;
                        break;
                    case SpeedType.GBps:
                        _unit = FileCapacityHelper.ONE_GB;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }
            }
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

            var elapsedSeconds = ElapsedTime.TotalSeconds;
            if (elapsedSeconds > 0)
            {
                BytesPerSecond = reportValue / elapsedSeconds;
            }
        }
        #endregion


        #region Implementation
        #endregion
    }
}