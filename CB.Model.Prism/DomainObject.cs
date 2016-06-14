using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Prism.Mvvm;


namespace CB.Model.Prism
{
    public abstract class DomainObject: INotifyPropertyChanged, INotifyDataErrorInfo
    {
        #region Fields
        private ErrorsContainer<ValidationResult> _errorsContainer;
        #endregion


        #region  Properties & Indexers
        protected ErrorsContainer<ValidationResult> ErrorsContainer => _errorsContainer ?? (_errorsContainer = new ErrorsContainer<ValidationResult>(RaiseErrorsChanged));

        public bool HasErrors
            => ErrorsContainer.HasErrors;
        #endregion


        #region Events
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion


        #region Methods
        public IEnumerable GetErrors(string propertyName)
            => _errorsContainer.GetErrors(propertyName);
        #endregion


        #region Implementation
        protected virtual void OnErrorsChanged(DataErrorsChangedEventArgs e)
            => ErrorsChanged?.Invoke(this, e);

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected void RaiseErrorsChanged(string propertyName)
        {
            OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName));
        }

        protected void RaisePropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        protected void ValidateProperty(string propertyName, object value)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException("propertyName");
            }

            ValidateProperty(new ValidationContext(this, null, null) { MemberName = propertyName }, value);
        }

        protected virtual void ValidateProperty(ValidationContext validationContext, object value)
        {
            if (validationContext == null)
            {
                throw new ArgumentNullException("validationContext");
            }

            var validationResults = new List<ValidationResult>();
            Validator.TryValidateProperty(value, validationContext, validationResults);

            ErrorsContainer.SetErrors(validationContext.MemberName, validationResults);
        }
        #endregion
    }
}


// TODO: Delete DomainObject