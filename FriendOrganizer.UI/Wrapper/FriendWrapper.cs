using FriendOrganizer.Model;
using System;

namespace FriendOrganizer.UI.Wrapper
{
    public class FriendWrapper : NotifyDataErrorInfoBase
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
