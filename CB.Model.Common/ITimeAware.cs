using System;


namespace CB.Model.Common
{
    public interface ITimeAware
    {
        #region Abstract
        long ElapsedMilliseconds { get; }
        long ElapsedTicks { get; }
        TimeSpan ElapsedTime { get; }
        TimeSpan Interval { get; set; }
        TimeSpan RemainingTime { get; }
        #endregion
    }
}