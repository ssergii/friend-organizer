using FriendOrganizer.UI.Data.Lookups;
using FriendOrganizer.UI.Event;
using Prism.Events;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System;

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
                .GetEvent<AfterFriendSavedEvent>()
                .Subscribe(AfterFriendSaved);
            _eventAggregator
                .GetEvent<AfterFriendDeleteEvent>()
                .Subscribe(AfterFrienddeleted);

            Friends = new ObservableCollection<NavigationItemViewModel>();
        }

        private void AfterFriendSaved(AfterFriendSavedEventArgs obj)
        {
            var lookupItem = Friends.SingleOrDefault(x => x.Id == obj.Id);
            if (lookupItem == null)
                Friends.Add(new NavigationItemViewModel(obj.Id, obj.DisplayMember, _eventAggregator));
            else
                lookupItem.DisplayMember = obj.DisplayMember;
        }

        private void AfterFrienddeleted(int id)
        {
            var friend = Friends.Single(x => x.Id == id);
            Friends.Remove(friend);
        }

        public async Task LoadAsync()
        {
            Friends.Clear();

            var lookup = await _dataService.GetFriendLookupAsync();
            lookup.ToList().ForEach(x =>
                Friends.Add( new NavigationItemViewModel(x.Id, x.DisplayMember, _eventAggregator)));
        }
    }
}
