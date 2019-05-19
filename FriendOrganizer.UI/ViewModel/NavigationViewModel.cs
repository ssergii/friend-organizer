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

        public ObservableCollection<NavigationItemViewModel> Friends { get; }
        #endregion

        public NavigationViewModel(
            IFriendLookupDataService dataService,
            IEventAggregator eventAggregator)
        {
            _dataService = dataService;

            _eventAggregator = eventAggregator;
            _eventAggregator
                .GetEvent<AfterDetailSavedEvent>()
                .Subscribe(AfterDetailSaved);
            _eventAggregator
                .GetEvent<AfterDetailDeletedEvent>()
                .Subscribe(AfterDetailDeleted);

            Friends = new ObservableCollection<NavigationItemViewModel>();
        }

        private void AfterDetailSaved(AfterDetailSavedEventArgs args)
        {
            switch (args.VMName)
            {
                case nameof(FriendDetailViewModel) :
                    var lookupItem = Friends.SingleOrDefault(x => x.Id == args.Id);
                    if (lookupItem == null)
                        Friends.Add(new NavigationItemViewModel(args.Id, args.DisplayMember, nameof(FriendDetailViewModel), _eventAggregator));
                    else
                        lookupItem.DisplayMember = args.DisplayMember;
                    break;
            }
        }

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            switch (args.VMName)
            {
                case nameof(FriendDetailViewModel) :
                    var friend = Friends.Single(x => x.Id == args.Id);
                    Friends.Remove(friend);
                    break;
            }
        }

        public async Task LoadAsync()
        {
            Friends.Clear();

            var lookup = await _dataService.GetFriendLookupAsync();
            lookup.ToList().ForEach(x =>
                Friends.Add( new NavigationItemViewModel(x.Id, x.DisplayMember, nameof(FriendDetailViewModel), _eventAggregator)));
        }
    }
}
