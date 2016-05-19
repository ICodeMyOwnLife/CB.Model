using System;


namespace CB.Model.Common
{
    public interface IReportTimedProgress
    {
        #region Abstract
        TimeSpan ElapsedTime { get; }
        TimeSpan RemainingTime { get; }
        #endregion
    }
}