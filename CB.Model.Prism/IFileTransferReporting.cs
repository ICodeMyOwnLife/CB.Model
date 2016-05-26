using CB.Model.Common;
using Prism.Interactivity.InteractionRequest;


namespace CB.Model.Prism
{
    public interface IFileTransferReporting: INotification
    {
        #region Abstract
        FileProgressReporter FileProgressReporter { get; set; }
        #endregion
    }
}