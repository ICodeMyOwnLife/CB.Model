using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;


namespace CB.Model.Prism
{
    public class PrismModelCollectionBase<TModel, TCollection>: PrismCollectionBase<TModel, TCollection>
        where TCollection: IList, IEnumerable<TModel>, INotifyCollectionChanged, new() where TModel: class, new()
    {
        #region  Constructors & Destructor
        public PrismModelCollectionBase(): this(default(TCollection)) { }

        public PrismModelCollectionBase(TCollection collection): base(collection)
        {
            AddNewCommand = new DelegateCommand(AddNew);
        }
        #endregion


        #region  Commands
        public virtual ICommand AddNewCommand { get; }
        #endregion


        #region Methods
        public virtual void AddNew()
            => Add(new TModel());
        #endregion
    }
}