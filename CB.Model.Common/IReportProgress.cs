using System;


namespace CB.Model.Common
{
    public interface IReportProgress<T>: IProgress<T>
    {
        #region Abstract
        T Progress { get; }
        #endregion
    }
}