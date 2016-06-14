using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CB.Model.Common;
using Prism.Mvvm;


namespace CB.Model.Prism
{
    public abstract class PrismModelBase: BindableObject, INotifyDataErrorInfo
    {
        #region Fields
        private ErrorsContainer<ValidationResult> _errorsContainer;
        #endregion


        #region  Properties & Indexers
        protected ErrorsContainer<ValidationResult> ErrorsContainer
            => _errorsContainer ?? (_errorsContainer = new ErrorsContainer<ValidationResult>(OnErrorsChanged));

        public virtual bool HasErrors => _errorsContainer.HasErrors;
        #endregion


        #region Events
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        #endregion


        #region Methods
        public virtual IEnumerable GetErrors(string propertyName)
            => _errorsContainer.GetErrors(propertyName);
        #endregion


        #region Implementation
        protected virtual void OnErrorsChanged(string propertyName)
            => OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName));

        protected virtual void OnErrorsChanged(DataErrorsChangedEventArgs e)
            => ErrorsChanged?.Invoke(this, e);

        protected virtual void ValidateProperty(string propertyName, object value)
        {
            if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));

            ValidateProperty(new ValidationContext(this, null, null) { MemberName = propertyName }, value);
        }

        protected virtual void ValidateProperty(ValidationContext validationContext, object value)
        {
            if (validationContext == null) throw new ArgumentNullException(nameof(validationContext));
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateProperty(value, validationContext, validationResults);
            ErrorsContainer.SetErrors(validationContext.MemberName, validationResults);
        }
        #endregion
    }
}