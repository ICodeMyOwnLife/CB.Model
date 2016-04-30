using System;


namespace CB.Model.Common
{
    public class ToStringAttribute: Attribute
    {
        #region  Properties & Indexers
        public int OrderIndex { get; set; } = int.MaxValue;
        #endregion
    }
}



// TODO: Reimplement ToString