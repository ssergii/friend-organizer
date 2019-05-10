using System;
using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.Event;
using Prism.Events;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.ViewModel
{
    public class FriendDetailViewModel : BaseViewModel, IFriendDetailViewModel
    {
        #region fields and properties
        private IDataService<Friend> _dataService;

        private Friend _friend;
        private IEventAggregator _eventAggregator;

        public Friend Friend
        {
            get { return _friend; }
            set
            {
                _friend = value;
                OnPropertyChanged();
            }
        }
        #endregion


        public FriendDetailViewModel(
            IDataService<Friend> dataService,
            IEventAggregator eventAggregator)
        {
            _dataService = dataService;
            _eventAggregator = eventAggregator;
            _eventAggregator
                .GetEvent<OpenFriendDetailViewEvent>()
                .Subscribe(OnOpenFriendDetailViewEvent);
        }

        private async void OnOpenFriendDetailViewEvent(int id)
        {
            await LoadByIdAsync(id);
        }

        public async Task LoadByIdAsync(int id)
        {
             Friend = await _dataService.GetByIdAsync(id);
        }
    }
}
