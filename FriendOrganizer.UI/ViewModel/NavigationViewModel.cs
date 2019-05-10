using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.Event;
using Prism.Events;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.ViewModel
{
    public class NavigationViewModel : BaseViewModel, INavigationViewModel
    {
        #region fields and properties
        private IFriendLookupDataService _dataService;
        private IEventAggregator _eventAggregator;

        private LookupItem _selecetedFriend;
        public LookupItem SelecetedFriend
        {
            get { return _selecetedFriend; }
            set
            {
                _selecetedFriend = value;
                OnPropertyChanged();

                if (_selecetedFriend != null)
                    _eventAggregator
                        .GetEvent<OpenFriendDetailViewEvent>()
                        .Publish(_selecetedFriend.Id);
            }
        }

        public ObservableCollection<LookupItem> Friends { get; }
        #endregion

        public NavigationViewModel(
            IFriendLookupDataService dataService,
            IEventAggregator eventAggregator)
        {
            _dataService = dataService;
            _eventAggregator = eventAggregator;

            Friends = new ObservableCollection<LookupItem>();
        }


        public async Task LoadAsync()
        {
            Friends.Clear();

            var lookup = await _dataService.GetFriendLookupAsync();
            lookup.ToList().ForEach(x =>
                Friends.Add(x));
        }
    }
}
