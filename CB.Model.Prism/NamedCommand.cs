using System.Windows.Input;


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
        #endregion


        #region  Properties & Indexers
        public ICommand Command { get; }
        public string Name { get; }
        #endregion
    }
}