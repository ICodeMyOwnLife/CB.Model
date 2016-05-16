using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading;


namespace CB.Model.Common
{
    public abstract class NotifiableViewModelBase: PropertyNotifiableObject
    {
        #region Fields
        protected readonly SynchronizationContext _synchronizationContext = SynchronizationContext.Current;
        #endregion


        #region Implementation
        protected virtual void NotifyPropertyChangedSync<TProperty>(Expression<Func<TProperty>> propertyExpression)
            => NotifyPropertyChangedSync(propertyExpression.GetPropertyName());

        protected virtual void NotifyPropertyChangedSync(string propertyName)
        {
            if (_synchronizationContext == null) NotifyPropertyChanged(propertyName);
            else
            {
                _synchronizationContext.Send(_ => NotifyPropertyChanged(propertyName), null);
            }
        }

        protected virtual bool SetFieldSync<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;

            field = value;
            NotifyPropertyChangedSync(propertyName);
            return true;
        }

        protected virtual bool SetPropertySync<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            return SetFieldSync(ref field, value, propertyName);
        }
        #endregion
    }
}