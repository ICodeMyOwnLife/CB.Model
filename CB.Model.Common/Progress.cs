using System.ComponentModel;
using System.Globalization;


namespace CB.Model.Common
{
    [TypeConverter(typeof(ProgressConverter))]
    public struct Progress
    {
        public double Value { get; }
        public bool IsIndeterminate { get; }
        public bool IsRunning { get; }

        public Progress(double? value)
        {
            if (value.HasValue)
            {
                IsRunning = true;
                if (double.IsNaN(value.Value))
                {
                    IsIndeterminate = true;
                    Value = 0;
                }
                else
                {
                    IsIndeterminate = false;
                    Value = value.Value;
                }
            }
            else
            {
                IsRunning = false;
                IsIndeterminate = false;
                Value = 0;
            }
        }

        public static Progress IndeterminateProgress
            => new Progress(double.NaN);

        public static Progress NotRunningProgress
            => new Progress(null);

        public static Progress FromValue(double value)
            => new Progress(value);

        public static implicit operator Progress(double? progress)
            => new Progress(progress);

        public static implicit operator double?(Progress progress)
            => !progress.IsRunning ? (double?)null : progress.IsIndeterminate ? double.NaN : progress.Value;

        public string ToString(string format, CultureInfo culture)
            => !IsRunning ? "Not Running" : IsIndeterminate ? "Indeterminate" : Value.ToString(format, culture);

        public override string ToString()
            => ToString("", CultureInfo.CurrentCulture);
    }
}