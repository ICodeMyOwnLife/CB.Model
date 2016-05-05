namespace CB.Model.Common
{
    public abstract class IdEntityBase: ObservableObject, IIdEntity
    {
        #region Fields
        private int _id;
        #endregion


        #region  Properties & Indexers
        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }
        #endregion


        #region Methods
        public static TIdEntity Copy<TIdEntity>(TIdEntity entity, bool copyId) where TIdEntity: IdEntityBase, new()
        {
            var copy = new TIdEntity();
            copy.CopyFrom(entity, copyId);
            return copy;
        }

        public virtual void CopyFrom(IdEntityBase other, bool copyId = false)
        {
            if (copyId) Id = other.Id;
        }
        #endregion
    }
}