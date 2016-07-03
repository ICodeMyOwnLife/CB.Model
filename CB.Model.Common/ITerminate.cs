using System.Threading.Tasks;


namespace CB.Model.Common
{
    public interface ITerminate
    {
        #region Abstract
        void Terminate();
        Task TerminateAsync();
        #endregion
    }
}