using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Wrapper
{
    public class PhoneNumberWrapper : ModelWrapper<PhoneNumber>
    {
        public PhoneNumberWrapper(PhoneNumber phone) : base(phone) { }

        public string Number {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
    }
}
