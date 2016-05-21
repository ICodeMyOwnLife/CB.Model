namespace CB.Model.Common
{
    public interface IReportFileProgress
    {
        #region Abstract
        long FileSize { get; set; }

        double BytesPerSecond { get; }
        #endregion
    }
}