namespace CB.Model.Common
{
    public interface IViewModelConfiguration<in TModel>
    {
        #region Abstract
        void LoadItems();

        void SetSelectedItems(TModel selectedItem);
        #endregion
    }

    public class ViewModelConfiguration<TModel>: IViewModelConfiguration<TModel>
    {
        public void LoadItems()
        {
            throw new System.NotImplementedException();
        }

        public void SetSelectedItems(TModel selectedItem)
        {
            throw new System.NotImplementedException();
        }
    }
}