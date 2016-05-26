using System;


namespace CB.Model.Common
{
    public interface ILog
    {
        #region Abstract
        void Log(string logContent);
        void LogError(Exception exception);
        #endregion
    }
}