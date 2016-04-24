namespace CB.Model.Common
{
    public class IdNameModelBase: IdModelBase
    {
        #region Fields
        protected string _name;
        #endregion


        #region  Properties & Indexers
        public virtual string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }
        #endregion
    }
}