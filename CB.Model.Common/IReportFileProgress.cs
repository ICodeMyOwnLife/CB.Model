namespace CB.Model.Common
{
    public interface IReportFileProgress
    {
        #region Abstract
        long FileSize { get; set; }

        /// <summary>
        ///     MB/s
        /// </summary>
        double Speed { get; }
        #endregion
    }
}