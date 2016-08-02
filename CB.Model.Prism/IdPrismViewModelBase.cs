using CB.Model.Common;


namespace CB.Model.Prism
{
    public class IdPrismViewModelBase<TId>: PrismModelBase, IIdentification<TId>
    {
        #region Fields
        protected TId _id;
        #endregion


        #region  Properties & Indexers
        public virtual TId Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }
        #endregion
    }
}