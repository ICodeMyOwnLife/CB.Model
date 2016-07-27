using System;
using System.Diagnostics;
using System.Windows.Threading;


namespace CB.Model.Common
{
    public class StopwatchTimer: BindableObject, ITimeAware
    {
        #region Fields
        private readonly DispatcherTimer _mainTimer = new DispatcherTimer();
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private TimeSpan? _updateInterval;
        private DispatcherTimer _updateTimer;
        #endregion


        #region  Constructors & Destructor
        public StopwatchTimer(TimeSpan interval): this()
        {
            Interval = interval;
        }

        public StopwatchTimer(TimeSpan interval, TimeSpan updateInterval): this(interval)
        {
            UpdateInterval = updateInterval;
        }

        public StopwatchTimer()
        {
            _stopwatch.Start();
        }
        #endregion


        #region  Properties & Indexers
        public long ElapsedMilliseconds => _stopwatch.ElapsedMilliseconds;
        public long ElapsedTicks => _stopwatch.ElapsedTicks;
        public TimeSpan ElapsedTime => _stopwatch.Elapsed;

        public TimeSpan Interval
        {
            get { return _mainTimer.Interval; }
            set { _mainTimer.Interval = value; }
        }

        public TimeSpan RemainingTime => _mainTimer.Interval - _stopwatch.Elapsed;

        public TimeSpan? UpdateInterval
        {
            get { return _updateInterval; }

            set
            {
                _updateInterval = value;
                if (value.HasValue)
                {
                    if (_updateTimer == null)
                    {
                        _updateTimer = new DispatcherTimer();
                        _updateTimer.Tick += UpdateTimer_Tick;
                    }

                    _updateTimer.Interval = value.Value;
                    _updateTimer.Start();
                }
                else
                {
                    _updateTimer.Stop();
                }
            }
        }
        #endregion


        #region Events
        public event EventHandler Tick
        {
            add { _mainTimer.Tick += value; }
            remove { _mainTimer.Tick -= value; }
        }
        #endregion


        #region Methods
        public void Start()
        {
            _mainTimer.Start();
            _stopwatch.Restart();
        }

        public void Stop()
        {
            _mainTimer.Stop();
            _stopwatch.Reset();
        }
        #endregion


        #region Event Handlers
        private void UpdateTimer_Tick(object sender, EventArgs e)
            => NotifyPropertiesChanged(nameof(ElapsedMilliseconds), nameof(ElapsedTicks), nameof(ElapsedTime),
                nameof(RemainingTime));
        #endregion
    }
}