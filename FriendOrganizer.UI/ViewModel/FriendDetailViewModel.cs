using FriendOrganizer.Model;
using FriendOrganizer.UI.Data.Repositories;
using FriendOrganizer.UI.Event;
using FriendOrganizer.UI.Wrapper;
using Prism.Commands;
using Prism.Events;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Linq;
using FriendOrganizer.UI.View.Services;
using FriendOrganizer.UI.Data.Lookups;
using System.Collections.ObjectModel;

namespace FriendOrganizer.UI.ViewModel
{
    public class FriendDetailViewModel : BaseViewModel, IFriendDetailViewModel
    {
        #region fields and properties
        private IRepository<Friend> _friendRepository;
        private IEventAggregator _eventAggregator;
        private IMessageDialogService _messageDialogService;
        private IProgLanguageLookupDataService _progLanDataService;

        private FriendWrapper _friend;
        public FriendWrapper Friend
        {
            get { return _friend; }
            set
            {
                _friend = value;
                OnPropertyChanged();
            }
        }

        private bool _hasChanges;
        public bool HasChanges
        {
            get { return _hasChanges; }
            set
            {
                if (_hasChanges == value)
                    return;

                _hasChanges = value;
                OnPropertyChanged();
                (SaveCommand as DelegateCommand).RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<LookupItem> ProgLanguages { get; }
        #endregion

        public FriendDetailViewModel(
            IRepository<Friend> repository,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService,
            IProgLanguageLookupDataService progLanDataService)
        {
            _friendRepository = repository;
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;
            _progLanDataService = progLanDataService;

            ProgLanguages = new ObservableCollection<LookupItem>();

            InitCommends();
        }

        public async Task LoadByIdAsync(int? id)
        {
            var model = id.HasValue ?
                await _friendRepository.GetByIdAsync(id.Value) : CreateFriend();

            InitFriend(model);
            await LoadProgLanguagesAsync();
        }

        #region commands
        public ICommand SaveCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }

        private void InitCommends()
        {
            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExcute);
            DeleteCommand = new DelegateCommand(OnDeleteCommandExecute);
        }

        private bool OnSaveCanExcute()
        {
            return Friend != null && !Friend.HasErrors && HasChanges;
        }

        private async void OnSaveExecute()
        {
            await _friendRepository.SaveAsync();

            HasChanges = _friendRepository.HasChanges();

            _eventAggregator.GetEvent<AfterFriendSavedEvent>().Publish(
                new AfterFriendSavedEventArgs
                {
                    Id = Friend.Id,
                    DisplayMember = $"{Friend.FirstName} {Friend.LastName}"
                });
        }

        private async void OnDeleteCommandExecute()
        {
            var result = _messageDialogService.ShowOkCancelDialog(
                $"Do you really want to delete {Friend.FirstName} {Friend.LastName}",
                "Question");

            if (result == MessageDialogResult.Cancel)
                return;

            _friendRepository.Remove(Friend.Model);
            await _friendRepository.SaveAsync();

            _eventAggregator.GetEvent<AfterFriendDeleteEvent>().Publish(Friend.Id);
        }
        #endregion

        #region methods
        private Friend CreateFriend()
        {
            var friend = new Friend();
            _friendRepository.Add(friend);

            return friend;
        }

        private void InitFriend(Friend model)
        {
            Friend = new FriendWrapper(model);
            Friend.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                    HasChanges = _friendRepository.HasChanges();

                if (e.PropertyName == nameof(Friend.HasErrors))
                    (SaveCommand as DelegateCommand).RaiseCanExecuteChanged();
            };

            (SaveCommand as DelegateCommand).RaiseCanExecuteChanged();
        }

        private async Task LoadProgLanguagesAsync()
        {
            ProgLanguages.Clear();
            ProgLanguages.Add(new NullLookupItem { DisplayMember = "-" });
            var languages = await _progLanDataService.GetProgLanguagesLookupAsync();
            languages.ToList().ForEach(x => ProgLanguages.Add(x));
        }
        #endregion
    }
}
