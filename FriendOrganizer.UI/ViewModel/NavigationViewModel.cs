using FriendOrganizer.UI.Data.Lookups;
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

        private NavigationItemViewModel _selecetedFriend;
        public NavigationItemViewModel SelecetedFriend
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

        public ObservableCollection<NavigationItemViewModel> Friends { get; }
        #endregion

        public NavigationViewModel(
            IFriendLookupDataService dataService,
            IEventAggregator eventAggregator)
        {
            _dataService = dataService;

            _eventAggregator = eventAggregator;
            _eventAggregator
                .GetEvent<AfterFriendSavedEvent>()
                .Subscribe(AfterFriendSaved);

            Friends = new ObservableCollection<NavigationItemViewModel>();
        }

        private void AfterFriendSaved(AfterFriendSavedEventArgs obj)
        {
            var lookupItem = Friends.Single(x => x.Id == obj.Id);
            lookupItem.DisplayMember = obj.DisplayMember;
        }

        public async Task LoadAsync()
        {
            Friends.Clear();

            var lookup = await _dataService.GetFriendLookupAsync();
            lookup.ToList().ForEach(x =>
                Friends.Add( new NavigationItemViewModel(x.Id, x.DisplayMember)));
        }
    }
}
