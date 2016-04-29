using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;


namespace CB.Model.Common
{
    public abstract class ConfiguredViewModelBase<TModel>: ViewModelBase
    {
        #region Fields
        private readonly IList<Action<TModel>> _afterSaveItemModelSetters = new List<Action<TModel>>();
        private readonly IList<Action<TModel>> _beforeSaveItemModelSetters = new List<Action<TModel>>();
        private readonly IList<Action> _collectionInitializers = new List<Action>();
        private Action<TModel> _modelDeleter;
        private Func<TModel, TModel> _modelSaver;
        private Func<IEnumerable<TModel>> _modelsLoader;
        private readonly IList<Action<TModel>> _selectedElementSetters = new List<Action<TModel>>();
        #endregion


        #region Implementation
        private void AddAfterSaveItemModelSetter<TElement>(Expression<Func<TElement>> selectedElementExpression,
            Expression<Func<TModel, TElement>> modelElementExpression)
        {
            if (selectedElementExpression == null || modelElementExpression == null) return;

            _afterSaveItemModelSetters.Add(item =>
            {
                var selectedElementProp = GetPropertyInfo(selectedElementExpression);
                var modelElementProp = GetPropertyInfo(modelElementExpression);
                var selectedElement = (TElement)selectedElementProp.GetValue(this);
                modelElementProp.SetValue(item, selectedElement);
            });
        }

        private void AddBeforeSaveItemModelSetter<TElement, TId>(Expression<Func<TElement>> selectedElementExpression,
            Expression<Func<TModel, TElement>> modelElementExpression,
            Expression<Func<TModel, TId>> modelElementIdExpression, Func<TElement, TId> elementIdGetter)
        {
            if (selectedElementExpression == null || modelElementExpression == null || modelElementIdExpression == null ||
                elementIdGetter == null) return;
            _beforeSaveItemModelSetters.Add(item =>
            {
                var selectedElementProp = GetPropertyInfo(selectedElementExpression);
                var modelElementProp = GetPropertyInfo(modelElementExpression);
                var selectedElement = (TElement)selectedElementProp.GetValue(this);

                modelElementProp.SetValue(item, null);
                var modelElementIdProp1 = GetPropertyInfo(modelElementIdExpression);
                modelElementIdProp1.SetValue(item, elementIdGetter(selectedElement));
            });
        }

        private void AddCollectionInitializer<TCollection>(Expression<Func<TCollection>> collectionExpression,
            Func<TCollection> collectionInitializer)
        {
            if (collectionExpression == null || collectionInitializer == null) return;

            _collectionInitializers.Add(() =>
            {
                var collectionProp = GetPropertyInfo(collectionExpression);
                collectionProp.SetValue(this, collectionInitializer());
            });
        }

        private void AddSelectedElementSetter<TCollection, TElement, TId>(
            Expression<Func<TCollection>> collectionExpression,
            Expression<Func<TElement>> selectedElementExpression, Expression<Func<TModel, TId>> modelElementIdExpression,
            Func<TElement, TId> elementIdGetter) where TCollection: IEnumerable<TElement> where TElement: class
        {
            if (collectionExpression == null || selectedElementExpression == null || modelElementIdExpression == null ||
                elementIdGetter == null) return;

            _selectedElementSetters.Add(selectedItem =>
            {
                PropertyInfo selectedElementProp = GetPropertyInfo(selectedElementExpression),
                             modelElementIdProp = GetPropertyInfo(modelElementIdExpression),
                             collectionProp = GetPropertyInfo(collectionExpression);

                var selectedModelElementId = (TId)modelElementIdProp.GetValue(selectedItem);

                var items = (TCollection)collectionProp.GetValue(this);

                var selectedValue = items?.FirstOrDefault(i => Equals(elementIdGetter(i), selectedModelElementId));

                selectedElementProp.SetValue(this, selectedValue);
            });
        }

        protected virtual ConfiguredViewModelBase<TModel> Collection<TCollection, TElement, TId>(
            Expression<Func<TCollection>> collectionExpression,
            Func<TCollection> collectionInitializer,
            Expression<Func<TElement>> selectedElementExpression,
            Expression<Func<TModel, TElement>> modelElementExpression,
            Expression<Func<TModel, TId>> modelElementIdExpression,
            Func<TElement, TId> elementIdGetter)
            where TCollection: class, IEnumerable<TElement> where TElement: class, new()
        {
            AddCollectionInitializer(collectionExpression, collectionInitializer);

            AddSelectedElementSetter(collectionExpression, selectedElementExpression, modelElementIdExpression,
                elementIdGetter);

            AddBeforeSaveItemModelSetter(selectedElementExpression, modelElementExpression,
                modelElementIdExpression, elementIdGetter);

            AddAfterSaveItemModelSetter(selectedElementExpression, modelElementExpression);

            return this;
        }

        protected virtual void DeleteModel(TModel model)
            => _modelDeleter?.Invoke(model);

        private static PropertyInfo GetPropertyInfo(
            LambdaExpression propertyExpression)
        {
            if (propertyExpression == null) throw new ArgumentNullException(nameof(propertyExpression));

            var memberExpr = propertyExpression.Body as MemberExpression;
            if (memberExpr == null)
                throw new ArgumentException($"{propertyExpression} refers to a method, not a property.");

            var propInfo = memberExpr.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException($"{propertyExpression} refers to a field, not a property.");

            return propInfo;
        }

        protected virtual void LoadCollections()
        {
            foreach (var initializer in _collectionInitializers) { initializer(); }
        }

        protected virtual IEnumerable<TModel> LoadModels()
            => _modelsLoader?.Invoke();

        protected ConfiguredViewModelBase<TModel> ModelDeleter(Action<TModel> modelDeleter)
        {
            _modelDeleter = modelDeleter;
            return this;
        }

        protected ConfiguredViewModelBase<TModel> ModelSaver(Func<TModel, TModel> modelSaver)
        {
            _modelSaver = modelSaver;
            return this;
        }

        protected ConfiguredViewModelBase<TModel> ModelsLoader(Func<IEnumerable<TModel>> modelsLoader)
        {
            _modelsLoader = modelsLoader;
            return this;
        }

        protected virtual TModel SaveModel(TModel model)
        {
            if (_modelSaver == null) return default(TModel);

            SetModelBeforeSaving(model);
            var result = _modelSaver(model);
            SetModelAfterSaving(result);
            return result;
        }

        protected virtual void SetModelAfterSaving(TModel model)
        {
            foreach (var setter in _afterSaveItemModelSetters)
            {
                setter(model);
            }
        }

        protected virtual void SetModelBeforeSaving(TModel model)
        {
            foreach (var setter in _beforeSaveItemModelSetters)
            {
                setter(model);
            }
        }

        protected virtual void SetSelectedElements(TModel selectedItem)
        {
            foreach (var selector in _selectedElementSetters)
            {
                selector(selectedItem);
            }
        }
        #endregion
    }
}