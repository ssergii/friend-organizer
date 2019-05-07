using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;
using System.Collections.ObjectModel;
using System.Linq;

namespace FriendOrganizer.UI.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        #region fields
        private IDataService<Friend> _dataService;

        private Friend _selectedFriend;
        #endregion

        #region constructors
        public MainViewModel(IDataService<Friend> dataService)
        {
            _dataService = dataService;

            Friends = new ObservableCollection<Friend>();
        }
        #endregion

        #region properties
        public ObservableCollection<Friend> Friends { get; set; }

        public Friend SelectedFriend
        {
            get { return _selectedFriend; }
            set
            {
                _selectedFriend = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region public methods
        public void Load()
        {
            var friends = _dataService.GetAll();
            Friends.Clear();

            friends.ToList().ForEach(x =>
               Friends.Add(x));
        }
        #endregion
    }
}
