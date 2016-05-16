using System;
using System.Linq;
using System.Reflection;


namespace CB.Model.Common
{
    [Serializable]
    public abstract class ObservableObject: PropertyNotifiableObject
    {
        #region Override
        public override string ToString()
        {
            return $@"{GetType().Name}: {{{string.Join(", ",
                GetType().GetProperties().Where(p => p.GetCustomAttribute<ToStringAttribute>() != null).OrderBy(
                    p => p.GetCustomAttribute<ToStringAttribute>().OrderIndex).Select(
                        p => $"{p.Name}: {p.GetValue(this)}"))}}}";
        }
        #endregion
    }
}