namespace FriendOrganizer.UI.ViewModel
{
    public class NavigationItemViewModel : BaseViewModel
    {
        #region fields and properties
        public int Id { get; }

        private string _displayMember;
        public string DisplayMember
        {
            get { return _displayMember; }
            set
            {
                _displayMember = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public NavigationItemViewModel(int id, string displayMember)
        {
            Id = id;
            DisplayMember = displayMember;
        }
    }
}
