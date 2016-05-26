using System;
using Prism.Interactivity.InteractionRequest;


namespace CB.Model.Prism
{
    namespace CB.Model.Prism
    {
        public class ConfirmationRequestProvider: RequestProviderBase<IConfirmation>
        {
            #region Fields
            public static string CommonConfirmTitle = "Confirm";
            #endregion


            #region  Properties & Indexers
            public string ConfirmTitle { get; set; }

            public override InteractionRequest<IConfirmation> Request { get; } =
                new InteractionRequest<IConfirmation>();
            #endregion


            #region Methods
            public virtual void Confirm(string title, object content, Action<IConfirmation> callback)
                => Request.Raise(new Confirmation { Title = title, Content = content }, callback);

            public virtual void Confirm(string content, Action<IConfirmation> callback)
                => Confirm(ConfirmTitle ?? CommonConfirmTitle, content, callback);

            public virtual void ConfirmOnUiThread(string title, object content, Action<IConfirmation> callback)
                => RunOnUiThread(() => Confirm(title, content, callback));

            public virtual void ConfirmOnUiThread(string content, Action<IConfirmation> callback)
                => RunOnUiThread(() => Confirm(content, callback));
            #endregion
        }
    }
}