using System;


namespace CB.Model.Common
{
    public interface IReportProgress<out T>
    {
        #region Abstract
        T Progress { get; }
        #endregion
    }
}