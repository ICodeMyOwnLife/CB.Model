using System;
using Prism.Interactivity.InteractionRequest;


namespace CB.Model.Prism
{
    namespace CB.Model.Prism
    {
        public class ConfirmationRequestProvider
        {
            #region Fields
            public static string CommonConfirmTitle = "Confirm";
            #endregion


            #region  Properties & Indexers
            public virtual InteractionRequest<IConfirmation> ConfirmationRequest { get; } =
                new InteractionRequest<IConfirmation>();

            public string ConfirmTitle { get; set; }
            #endregion


            #region Methods
            public virtual void Confirm(string title, object content, Action<IConfirmation> callback)
                => ConfirmationRequest.Raise(new Confirmation { Title = title, Content = content }, callback);

            public virtual void Confirm(string content, Action<IConfirmation> callback)
                => Confirm(ConfirmTitle ?? CommonConfirmTitle, content, callback);
            #endregion
        }
    }
}