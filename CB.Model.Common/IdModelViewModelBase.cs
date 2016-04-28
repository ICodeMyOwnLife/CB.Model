using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Input;


namespace CB.Model.Common
{
    public abstract class ConfiguredViewModelBase<TModel>: ViewModelBase
    {
        #region Fields
        private readonly IList<Action> _collectionInitializers = new List<Action>();
        private readonly IList<Action<TModel>> _selectedItemSelectors = new List<Action<TModel>>();
        #endregion


        #region Methods
        public virtual ConfiguredViewModelBase<TModel> Collection<TCollection, TElement, TId>(
            Expression<Func<TCollection>> collectionPropertyExpression,
            Expression<Func<TElement>> selectedElementExpression, Func<TCollection> collectionInitializer,
            Func<TModel, TElement> modelElementGetter, Func<TElement, TId> elementIdGetter)
            where TCollection: class, IEnumerable<TElement> where TElement: class
        {
            if (collectionPropertyExpression == null || collectionInitializer == null) return this;

            var itemsPropInfo = GetPropertyInfo(collectionPropertyExpression);
            _collectionInitializers.Add(() => itemsPropInfo.SetValue(this, collectionInitializer()));

            if (selectedElementExpression == null || modelElementGetter == null || elementIdGetter == null) return this;

            var selectedPropInfo = GetPropertyInfo(selectedElementExpression);
            _selectedItemSelectors.Add(selectedItem =>
            {
                var selectedModelItem = modelElementGetter(selectedItem);
                var selectedValue = default(TElement);

                if (selectedModelItem != null && elementIdGetter(selectedModelItem) != null)
                {
                    var items = itemsPropInfo.GetValue(this) as TCollection;
                    selectedValue =
                        items?.FirstOrDefault(i => elementIdGetter(i).Equals(elementIdGetter(selectedModelItem)));
                }

                selectedPropInfo.SetValue(this, selectedValue);
            });
            return this;
        }

        public virtual void LoadCollections()
        {
            foreach (var initializer in _collectionInitializers) { initializer(); }
        }

        public virtual void SetSelectedElements(TModel selectedItem)
        {
            foreach (var selector in _selectedItemSelectors)
            {
                selector(selectedItem);
            }
        }
        #endregion


        #region Implementation
        private PropertyInfo GetPropertyInfo<TProperty>(
            Expression<Func<TProperty>> propertyExpression)
        {
            if (propertyExpression == null) throw new ArgumentNullException(nameof(propertyExpression));

            var memberExpr = propertyExpression.Body as MemberExpression;
            if (memberExpr == null)
                throw new ArgumentException($"{propertyExpression} refers to a method, not a property.");

            var propInfo = memberExpr.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException($"{propertyExpression} refers to a field, not a property.");

            Type objType = GetType(), reflectedType = propInfo.ReflectedType;
            if (reflectedType == null || objType != reflectedType && !objType.IsSubclassOf(reflectedType))
                throw new ArgumentException($"{propertyExpression} refers to a property that is not from type {objType}");

            return propInfo;
        }
        #endregion
    }

    public abstract class IdModelViewModelBase<TModel>: ConfiguredViewModelBase<TModel> where TModel: IdModelBase, new()
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
            LoadCollections();
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
        protected virtual void OnSelectedItemChanged(TModel selectedItem)
        {
            CanEdit = selectedItem != null;
            SetSelectedElements(selectedItem);
        }
        #endregion
    }
}