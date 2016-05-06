using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;


namespace CB.Model.Common
{
    public abstract class IdEntityViewModelBase<TEntity>: ConfiguredViewModelBase<TEntity>
        where TEntity: IdEntityBase, new()
    {
        #region Fields
        private ICommand _addNewItemCommand;
        private bool _canEdit;
        private bool _collectionsLoaded;
        private ICommand _copyCommand;
        private ICommand _deleteCommand;
        private ObservableCollection<TEntity> _items;
        private ListCollectionView _itemsView;
        private ICommand _loadCommand;
        private Action<int> _modelDeleterById;
        private string _name = typeof(TEntity).Name;
        protected string _pluralName = $"{typeof(TEntity).Name}s";
        private ICommand _saveCommand;
        private TEntity _selectedItem;
        #endregion


        #region Abstract
        protected abstract bool CanSaveItem(TEntity item);
        #endregion


        #region  Properties & Indexers
        public virtual ICommand AddNewItemCommand => GetCommand(ref _addNewItemCommand, _ => AddNewItem());

        public bool CanEdit
        {
            get { return _canEdit; }
            private set { SetProperty(ref _canEdit, value); }
        }

        public ICommand CopyCommand => GetCommand(ref _copyCommand, _ => Copy(), _ => SelectedItem != null);

        public virtual ICommand DeleteCommand
            => GetCommand(ref _deleteCommand, _ => Delete(), _ => SelectedItem?.Id != null);

        public virtual IEnumerable<TEntity> Items
        {
            get { return _items; }
            protected set
            {
                if (SetProperty(ref _items,
                    value as ObservableCollection<TEntity> ?? new ObservableCollection<TEntity>(value)))
                {
                    ItemsView = _items == null ? null : new ListCollectionView(_items);
                }
            }
        }

        public ListCollectionView ItemsView
        {
            get { return _itemsView; }
            private set { SetProperty(ref _itemsView, value); }
        }

        public ICommand LoadCommand => GetCommand(ref _loadCommand, _ => Load());

        public virtual string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public virtual string PluralName
        {
            get { return _pluralName; }
            set { SetProperty(ref _pluralName, value); }
        }

        public virtual ICommand SaveCommand
            => GetCommand(ref _saveCommand, _ => Save(), _ => CanSaveItem(SelectedItem));

        public virtual TEntity SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (!SetProperty(ref _selectedItem, value)) return;
                OnSelectedItemChanged(value);
            }
        }
        #endregion


        #region Methods
        public virtual void AddNewItem()
        {
            SelectedItem = new TEntity();
            if (!_collectionsLoaded) LoadCollections();
        }

        public virtual void Copy()
        {
            if (SelectedItem == null)
            {
                AddNewItem();
            }
            else
            {
                var newItem = new TEntity();
                newItem.CopyFrom(SelectedItem, false);
                SelectedItem = newItem;
            }
        }

        public virtual void Delete()
        {
            if (SelectedItem?.Id == null) return;

            DeleteItem(SelectedItem.Id);
            _items.Remove(_items?.FirstOrDefault(i => i.Id == SelectedItem.Id));
            SelectedItem = Items?.FirstOrDefault();
        }

        public virtual void Load()
        {
            LoadCollections();
            _collectionsLoaded = true;
            Items = LoadItems();
            SelectedItem = Items?.FirstOrDefault();
        }

        public virtual void Save()
        {
            var savedItem = SaveItem(SelectedItem);
            if (Items == null) Items = LoadItems();
            if (savedItem == null) return;

            SelectedItem.CopyFrom(savedItem, true);
            if (_items.All(i => i.Id != savedItem.Id)) _items.Add(savedItem);

            SelectedItem = savedItem.Id > 0 ? Items.FirstOrDefault(i => i.Id == savedItem.Id) : null;
        }
        #endregion


        #region Implementation
        protected virtual void DeleteItem(TEntity item)
            => DeleteModel(item);

        protected virtual void DeleteItem(int id)
            => DeleteModel(id);

        protected void DeleteModel(int id)
            => _modelDeleterById?.Invoke(id);

        protected virtual IEnumerable<TEntity> LoadItems()
        {
            return LoadModels();
        }

        protected ConfiguredViewModelBase<TEntity> ModelDeleter(Action<int> modelDeleterById)
        {
            _modelDeleterById = modelDeleterById;
            return this;
        }

        protected virtual void OnSelectedItemChanged(TEntity selectedItem)
        {
            CanEdit = selectedItem != null;
            SetSelectedElements(selectedItem);
        }

        protected virtual TEntity SaveItem(TEntity item)
        {
            return SaveModel(item);
        }
        #endregion
    }
}