using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;


namespace CB.Model.Common
{
    public class ViewModelConfiguration<TViewModel, TModel>: IViewModelConfiguration<TModel>
        where TViewModel: IdModelViewModelBase<TModel> where TModel: IdModelBase, new()
    {
        #region Fields
        private readonly IList<Action<TViewModel>> _propertyInitializers = new List<Action<TViewModel>>();
        private readonly IList<Action<TModel>> _selectedItemSelectors = new List<Action<TModel>>();
        private readonly TViewModel _viewModel;
        #endregion


        #region  Constructors & Destructor
        public ViewModelConfiguration(TViewModel viewModel)
        {
            _viewModel = viewModel;
        }
        #endregion


        #region Methods
        public ViewModelConfiguration<TViewModel, TModel> Items<TCollection, TItem, TId>(
            Expression<Func<TViewModel, TCollection>> itemsExpression,
            Func<TCollection> initializeCollection = null, Func<TModel, TItem> getModelItem = null,
            Expression<Func<TViewModel, TItem>> selectedItemExpression = null,
            Func<TItem, TId> getItemId = null) where TCollection: class, IEnumerable<TItem> where TItem: class
        {
            if (itemsExpression == null || initializeCollection == null) return this;

            var itemsPropInfo = GetPropertyInfo(itemsExpression);
            _propertyInitializers.Add(model => itemsPropInfo.SetValue(model, initializeCollection()));

            if (selectedItemExpression == null || getModelItem == null || getItemId == null) return this;

            var selectedPropInfo = GetPropertyInfo(selectedItemExpression);
            _selectedItemSelectors.Add(selectedItem =>
            {
                var selectedModelItem = getModelItem(selectedItem);
                TItem selectedValue = null;
                if (selectedModelItem != null && getItemId(selectedModelItem) != null)
                {
                    var items = itemsPropInfo.GetValue(_viewModel) as TCollection;
                    selectedValue = items?.FirstOrDefault(i => getItemId(i).Equals(getItemId(selectedModelItem)));
                }

                selectedPropInfo.SetValue(_viewModel, selectedValue);
            });
            return this;
        }

        public void LoadItems()
        {
            foreach (var initializer in _propertyInitializers) { initializer(_viewModel); }
        }

        public void SetSelectedItems(TModel selectedItem)
        {
            foreach (var selector in _selectedItemSelectors)
            {
                selector(selectedItem);
            }
        }
        #endregion


        #region Implementation
        private static PropertyInfo GetPropertyInfo(LambdaExpression propertyExpression)
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
}