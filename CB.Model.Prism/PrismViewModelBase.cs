using System;
using CB.Model.Common;
using Prism.Interactivity.InteractionRequest;


namespace CB.Model.Prism
{
    public class PrismViewModelBase: ObservableObject
    {
        #region Fields
        public static string CommonConfirmTitle = "Confirm";
        public static string CommonErrorTitle = "Error";
        public static string CommonWarningTitle = "Warning";
        #endregion


        #region  Properties & Indexers
        public virtual InteractionRequest<IConfirmation> ConfirmationRequest { get; } =
            new InteractionRequest<IConfirmation>();

        public string ConfirmTitle { get; set; }

        public string ErrorTitle { get; set; }

        public virtual InteractionRequest<INotification> NotificationRequest { get; } =
            new InteractionRequest<INotification>();

        public string WarningTitle { get; set; }
        #endregion


        #region Implementation
        protected virtual void Confirm(string title, object content, Action<IConfirmation> callback)
            => ConfirmationRequest.Raise(new Confirmation { Title = title, Content = content }, callback);

        protected virtual void Confirm(string content, Action<IConfirmation> callback)
            => Confirm(ConfirmTitle ?? CommonConfirmTitle, content, callback);

        protected virtual void Notify(string title, object content, Action<INotification> callback)
            => NotificationRequest.Raise(new Notification { Title = title, Content = content }, callback);

        protected virtual void NotifyError(string content, Action<INotification> callback = null)
            => Notify(ErrorTitle ?? CommonErrorTitle, content, callback);

        protected virtual void NotifyWarning(string content, Action<INotification> callback = null)
            => Notify(WarningTitle ?? CommonWarningTitle, content, callback);
        #endregion
    }
}