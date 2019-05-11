using FriendOrganizer.UI.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace FriendOrganizer.UI.Wrapper
{
    public class NotifyDataErrorInfoBase : BaseViewModel, INotifyDataErrorInfo
    {
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

        protected void AddError(string propertyName, string error)
        {
            if (!_errorsByProperyName.ContainsKey(propertyName))
                _errorsByProperyName[propertyName] = new List<string>();

            if (!_errorsByProperyName[propertyName].Contains(error))
            {
                _errorsByProperyName[propertyName].Add(error);
                OnErrorsChanged(propertyName);
            }
        }

        protected void ClearError(string propertyName)
        {
            if (_errorsByProperyName.ContainsKey(propertyName))
            {
                _errorsByProperyName.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }
    }
}
