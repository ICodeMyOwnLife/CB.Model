using System.Diagnostics.CodeAnalysis;


namespace CB.Model.Common
{
    public class IdModelBase: ObservableObject
    {
        #region Fields
        private int? _id;
        #endregion


        #region  Constructors & Destructor
        public IdModelBase() { }

        [SuppressMessage("ReSharper", "VirtualMemberCallInContructor")]
        public IdModelBase(IdModelBase model, bool copyId = false)
        {
            CopyFrom(model, copyId);
        }
        #endregion


        #region  Properties & Indexers
        [ToString(OrderIndex = 0)]
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