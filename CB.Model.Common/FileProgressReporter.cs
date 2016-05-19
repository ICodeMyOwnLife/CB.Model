using System;


namespace CB.Model.Common
{
    public class FileProgressReporter: TimedProgressReporterBase<long>, IReportFileProgress
    {
        #region Fields
        private const int ONE_BYTE = 1 << 0;
        private const int ONE_GB = 1 << 30;
        private const int ONE_KB = 1 << 10;
        private const int ONE_MB = 1 << 20;
        private double _speed;
        private SpeedType _speedType;
        private int _unit;
        #endregion


        #region  Constructors & Destructor
        public FileProgressReporter()
        {
            SpeedType = SpeedType.MBps;
        }
        #endregion


        #region  Properties & Indexers
        public long FileSize { get; set; }

        public virtual double Speed
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
                        _unit = ONE_BYTE;
                        break;
                    case SpeedType.KBps:
                        _unit = ONE_KB;
                        break;
                    case SpeedType.MBps:
                        _unit = ONE_MB;
                        break;
                    case SpeedType.GBps:
                        _unit = ONE_GB;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }
            }
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

            var elapsedSeconds = ElapsedTime.TotalSeconds;
            if (elapsedSeconds > 0)
            {
                Speed = reportValue / elapsedSeconds / _unit;
            }
        }
        #endregion
    }
}