using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.Event;
using Prism.Commands;
using Prism.Events;
using System.Threading.Tasks;
using System.Windows.Input;

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

        #region commands
        private ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                    _saveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExcute);

                return _saveCommand;
            }
        }

        private bool OnSaveCanExcute()
        {
            // TODO: friend validation
            return true;
        }

        private async void OnSaveExecute()
        {
            await _dataService.SaveAsync(Friend);

            _eventAggregator.GetEvent<AfterFriendSavedEvent>().Publish(
                new AfterFriendSavedEventArgs
                {
                    Id = Friend.Id,
                    DisplayMember = $"{Friend.FirstName} {Friend.LastName}"
                });
        }
        #endregion
    }
}
