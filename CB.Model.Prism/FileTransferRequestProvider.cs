using CB.Model.Common;
using Prism.Interactivity.InteractionRequest;


namespace CB.Model.Prism
{
    /*public class FileTransferRequestProvider
    {
        #region  Properties & Indexers
        public virtual InteractionRequest<IFileTransferReporting> FileTransfeRequest { get; } =
            new InteractionRequest<IFileTransferReporting>();
        #endregion

        public virtual void StartTransfer(FileProgressReporter fileProgressReporter)
            => FileTransfeRequest.Raise(fileProgressReporter)
    }*/

    public interface IFileTransferReporting: INotification
    {
        #region Abstract
        FileProgressReporter FileProgressReporter { get; set; }
        #endregion
    }
}