using System;
using System.Linq;
using System.Windows.Input;
using CB.Model.Common;
using Microsoft.Practices.Prism.Commands;
using Prism.Interactivity.InteractionRequest;


namespace CB.Model.Prism
{
    public class PrismViewModelBase: NotifiableViewModelBase
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

        protected virtual ICommand GetCommand(ref ICommand command, Action executeAction, Func<bool> canExecuteFunc = null)
            => command ?? (command = new DelegateCommand(executeAction, canExecuteFunc));

        protected virtual ICommand GetCommand(ref ICommand command, Action<object> executeAction,
            Func<object, bool> canExecuteFunc = null)
            => GetCommand<object>(ref command, executeAction, canExecuteFunc);

        protected virtual ICommand GetCommand<T>(ref ICommand command, Action<T> executeAction,
            Func<T, bool> canExecuteFunc = null)
            => command ?? (command = new DelegateCommand<T>(executeAction, canExecuteFunc));

        protected virtual void Notify(string title, object content, Action<INotification> callback = null)
            => NotificationRequest.Raise(new Notification { Title = title, Content = content }, callback);

        protected virtual void NotifyError(string content, Action<INotification> callback = null)
            => Notify(ErrorTitle ?? CommonErrorTitle, content, callback);

        protected virtual void NotifyWarning(string content, Action<INotification> callback = null)
            => Notify(WarningTitle ?? CommonWarningTitle, content, callback);

        protected void RaiseCommandsCanExecuteChanged(params ICommand[] commands)
        {
            foreach (var command in commands.OfType<DelegateCommandBase>())
            {
                command.RaiseCanExecuteChanged();
            }
        }
        #endregion
    }
}