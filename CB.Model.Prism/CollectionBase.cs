using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Data;


namespace CB.Model.Prism
{
    public class CollectionBase<TItem, TCollection>: PrismViewModelBase where TCollection: IList, IEnumerable<TItem>
    {
        #region Fields
        private TCollection _collection;
        private ListCollectionView _collectionView;
        private TItem _selectedItem;
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

        public virtual ListCollectionView CollectionView
        {
            get { return _collectionView; }
            protected set { SetProperty(ref _collectionView, value); }
        }

        public virtual TItem SelectedItem
        {
            get { return _selectedItem; }
            protected set { SetProperty(ref _selectedItem, value); }
        }
        #endregion


        #region Methods
        public virtual void Add(TItem item)
        {
            Collection.Add(item);
            Select(item);
        }

        public virtual void Remove(TItem item)
            => Collection.Remove(item);

        public virtual void RemoveItem()
            => Remove(SelectedItem);

        public virtual void Select(TItem item)
            => CollectionView.MoveCurrentTo(item);
        #endregion


        #region Event Handlers
        protected virtual void View_CurrentChanged(object sender, EventArgs e)
            => SelectedItem = CollectionView.CurrentItem is TItem ? (TItem)CollectionView.CurrentItem : default(TItem);
        #endregion
    }
}