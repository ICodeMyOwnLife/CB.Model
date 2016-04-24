using System.ComponentModel;


namespace CB.Model.Common
{
    public static class ObservableObjectHelper
    {
        #region Methods
        public static void BindProperty(this INotifyPropertyChanged sourceObject, string sourceProperty,
            INotifyPropertyChanged targetObject, string targetProperty, BindMode bindMode)
        {
            ObservableObject.BindProperty(sourceObject, sourceProperty, targetObject, targetProperty, bindMode);
        }
        #endregion
    }
}