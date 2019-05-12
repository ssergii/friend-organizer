using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace FriendOrganizer.UI.Wrapper
{
    public class ModelWrapper<T> : NotifyDataErrorInfoBase
    {
        public ModelWrapper(T model)
        {
            Model = model;
        }

        public T Model { get; }

        protected TValue GetValue<TValue>([CallerMemberName]string propertyName = null)
        {
            return (TValue)typeof(T).GetProperty(propertyName).GetValue(Model);
        }

        protected void SetValue<TValue>(TValue val, [CallerMemberName]string propertyName = null)
        {
            typeof(T).GetProperty(propertyName).SetValue(Model, val);
            OnPropertyChanged();
            ValidatePropertyInternal(propertyName);
        }

        private void ValidatePropertyInternal(string propertyName)
        {
            ClearError(propertyName);

            var errors = ValidateProperty(propertyName);
            if (errors == null)
                return;

            errors.ToList().ForEach(x => AddError(propertyName, x));
        }

        protected virtual IEnumerable<string> ValidateProperty(string propertyName)
        {
            return null;
        }
    }
}
