using System.Linq;
using System.Windows.Input;


namespace CB.Model.Common
{
    public abstract class IdModelViewModelBase<TModel>: ViewModelBase where TModel: IdModelBase, new()
    {
        #region Fields
        private ICommand _addNewItemCommand;
        private bool _canEdit;
        private ICommand _deleteCommand;
        private TModel[] _items;
        private ICommand _loadCommand;
        private string _name = typeof(TModel).Name;
        private ICommand _saveCommand;
        private TModel _selectedItem;
        #endregion


        #region Abstract
        protected abstract bool CanSaveItem(TModel item);
        protected abstract void DeleteItem(int id);
        protected abstract TModel[] LoadItems();
        protected abstract TModel SaveItem(TModel item);
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

        public virtual TModel[] Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }

        public ICommand LoadCommand => GetCommand(ref _loadCommand, _ => Load());

        public virtual string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
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
            if (!SelectedItem.Id.HasValue) return;

            DeleteItem(SelectedItem.Id.Value);
            Load();
        }

        public virtual void Load()
        {
            Items = LoadItems();
            SelectedItem = Items?.Length > 0 ? Items[0] : null;
        }

        public virtual void Save()
        {
            var savedItem = SaveItem(SelectedItem);
            Items = LoadItems();
            SelectedItem = savedItem.Id.HasValue ? Items.FirstOrDefault(i => i.Id == savedItem.Id) : null;
        }
        #endregion


        #region Implementation
        protected virtual void OnSelectedItemChanged(TModel selectedItem)
        {
            CanEdit = selectedItem != null;
        }
        #endregion
    }
}