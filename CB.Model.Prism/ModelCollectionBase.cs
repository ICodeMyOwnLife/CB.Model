using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;


namespace CB.Model.Prism
{
    public class ModelCollectionBase<TModel, TCollection>: CollectionBase<TModel, TCollection>
        where TCollection: IList, IEnumerable<TModel> where TModel: class, new()
    {
        #region  Constructors & Destructor
        public ModelCollectionBase(): this(default(TCollection)) { }

        public ModelCollectionBase(TCollection collection): base(collection)
        {
            AddCommand = new DelegateCommand<TModel>(Add);
            AddNewCommand = new DelegateCommand(AddNew);
            RemoveCommand = new DelegateCommand<TModel>(Remove, m => m != null);
            RemoveItemCommand = new DelegateCommand(RemoveItem, () => SelectedItem != null);
            SelectCommand = new DelegateCommand<TModel>(Select);
        }
        #endregion


        #region  Commands
        public virtual ICommand AddCommand { get; }
        public virtual ICommand AddNewCommand { get; }
        public virtual ICommand RemoveCommand { get; }
        public virtual ICommand RemoveItemCommand { get; }
        public virtual ICommand SelectCommand { get; }
        #endregion


        #region Methods
        public virtual void AddNew()
            => Add(new TModel());
        #endregion


        #region Override
        protected override void InvokePropertyChanged(string propertyName)
        {
            base.InvokePropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(SelectedItem):
                    RaiseCommandsCanExecuteChanged(RemoveItemCommand);
                    break;
            }
        }
        #endregion
    }
}