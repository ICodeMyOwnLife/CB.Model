﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        protected virtual void LogError(Exception exception)
            => Debug.WriteLine(exception.ToString());

        protected virtual void NotifyError(string error)
            => Debug.WriteLine(error);

        protected virtual void NotifyPropertyChangedSync<TProperty>(Expression<Func<TProperty>> propertyExpression)
            => NotifyPropertyChangedSync(propertyExpression.GetPropertyName());

        protected virtual void NotifyPropertyChangedSync(string propertyName)
            => TryInvokeOnUiThread(() => NotifyPropertyChanged(propertyName));

        protected virtual bool SetFieldSync<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;

            field = value;
            NotifyPropertyChangedSync(propertyName);
            return true;
        }

        protected virtual bool SetPropertySync<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
            => SetFieldSync(ref field, value, propertyName);

        protected virtual void Try(Action action)
        {
            try { action(); }
            catch (Exception exception)
            {
                NotifyError(exception.Message);
                LogError(exception);
            }
        }

        protected virtual void TryInvokeOnUiThread(Action action)
            => _synchronizationContext?.Send(_ => action(), null);
        #endregion
    }
}