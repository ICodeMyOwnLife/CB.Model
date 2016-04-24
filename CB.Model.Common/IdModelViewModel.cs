using System;


namespace CB.Model.Common
{
    public class IdModelViewModel: IdModelViewModelBase<IdModelBase>
    {
        #region Override
        protected override bool CanSaveItem(IdModelBase item)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteItem(int id)
        {
            throw new NotImplementedException();
        }

        protected override IdModelBase[] LoadItems()
        {
            throw new NotImplementedException();
        }

        protected override IdModelBase SaveItem(IdModelBase item)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}