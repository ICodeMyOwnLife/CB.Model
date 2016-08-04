using System;


namespace CB.Model.Annotations
{
    public class ViewAttribute: Attribute
    {
        #region  Properties & Indexers
        public TextAlignment Alignment { get; set; } = TextAlignment.Left;
        #endregion
    }
}