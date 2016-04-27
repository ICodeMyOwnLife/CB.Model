using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Input;


namespace CB.Model.Common
{
    public interface IViewModelConfiguration
    {
        #region Abstract
        void LoadItems();
        #endregion
    }

    public class ViewModelConfiguration<TViewModel>: IViewModelConfiguration
    {
        #region Fields
        private readonly TViewModel _model;
        private readonly IList<Action<TViewModel>> _propertyInitializers = new List<Action<TViewModel>>();
        #endregion


        #region  Constructors & Destructor
        public ViewModelConfiguration(TViewModel model)
        {
            _model = model;
        }
        #endregion


        #region Methods
        public ViewModelConfiguration<TViewModel> Items<TCollection, TItem, TId>(
            Expression<Func<TViewModel, TCollection>> itemsExpression,
            Func<TCollection> initializeCollection = null,
            Expression<Func<TViewModel, TItem>> selectedItemExpression = null,
            Func<TItem, TId> getItemId = null) where TCollection: IEnumerable<TItem>
        {
            if (itemsExpression != null && initializeCollection != null)
            {
                var itemsPropInfo = GetPropertyInfo(itemsExpression);
                _propertyInitializers.Add(model => { itemsPropInfo.SetValue(model, initializeCollection()); });
            }
            return this;
        }

        public void LoadItems()
        {
            foreach (var initializer in _propertyInitializers) { initializer(_model); }
        }
        #endregion


        #region Implementation
        private static PropertyInfo GetPropertyInfo<TObject, TProperty>(
            Expression<Func<TObject, TProperty>> propertyExpression)
        {
            if (propertyExpression == null) throw new ArgumentNullException(nameof(propertyExpression));

            var memberExpr = propertyExpression.Body as MemberExpression;
            if (memberExpr == null)
                throw new ArgumentException($"{propertyExpression} refers to a method, not a property.");

            var propInfo = memberExpr.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException($"{propertyExpression} refers to a field, not a property.");

            Type objType = typeof(TObject), reflectedType = propInfo.ReflectedType;
            if (reflectedType == null || objType != reflectedType && !objType.IsSubclassOf(reflectedType))
                throw new ArgumentException($"{propertyExpression} refers to a property that is not from type {objType}");

            return propInfo;
        }
        #endregion
    }

    public abstract class IdModelViewModelBase<TModel>: ViewModelBase where TModel: IdModelBase, new()
    {
        #region Fields
        private ICommand _addNewItemCommand;
        private bool _canEdit;
        private ICommand _deleteCommand;
        private ObservableCollection<TModel> _items;
        private ICommand _loadCommand;
        private string _name = typeof(TModel).Name;
        protected string _pluralName = $"{typeof(TModel).Name}s";
        private ICommand _saveCommand;
        private TModel _selectedItem;
        private readonly IViewModelConfiguration _viewModelConfiguration;
        #endregion


        #region  Constructors & Destructor
        [SuppressMessage("ReSharper", "VirtualMemberCallInContructor")]
        protected IdModelViewModelBase()
        {
            _viewModelConfiguration = CreateViewModelConfiguration();
        }
        #endregion


        #region Abstract
        protected abstract bool CanSaveItem(TModel item);
        protected abstract void DeleteItem(int id);
        protected abstract IEnumerable<TModel> LoadItems();
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
            _viewModelConfiguration?.LoadItems();
            Items = LoadItems();
            SelectedItem = Items?.FirstOrDefault();
        }

        public virtual void Save()
        {
            var savedItem = SaveItem(SelectedItem);
            if (savedItem == null) return;

            if (_items.All(i => i.Id != savedItem.Id)) _items.Add(savedItem);

            SelectedItem = savedItem.Id.HasValue ? Items.FirstOrDefault(i => i.Id == savedItem.Id) : null;
        }
        #endregion


        #region Implementation
        protected virtual IViewModelConfiguration CreateViewModelConfiguration()
        {
            return null;
        }

        protected virtual void OnSelectedItemChanged(TModel selectedItem)
        {
            CanEdit = selectedItem != null;
        }
        #endregion
    }
}