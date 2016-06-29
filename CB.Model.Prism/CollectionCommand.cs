using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;


namespace CB.Model.Prism
{
    public class CollectionCommand<TCollection, TViewModel>: ICommand
        where TCollection: IEnumerable<TViewModel>, INotifyCollectionChanged
    {
        #region Fields
        private readonly TCollection _collection;
        private readonly Func<TViewModel, ICommand> _commandGetter;
        #endregion


        #region  Constructors & Destructor
        public CollectionCommand(TCollection collection, Func<TViewModel, ICommand> commandGetter)
        {
            _collection = collection;
            _commandGetter = commandGetter;
            _collection.CollectionChanged += Collection_CollectionChanged;
        }

        public CollectionCommand(TCollection collection, Func<TViewModel, ICommand> commandGetter,
            ActiveStategy activeStategy): this(collection, commandGetter)
        {
            ActiveStategy = activeStategy;
        }
        #endregion


        #region  Properties & Indexers
        public ActiveStategy ActiveStategy { get; set; }
        #endregion


        #region Events
        public event EventHandler CanExecuteChanged;
        #endregion


        #region Methods
        public bool CanExecute(object parameter)
        {
            if (!_collection.Any()) return false;
            switch (ActiveStategy)
            {
                case ActiveStategy.WhenAny:
                    return GetCommands().Any(c => c.CanExecute(parameter));
                case ActiveStategy.WhenAll:
                    return GetCommands().All(c => c.CanExecute(parameter));
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Execute(object parameter)
        {
            foreach (var command in GetCommands())
            {
                command.Execute(parameter);
            }
        }
        #endregion


        #region Event Handlers
        private void Collection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null) RegisterCommands(GetCommands(e.NewItems.Cast<TViewModel>()));
            if (e.OldItems != null) UnregisterCommands(GetCommands(e.OldItems.Cast<TViewModel>()));
            OnCanExecuteChanged();
        }
        #endregion


        #region Implementation
        private IEnumerable<ICommand> GetCommands()
            => GetCommands(_collection);

        private IEnumerable<ICommand> GetCommands(IEnumerable<TViewModel> viewModels)
            => viewModels.Select(_commandGetter);

        protected virtual void OnCanExecuteChanged()
            => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        private void RegisterCommands(IEnumerable<ICommand> commands)
        {
            foreach (var command in commands)
            {
                command.CanExecuteChanged += CanExecuteChanged;
            }
        }

        private void UnregisterCommands(IEnumerable<ICommand> commands)
        {
            foreach (var command in commands)
            {
                command.CanExecuteChanged -= CanExecuteChanged;
            }
        }
        #endregion
    }
}