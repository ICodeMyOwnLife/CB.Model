using System.Collections.Generic;


namespace CB.Model.Common
{
    public class IdNameModelViewModel:IdNameModelViewModelBase<IdNameModelBase>
    {
        protected override void DeleteItem(int id)
        {
            throw new System.NotImplementedException();
        }

        protected override IEnumerable<IdNameModelBase> LoadItems()
        {
            throw new System.NotImplementedException();
        }

        protected override IdNameModelBase SaveItem(IdNameModelBase item)
        {
            throw new System.NotImplementedException();
        }
    }
}