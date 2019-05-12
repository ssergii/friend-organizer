using FriendOrganizer.Model;
using System;

namespace FriendOrganizer.UI.Wrapper
{
    public class FriendWrapper : ModelWrapper<Friend>
    {
        public FriendWrapper(Friend model) : base(model) { }

        #region model wrapping
        public int Id
        {
            get { return GetValue<int>(); }
        }

        public string FirstName
        {
            get { return GetValue<string>(); }
            set
            {
                SetValue(value);
                ValidateProperty(nameof(FirstName));
            }
        }

        public string LastName
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string Email
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
        #endregion

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
    }
}
