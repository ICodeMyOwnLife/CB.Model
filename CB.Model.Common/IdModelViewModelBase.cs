using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;


namespace CB.Model.Common
{
    public abstract class IdModelViewModelBase<TModel>: ConfiguredViewModelBase<TModel> where TModel: IdModelBase, new()
    {
        #region Fields
        private ICommand _addNewItemCommand;
        private bool _canEdit;
        private ICommand _deleteCommand;
        private ObservableCollection<TModel> _items;
        private ICommand _loadCommand;
        private Action<int> _modelDeleterById;
        private string _name = typeof(TModel).Name;
        protected string _pluralName = $"{typeof(TModel).Name}s";
        private ICommand _saveCommand;
        private TModel _selectedItem;
        #endregion


        #region Abstract
        protected abstract bool CanSaveItem(TModel item);
        #endregion


        #region  Properties & Indexers
        public virtual ICommand AddNewItemCommand => GetCommand(ref _addNewItemCommand, _ => AddNewItem());

        public bool CanEdit
        {
            get { return _canEdit; }
            private set { SetProperty(ref _canEdit, value); }
        }

        public virtual ICommand DeleteCommand
            => GetCommand(ref _deleteCommand, _ => Delete(), _ => SelectedItem?.Id != null);

        public virtual IEnumerable<TModel> Items
        {
            get { return _items; }
            protected set
            {
                SetProperty(ref _items, value as ObservableCollection<TModel> ?? new ObservableCollection<TModel>(value));
            }
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

        public virtual TModel SelectedItem
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
            => SelectedItem = new TModel();

        public virtual void Delete()
        {
            if (SelectedItem?.Id == null) return;

            DeleteItem(SelectedItem.Id.Value);
            _items.Remove(_items?.FirstOrDefault(i => i.Id != null && i.Id.Value == SelectedItem.Id.Value));
            SelectedItem = Items?.FirstOrDefault();
        }

        public virtual void Load()
        {
            LoadCollections();
            Items = LoadItems();
            SelectedItem = Items?.FirstOrDefault();
        }

        public virtual void Save()
        {
            var savedItem = SaveItem(SelectedItem);
            if (savedItem == null) return;

            SelectedItem.CopyFrom(savedItem, true);
            if (_items.All(i => i.Id != savedItem.Id)) _items.Add(savedItem);

            SelectedItem = savedItem.Id.HasValue ? Items.FirstOrDefault(i => i.Id == savedItem.Id) : null;
        }
        #endregion


        #region Implementation
        protected virtual void DeleteItem(TModel item)
            => DeleteModel(item);

        protected virtual void DeleteItem(int id)
            => DeleteModel(id);

        protected void DeleteModel(int id)
            => _modelDeleterById?.Invoke(id);

        protected virtual IEnumerable<TModel> LoadItems()
        {
            return LoadModels();
        }

        protected virtual ConfiguredViewModelBase<TModel> ModelDeleter(Action<int> modelDeleterById)
        {
            _modelDeleterById = modelDeleterById;
            return this;
        }

        protected virtual void OnSelectedItemChanged(TModel selectedItem)
        {
            CanEdit = selectedItem != null;
            SetSelectedElements(selectedItem);
        }

        protected virtual TModel SaveItem(TModel item)
        {
            return SaveModel(item);
        }
        #endregion
    }
}


// Add before Load (LoadCollections() after Add(), LoadItems() after SaveItem())