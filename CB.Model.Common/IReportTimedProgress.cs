using System;


namespace CB.Model.Common
{
    public interface IReportTimedProgress<T>: IReportProgress<T>
    {
        #region Abstract
        TimeSpan ElapsedTime { get; }
        TimeSpan RemainingTime { get; }
        #endregion
    }
}