using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using Prism.Commands;


namespace CB.Model.Prism
{
    public class PrismCollectionBase<TModel, TCollection>: CollectionBase<TModel, TCollection>, INotifyCollectionChanged
        where TModel: class where TCollection: IList, IEnumerable<TModel>, INotifyCollectionChanged, new()
    {
        #region  Constructors & Destructor
        public PrismCollectionBase(): this(new TCollection()) { }

        public PrismCollectionBase(TCollection collection): base(collection)
        {
            collection.CollectionChanged += Collection_CollectionChanged;
            AddCommand = new DelegateCommand<TModel>(Add);
            ClearCommand = new DelegateCommand(Clear, () => CanClear).ObservesProperty(() => CanClear);
            RemoveCommand = new DelegateCommand<TModel>(Remove, m => Collection.Contains(m));
            RemoveItemCommand =
                new DelegateCommand(RemoveItem, () => CanRemoveItem).ObservesProperty(() => CanRemoveItem);
            SelectCommand = new DelegateCommand<TModel>(Select, m => Collection.Contains(m));
        }
        #endregion


        #region  Commands
        public virtual ICommand AddCommand { get; }
        public virtual ICommand ClearCommand { get; }
        public virtual ICommand RemoveCommand { get; }
        public virtual ICommand RemoveItemCommand { get; }
        public virtual ICommand SelectCommand { get; }
        #endregion


        #region  Properties & Indexers
        public virtual bool CanClear => Collection.Any();
        public virtual bool CanRemoveItem => Collection.Contains(SelectedItem);
        #endregion


        #region Events
        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add { Collection.CollectionChanged += value; }
            remove { Collection.CollectionChanged -= value; }
        }
        #endregion


        #region Override
        protected override void View_CurrentChanged(object sender, EventArgs e)
        {
            base.View_CurrentChanged(sender, e);
            NotifyPropertiesChanged(nameof(CanRemoveItem));
        }
        #endregion


        #region Event Handlers
        private void Collection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyAbilitiesChanged();
        }
        #endregion


        #region Implementation
        private void NotifyAbilitiesChanged()
        {
            NotifyPropertiesChanged(nameof(CanClear));
        }
        #endregion
    }
}