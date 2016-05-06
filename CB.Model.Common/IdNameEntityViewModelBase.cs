namespace CB.Model.Common
{
    public abstract class IdNameEntityViewModelBase<TEntity>: IdEntityViewModelBase<TEntity>
        where TEntity: IdNameEntityBase, new()
    {
        #region Override
        protected override bool CanSaveItem(TEntity item)
        {
            return !string.IsNullOrEmpty(item?.Name);
        }
        #endregion
    }
}