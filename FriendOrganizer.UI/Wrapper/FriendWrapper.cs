using FriendOrganizer.Model;
using FriendOrganizer.UI.ViewModel;
using System.ComponentModel;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace FriendOrganizer.UI.Wrapper
{
    public class FriendWrapper : BaseViewModel, INotifyDataErrorInfo
    {
        public FriendWrapper(Friend model)
        {
            Model = model;
        }

        #region model wrapping
        public Friend Model { get; }

        public int Id
        {
            get { return Model.Id; }
        }

        public string FirstName
        {
            get { return Model.FirstName; }
            set
            {
                Model.FirstName = value;
                OnPropertyChanged();
                ValidateProperty(nameof(FirstName));
            }
        }

        private void ValidateProperty(string propertyName)
        {
            ClearError(propertyName);

            switch (propertyName)
            {
                case nameof(FirstName):
                    if (string.IsNullOrEmpty(FirstName))
                        AddError(propertyName, "FirstName is requared");
                    if (string.Equals(FirstName, "Robot", StringComparison.OrdinalIgnoreCase))
                        AddError(propertyName, "Robots are not valid friends");
                    break;
            }
        }

        public string LastName
        {
            get { return Model.LastName; }
            set
            {
                Model.LastName = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get { return Model.Email; }
            set
            {
                Model.Email = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region INotifyDataErrorInfo members implementations
        private Dictionary<string, List<string>> _errorsByProperyName
          = new Dictionary<string, List<string>>();

        public bool HasErrors
        {
            get { return _errorsByProperyName.Any(); }
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            return _errorsByProperyName.ContainsKey(propertyName) ?
                _errorsByProperyName[propertyName] : null;
        }

        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        private void AddError(string propertyName, string error)
        {
            if (!_errorsByProperyName.ContainsKey(propertyName))
                _errorsByProperyName[propertyName] = new List<string>();

            if (!_errorsByProperyName[propertyName].Contains(error))
            {
                _errorsByProperyName[propertyName].Add(error);
                OnErrorsChanged(propertyName);
            }
        }

        private void ClearError(string propertyName)
        {
            if (_errorsByProperyName.ContainsKey(propertyName))
            {
                _errorsByProperyName.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }
        #endregion
    }
}
