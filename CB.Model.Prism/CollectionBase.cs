using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Windows.Data;
using CB.Model.Common;


namespace CB.Model.Prism
{
    public class CollectionBase<TItem, TCollection>: PrismViewModelBase where TCollection: IList, IEnumerable<TItem>
    {
        #region Fields
        private TCollection _collection;
        private ListCollectionView _collectionView;
        private TItem _selectedItem;
        #endregion


        #region  Constructors & Destructor

        public CollectionBase(TCollection collection)
        {
            // ReSharper disable once VirtualMemberCallInContructor
            Collection = collection;
        }
        #endregion


        #region  Properties & Indexers
        public virtual TCollection Collection
        {
            get { return _collection; }
            private set
            {
                if (!SetProperty(ref _collection, value)) return;

                CollectionView = new ListCollectionView(value);
                CurrentChanged += OnCurrentChanged;
            }
        }

        public virtual ListCollectionView CollectionView
        {
            get { return _collectionView; }
            private set { SetProperty(ref _collectionView, value); }
        }

        public virtual TItem SelectedItem
        {
            get { return _selectedItem; }
            protected set { SetProperty(ref _selectedItem, value); }
        }
        #endregion


        #region Events
        public event EventHandler CurrentChanged
        {
            add { CollectionView.CurrentChanged += value; }
            remove { CollectionView.CurrentChanged -= value; }
        }
        #endregion


        #region Methods
        public virtual void Add(TItem item)
        {
            Collection.Add(item);
            Select(item);
        }

        public virtual void Clear()
            => Collection.Clear();

        public virtual void Remove(TItem item)
            => Collection.Remove(item);

        public virtual void RemoveItem()
            => Remove(SelectedItem);

        public virtual void Select(TItem item)
            => CollectionView.MoveCurrentTo(item);

        public virtual void Sort<TProperty>(Expression<Func<TItem, TProperty>> propertyExpression,
            ListSortDirection direction)
            => Sort(propertyExpression.GetPropertyName(), direction);

        public virtual void Sort(string propertyName, ListSortDirection direction)
        {
            CollectionView.SortDescriptions.Clear();
            CollectionView.SortDescriptions.Add(new SortDescription(propertyName, direction));
        }
        #endregion


        #region Event Handlers
        protected virtual void OnCurrentChanged(object sender, EventArgs e)
            => SelectedItem = CollectionView.CurrentItem is TItem ? (TItem)CollectionView.CurrentItem : default(TItem);
        #endregion
    }
}