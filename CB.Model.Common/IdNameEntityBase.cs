namespace CB.Model.Common
{
    public abstract class IdNameEntityBase: IdEntityBase, IIdNameEntity
    {
        #region Fields
        private string _name;
        #endregion


        #region  Properties & Indexers
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }
        #endregion


        #region Override
        public override void CopyFrom(IdEntityBase other, bool copyId = false)
        {
            var idNameEntityBase = other as IdNameEntityBase;
            if (idNameEntityBase == null) return;

            Name = idNameEntityBase.Name;
            base.CopyFrom(other, copyId);
        }
        #endregion
    }
}