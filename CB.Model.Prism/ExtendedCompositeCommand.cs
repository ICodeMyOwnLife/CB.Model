using System;
using System.Linq;
using System.Windows.Input;
using Prism.Commands;


namespace CB.Model.Prism
{
    public class ExtendedCompositeCommand: CompositeCommand
    {
        #region  Properties & Indexers
        public ActiveStategy ActiveStategy { get; set; }
        #endregion


        #region Override
        public override bool CanExecute(object parameter)
        {
            ICommand[] commandList;
            lock (RegisteredCommands)
            {
                commandList = RegisteredCommands.Where(ShouldExecute).ToArray();
            }
            switch (ActiveStategy)
            {
                case ActiveStategy.WhenAny:
                    return commandList.All(c => c.CanExecute(parameter));
                case ActiveStategy.WhenAll:
                    return commandList.Any(c => c.CanExecute(parameter));
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        #endregion
    }
}