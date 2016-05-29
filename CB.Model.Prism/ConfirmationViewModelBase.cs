using Prism.Interactivity.InteractionRequest;


namespace CB.Model.Prism
{
    public class ConfirmationViewModelBase: PrismViewModelBase, IConfirmation
    {
        #region Fields
        private bool _confirmed;
        private object _content;
        private string _title;
        #endregion


        #region  Properties & Indexers
        public bool Confirmed
        {
            get { return _confirmed; }
            set { SetProperty(ref _confirmed, value); }
        }

        public object Content
        {
            get { return _content; }
            set { SetProperty(ref _content, value); }
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        #endregion
    }
}