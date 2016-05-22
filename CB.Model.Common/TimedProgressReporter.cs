namespace CB.Model.Common
{
    public class TimedProgressReporter: TimedProgressReporterBase<double>
    {
        #region Override
        protected override double? GetProgressFromReportValue(double value)
        {
            return value;
        }
        #endregion
    }
}