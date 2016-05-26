namespace CB.Model.Common
{
    public interface IReportFileProgress: IReportProgress<double?>
    {
        #region Abstract
        double BytesPerSecond { get; }
        string Capacity { get; }
        string FileName { get; set; }
        long FileSize { get; set; }
        string Rate { get; }
        string Transferred { get; }
        #endregion
    }
}