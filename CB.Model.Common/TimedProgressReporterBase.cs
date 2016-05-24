using System;
using System.Diagnostics;


namespace CB.Model.Common
{
    public abstract class TimedProgressReporterBase<TReport>: ProgressReporter<double?, TReport>, IReportTimedProgress
    {
        #region Fields
        private const double MINIMUM_PROGRESS_INTERVAL = 0.002786;
        private TimeSpan _elapsedTime;
        private TimeSpan _remainingTime;
        protected readonly Stopwatch _stopwatch = new Stopwatch();
        #endregion


        #region  Properties & Indexers
        public TimeSpan ElapsedTime
        {
            get { return _elapsedTime; }
            protected set { SetProperty(ref _elapsedTime, value); }
        }

        public TimeSpan RemainingTime
        {
            get { return _remainingTime; }
            private set { SetProperty(ref _remainingTime, value); }
        }
        #endregion


        #region Methods
        public virtual void Pause()
            => _stopwatch.Stop();

        public virtual void Reset()
            => _stopwatch.Reset();

        public virtual void Restart()
            => _stopwatch.Restart();

        public virtual void Resume()
            => _stopwatch.Start();

        public virtual void Start()
            => _stopwatch.Start();

        public virtual void Stop()
            => _stopwatch.Stop();
        #endregion


        #region Override
        public override void Report(TReport reportValue)
        {
            var newProgress = GetProgressFromReportValue(reportValue);
            if (newProgress.HasValue &&
                (!Progress.HasValue || Math.Abs(newProgress.Value) < double.Epsilon ||
                 Math.Abs(newProgress.Value - 1) < double.Epsilon ||
                 Math.Abs(newProgress.Value - Progress.Value) > MINIMUM_PROGRESS_INTERVAL))
            {
                SetProgress(newProgress.Value, reportValue);
            }
        }
        #endregion


        #region Implementation
        protected virtual void SetProgress(double newProgress, TReport reportValue)
        {
            Progress = newProgress;
            ElapsedTime = _stopwatch.Elapsed;

            if (Math.Abs(newProgress) < double.Epsilon)
            {
                Restart();
                RemainingTime = TimeSpan.MaxValue;
            }
            else if (Math.Abs(newProgress - 1) < double.Epsilon)
            {
                Reset();
                RemainingTime = TimeSpan.Zero;
            }

            if (Progress > 0)
            {
                RemainingTime = TimeSpan.FromMilliseconds(ElapsedTime.TotalMilliseconds * (1 / Progress.Value - 1));
            }
        }
        #endregion
    }
}