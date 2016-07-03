using System.Threading.Tasks;


namespace CB.Model.Common
{
    public interface IInitialize
    {
        #region Abstract
        void Initialize();
        Task InitializeAsync();
        #endregion
    }
}