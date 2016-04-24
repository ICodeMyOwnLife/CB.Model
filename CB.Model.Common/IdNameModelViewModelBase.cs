namespace CB.Model.Common
{
    public abstract class IdNameModelViewModelBase<TModel>: IdModelViewModelBase<TModel>
        where TModel: IdNameModelBase, new()
    {
        #region Override
        protected override bool CanSaveItem(TModel item)
        {
            return !string.IsNullOrEmpty(item?.Name);
        }
        #endregion
    }
}