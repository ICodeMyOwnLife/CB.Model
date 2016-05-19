using System.Linq;
using System.Windows.Input;
using CB.Model.Common;
using Microsoft.Practices.Prism.Commands;


namespace CB.Model.Prism
{
    public class PrismViewModelBase: NotifiableViewModelBase
    {
        #region Implementation
        protected void RaiseCommandsCanExecuteChanged(params ICommand[] commands)
        {
            foreach (var command in commands.OfType<DelegateCommandBase>())
            {
                command.RaiseCanExecuteChanged();
            }
        }
        #endregion
    }
}