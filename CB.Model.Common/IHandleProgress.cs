namespace CB.Model.Common
{
    public interface IHandleProgress
    {
        #region Abstract
        Progress Progress { get; set; }
        #endregion
    }
}