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
        private IFriendLookupDataService _friendDataService;
        private IMeetingLookupDataService _meetingDataService;
        private IEventAggregator _eventAggregator;

        public ObservableCollection<NavigationItemViewModel> Friends { get; }
        public ObservableCollection<NavigationItemViewModel> Meetings { get; }
        #endregion

        public NavigationViewModel(
            IFriendLookupDataService friendDataService,
            IMeetingLookupDataService meetingDataService,
            IEventAggregator eventAggregator)
        {
            _friendDataService = friendDataService;
            _meetingDataService = meetingDataService;

            _eventAggregator = eventAggregator;
            _eventAggregator
                .GetEvent<AfterDetailSavedEvent>()
                .Subscribe(AfterDetailSaved);
            _eventAggregator
                .GetEvent<AfterDetailDeletedEvent>()
                .Subscribe(AfterDetailDeleted);

            Friends = new ObservableCollection<NavigationItemViewModel>();
            Meetings = new ObservableCollection<NavigationItemViewModel>();
        }

        private void AfterDetailSaved(AfterDetailSavedEventArgs args)
        {
            switch (args.VMName)
            {
                case nameof(FriendDetailViewModel) :
                    AfterDetailSaved(Friends, args);
                    break;
                case nameof(MeetingDetailViewModel) :
                    AfterDetailSaved(Meetings, args);
                    break;
            }
        }

        private void AfterDetailSaved(
            ObservableCollection<NavigationItemViewModel> items,
            AfterDetailSavedEventArgs args)
        {
            var lookupItem = items.SingleOrDefault(x => x.Id == args.Id);
            if (lookupItem == null)
                items.Add(new NavigationItemViewModel(args.Id, args.DisplayMember, args.VMName, _eventAggregator));
            else
                lookupItem.DisplayMember = args.DisplayMember;
        }

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            switch (args.VMName)
            {
                case nameof(FriendDetailViewModel) :
                    AfterDetailDeleted(Friends, args);
                    break;
                case nameof(MeetingDetailViewModel):
                    AfterDetailDeleted(Meetings, args);
                    break;
            }
        }

        private void AfterDetailDeleted(
            ObservableCollection<NavigationItemViewModel> items,
            AfterDetailDeletedEventArgs args)
        {
            var item = items.Single(x => x.Id == args.Id);
            items.Remove(item);
        }

        public async Task LoadAsync()
        {
            Friends.Clear();
            var lookup = await _friendDataService.GetFriendLookupAsync();
            lookup.ToList().ForEach(x =>
                Friends.Add(new NavigationItemViewModel(x.Id, x.DisplayMember, nameof(FriendDetailViewModel), _eventAggregator)));

            Meetings.Clear();
            lookup = await _meetingDataService.GetMeetingLookupAsync();
            lookup.ToList().ForEach(x =>
                Meetings.Add(new NavigationItemViewModel(x.Id, x.DisplayMember, nameof(MeetingDetailViewModel), _eventAggregator)));
        }
    }
}
