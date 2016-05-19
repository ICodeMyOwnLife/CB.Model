using System;
using System.Diagnostics;


namespace CB.Model.Common
{
    public class TimedProgressReporter: ProgressReporter<double>, IReportTimedProgress<double>
    {
        #region Fields
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


        #region Override
        public override void Report(double value)
        {
            base.Report(value);
            ElapsedTime = _stopwatch.Elapsed;

            if (Math.Abs(value) < double.Epsilon)
            {
                _stopwatch.Restart();
                RemainingTime = TimeSpan.MaxValue;
            }
            else if (Math.Abs(value - 1) < double.Epsilon)
            {
                _stopwatch.Reset();
                RemainingTime = TimeSpan.Zero;
            }
            else
            {
                RemainingTime = TimeSpan.FromMilliseconds(ElapsedTime.TotalMilliseconds * (1 - 1 / value));
            }
        }
        #endregion
    }
}