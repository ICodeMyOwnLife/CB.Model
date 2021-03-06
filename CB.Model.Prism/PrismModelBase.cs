﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CB.Model.Common;
using Prism.Mvvm;


namespace CB.Model.Prism
{
    public abstract class PrismModelBase: BindableObject, INotifyDataErrorInfo
    {
        #region Fields
        [NonSerialized]
        private ErrorsContainer<ValidationResult> _errorsContainer;
        #endregion


        #region  Properties & Indexers
        [XmlIgnore, SoapIgnore, ScriptIgnore]
        [Display(AutoGenerateField = false)]
        protected ErrorsContainer<ValidationResult> ErrorsContainer
            => _errorsContainer ?? (_errorsContainer = new ErrorsContainer<ValidationResult>(OnErrorsChanged));

        [XmlIgnore, SoapIgnore, ScriptIgnore]
        [Display(AutoGenerateField = false)]
        public virtual bool HasErrors => ErrorsContainer.HasErrors;
        #endregion


        #region Events
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        #endregion


        #region Methods
        public virtual IEnumerable GetErrors(string propertyName)
            => ErrorsContainer.GetErrors(propertyName);
        #endregion


        #region Override
        protected override bool SetField<T>(ref T field, T value, string propertyName, Func<T, T> transformField = null)
        {
            if (!base.SetField(ref field, value, propertyName, transformField)) return false;

            ValidateProperty(value, propertyName);
            NotifyPropertyChanged(nameof(HasErrors));
            return true;
        }
        #endregion


        #region Implementation
        protected virtual void ClearError([CallerMemberName] string propertyName = "")
            => ClearPropertyErrors(propertyName);

        protected virtual void ClearPropertyErrors(string propertyName) => ErrorsContainer.ClearErrors(propertyName);

        protected virtual void OnErrorsChanged(string propertyName)
            => OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName));

        protected virtual void OnErrorsChanged(DataErrorsChangedEventArgs e)
            => ErrorsChanged?.Invoke(this, e);

        protected virtual void SetError(string error, [CallerMemberName] string propertyName = "")
            => SetPropertyErrors(propertyName, !string.IsNullOrEmpty(error) ? new[] { error } : null);

        protected virtual void SetErrors(IEnumerable<string> errors, [CallerMemberName] string propertyName = "")
            => SetPropertyErrors(propertyName, errors);

        protected virtual void SetPropertyErrors(string propertyName, IEnumerable<string> errors)
            => ErrorsContainer.SetErrors(propertyName, errors?.Select(e => new ValidationResult(e)));

        protected virtual void SetPropertyErrors(string propertyName, params string[] errors)
            => SetPropertyErrors(propertyName, errors?.Any() == true ? (IEnumerable<string>)errors : null);

        protected virtual void ValidateProperties(params string[] propertyNames)
        {
            var type = GetType();
            ValidateProperties(propertyNames.Select(propertyName => type.GetProperty(propertyName)));
        }

        protected virtual void ValidateProperties(IEnumerable<LambdaExpression> propertyExpressions)
            => ValidateProperties(propertyExpressions.Select(expression => expression.GetPropertyInfo()));

        protected virtual void ValidateProperties(params Expression<Func<object>>[] propertyExpressions)
            => ValidateProperties(propertyExpressions.Cast<LambdaExpression>());

        private void ValidateProperties(IEnumerable<PropertyInfo> propertyInfos)
        {
            foreach (var propertyInfo in propertyInfos)
            {
                ValidateProperty(propertyInfo.GetValue(this), propertyInfo.Name);
            }
        }

        protected virtual void ValidateProperty(object value, string propertyName)
        {
            if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));

            ValidateProperty(value, new ValidationContext(this, null, null) { MemberName = propertyName });
        }

        protected virtual void ValidateProperty(object value, ValidationContext validationContext)
        {
            if (validationContext == null) throw new ArgumentNullException(nameof(validationContext));
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateProperty(value, validationContext, validationResults);
            ErrorsContainer.SetErrors(validationContext.MemberName, validationResults);
        }
        #endregion
    }
}