using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.Event;
using FriendOrganizer.UI.Wrapper;
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

        private FriendWrapper _friend;
        private IEventAggregator _eventAggregator;

        public FriendWrapper Friend
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
            var model = await _dataService.GetByIdAsync(id);
            Friend = new FriendWrapper(model);
            Friend.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(Friend.HasErrors))
                    (SaveCommand as DelegateCommand).RaiseCanExecuteChanged();
            };

            (SaveCommand as DelegateCommand).RaiseCanExecuteChanged();
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
            return Friend != null && !Friend.HasErrors;
        }

        private async void OnSaveExecute()
        {
            await _dataService.SaveAsync(Friend.Model);

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
