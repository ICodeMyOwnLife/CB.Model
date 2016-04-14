namespace CB.Model.Common
{
    public class IdModelBase: ObservableObject
    {
        #region Fields
        private int? _id;
        #endregion


        #region  Properties & Indexers
        public int? Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }
        #endregion


        #region Methods
        public virtual void CopyFrom(IdModelBase obj, bool copyId)
        {
            if (copyId) Id = obj.Id;
        }
        #endregion
    }
}