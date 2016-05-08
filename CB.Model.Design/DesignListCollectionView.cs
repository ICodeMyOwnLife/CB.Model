using System.Collections;
using System.Windows.Data;


namespace CB.Model.Design
{
    public class DesignListCollectionView: ListCollectionView
    {
        #region  Constructors & Destructor
        public DesignListCollectionView(): base(new ArrayList()) { }
        #endregion


        #region  Properties & Indexers
        public IList List
        {
            get { return InternalList; }
            set
            {
                while (Count > 0)
                {
                    RemoveAt(0);
                }
                if (value == null) return;

                AddNew();
                foreach (var item in value)
                {
                    AddNewItem(item);
                }
                CommitNew();
            }
        }
        #endregion
    }
}