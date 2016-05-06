using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;


namespace CB.Model.Common
{
    [Serializable]
    public abstract class BindableObject: INotifyPropertyChanged
    {
        #region Events
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion


        #region Methods
        public static void BindProperty(INotifyPropertyChanged sourceObject, string sourceProperty,
            INotifyPropertyChanged targetObject, string targetProperty, BindMode bindMode = BindMode.TwoWay)
        {
            switch (bindMode)
            {
                case BindMode.TwoWay:
                    Bind(sourceObject, sourceProperty, targetObject, targetProperty);
                    Bind(targetObject, targetProperty, sourceObject, sourceProperty);
                    break;
                case BindMode.OneWay:
                    Bind(sourceObject, sourceProperty, targetObject, targetProperty);
                    break;
                case BindMode.OneWayToSource:
                    Bind(targetObject, targetProperty, sourceObject, sourceProperty);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(bindMode), bindMode, null);
            }
        }
        #endregion


        #region Override
        public override string ToString()
        {
            return $@"{GetType().Name}: {{{string.Join(", ",
                GetType().GetProperties().Where(p => p.GetCustomAttribute<ToStringAttribute>() != null).OrderBy(
                    p => p.GetCustomAttribute<ToStringAttribute>().OrderIndex).Select(
                        p => $"{p.Name}: {p.GetValue(this)}"))}}}";
        }
        #endregion


        #region Implementation
        private static void Bind(INotifyPropertyChanged sourceObject, string sourceProperty,
            INotifyPropertyChanged targetObject, string targetProperty)
        {
            PropertyInfo sourceProp, targetProp;
            if (sourceObject == null || string.IsNullOrEmpty(sourceProperty) || targetObject == null ||
                string.IsNullOrEmpty(targetProperty) ||
                (sourceProp = sourceObject.GetType().GetProperty(sourceProperty)) == null ||
                (targetProp = targetObject.GetType().GetProperty(targetProperty)) == null) return;

            SetBoundProperty(sourceObject, sourceProp, targetObject, targetProp);

            sourceObject.PropertyChanged += (sender, args) =>
            {
                if (Equals(sourceProperty, args.PropertyName))
                {
                    SetBoundProperty(sourceObject, sourceProp, targetObject, targetProp);
                }
            };
        }

        private void InvokePropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected virtual void NotifyAllPropertyChanged() => InvokePropertyChanged("");

        protected virtual void NotifyChanged([CallerMemberName] string propertyName = "")
            => NotifyPropertyChanged(propertyName);

        protected virtual void NotifyPropertyChanged<TProperty>(Expression<Func<TProperty>> propertyExpression)
            => NotifyPropertyChanged(propertyExpression.GetPropertyName());

        protected virtual void NotifyPropertyChanged(string propertyName)
        {
            Debug.Assert(propertyName == "" || GetType().GetProperty(propertyName) != null);
            InvokePropertyChanged(propertyName);
        }

        private static void SetBoundProperty(INotifyPropertyChanged sourceObject, PropertyInfo sourceProp,
            INotifyPropertyChanged targetObject, PropertyInfo targetProp)
        {
            targetProp.SetValue(targetObject, sourceProp.GetValue(sourceObject));
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
            [CallerMemberName] string propertyName = "") => SetField(ref field, value, propertyName, transformField);
        #endregion
    }
}