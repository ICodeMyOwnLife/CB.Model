using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Data;


namespace CB.Model.Common
{
    public class ModelCollectionBase<TModel, TCollection>: BindableObject where TCollection: IList, IEnumerable<TModel>
    {
        #region Fields
        private TCollection _collection;

        private ListCollectionView _collectionView;
        private TModel _selectedItem;
        #endregion


        #region  Constructors & Destructor
        public ModelCollectionBase() { }

        public ModelCollectionBase(TCollection collection)
        {
            // ReSharper disable once VirtualMemberCallInContructor
            Collection = collection;
        }
        #endregion


        #region  Properties & Indexers
        public virtual TCollection Collection
        {
            get { return _collection; }
            set
            {
                if (!SetProperty(ref _collection, value)) return;

                CollectionView = new ListCollectionView(value);
                CollectionView.CurrentChanged += View_CurrentChanged;
            }
        }

        public ListCollectionView CollectionView
        {
            get { return _collectionView; }
            protected set { SetProperty(ref _collectionView, value); }
        }

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