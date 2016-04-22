using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;


namespace CB.Model.Common
{
    [Serializable]
    public abstract class ObservableObject: INotifyPropertyChanged
    {
        #region Events
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion


        #region Implementation
        private void InvokePropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected virtual void NotifyAllPropertyChanged()
            => InvokePropertyChanged("");

        protected virtual void NotifyChanged([CallerMemberName] string propertyName = "")
            => NotifyPropertyChanged(propertyName);

        protected virtual void NotifyPropertyChanged(string propertyName)
        {
            Debug.Assert(propertyName == "" || GetType().GetProperty(propertyName) != null);
            InvokePropertyChanged(propertyName);
        }

        protected virtual bool SetField<T>(ref T field, T value, string propertyName, Func<T, T> transformField = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;

            field = transformField == null ? value : transformField(value);
            NotifyPropertyChanged(propertyName);
            return true;
        }

        protected virtual bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
            => SetField(ref field, value, propertyName);

        protected virtual bool SetProperty<T>(ref T field, T value, Func<T, T> transformField,
            [CallerMemberName] string propertyName = "")
            => SetField(ref field, value, propertyName, transformField);
        #endregion
    }
}