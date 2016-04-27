namespace CB.Model.Common
{
    public interface IViewModelConfiguration<in TModel>
    {
        #region Abstract
        void LoadItems();

        void SetSelectedItems(TModel selectedItem);
        #endregion
    }
}