using System;
using System.Collections;
using System.Windows.Data;


namespace CB.Model.Common
{
    public class CollectionModelBase<TModel>: BindableObject
    {
        #region Fields
        private IList _collection;
        private TModel _selectedItem;
        #endregion


        #region  Constructors & Destructor
        public CollectionModelBase() { }

        public CollectionModelBase(IList collection)
        {
            // ReSharper disable once VirtualMemberCallInContructor
            Collection = collection;
        }
        #endregion


        #region  Properties & Indexers
        public virtual IList Collection
        {
            get { return _collection; }
            set
            {
                if (!SetProperty(ref _collection, value)) return;

                CollectionView = new ListCollectionView(value);
                CollectionView.CurrentChanged += View_CurrentChanged;
            }
        }

        public virtual ListCollectionView CollectionView { get; protected set; }

        public virtual TModel SelectedItem
        {
            get { return _selectedItem; }
            protected set { SetProperty(ref _selectedItem, value); }
        }
        #endregion


        #region Event Handlers
        protected virtual void View_CurrentChanged(object sender, EventArgs e)
            => SelectedItem = (TModel)CollectionView.CurrentItem;
        #endregion
    }
}