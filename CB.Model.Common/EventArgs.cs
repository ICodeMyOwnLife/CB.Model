using System;


namespace CB.Model.Common
{
    public class EventArgs<T>: EventArgs
    {
        #region  Constructors & Destructor
        public EventArgs(T value)
        {
            Value = value;
        }
        #endregion


        #region  Properties & Indexers
        public T Value { get; set; }
        #endregion
    }
}