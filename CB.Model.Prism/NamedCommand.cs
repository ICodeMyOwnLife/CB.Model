using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;


namespace CB.Model.Prism
{
    public class NamedCommand: INameCommand
    {
        #region  Constructors & Destructor
        public NamedCommand(string name, ICommand command)
        {
            Command = command;
            Name = name;
        }

        public NamedCommand(string name, Action executeMethod, Func<bool> canExecuteMethod)
            : this(name, new DelegateCommand(executeMethod, canExecuteMethod)) { }

        public NamedCommand(string name, Action executeMethod)
            : this(name, new DelegateCommand(executeMethod, null)) { }
        #endregion


        #region  Properties & Indexers
        public ICommand Command { get; }
        public string Name { get; }
        #endregion


        #region Methods
        public static NamedCommand FromAsyncHandler(string name, Func<Task> executeMethod, Func<bool> canExecuteMethod)
            => new NamedCommand(name, DelegateCommand.FromAsyncHandler(executeMethod, canExecuteMethod));

        public static NamedCommand FromAsyncHandler(string name, Func<Task> executeMethod)
            => new NamedCommand(name, DelegateCommand.FromAsyncHandler(executeMethod, null));
        #endregion


        #region Override
        public override string ToString()
            => Name;
        #endregion
    }
}