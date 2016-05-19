using System;
using Prism.Interactivity.InteractionRequest;


namespace CB.Model.Prism
{
    public class NotificationRequestProvider
    {
        #region Fields
        public static string CommonErrorTitle = "Error";
        public static string CommonWarningTitle = "Warning";
        #endregion


        #region  Properties & Indexers
        public virtual string ErrorTitle { get; set; }

        public virtual InteractionRequest<INotification> NotificationRequest { get; } =
            new InteractionRequest<INotification>();

        public virtual string WarningTitle { get; set; }
        #endregion


        #region Methods
        public virtual void Notify(string title, object content, Action<INotification> callback = null)
            => NotificationRequest.Raise(new Notification { Title = title, Content = content }, callback);

        public virtual void NotifyError(string content, Action<INotification> callback = null)
            => Notify(ErrorTitle ?? CommonErrorTitle, content, callback);

        public virtual void NotifyWarning(string content, Action<INotification> callback = null)
            => Notify(WarningTitle ?? CommonWarningTitle, content, callback);
        #endregion
    }
}