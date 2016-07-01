using System.Windows.Input;


namespace CB.Model.Prism
{
    public interface INameCommand
    {
        #region Abstract
        ICommand Command { get; }
        string Name { get; }
        #endregion
    }
}