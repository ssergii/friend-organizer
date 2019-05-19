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
using System;
using System.Collections.Generic;
using System.ComponentModel;

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

        private PhoneNumberWrapper _selectedPhoneNumber;
        public PhoneNumberWrapper SelectedPhoneNumber
        {
            get { return _selectedPhoneNumber; }
            set
            {
                _selectedPhoneNumber = value;
                OnPropertyChanged();
                (RemovePhoneCommand as DelegateCommand).RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<PhoneNumberWrapper> PhoneNumbers { get; }
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
            PhoneNumbers = new ObservableCollection<PhoneNumberWrapper>();

            InitCommends();
        }

        public async Task LoadByIdAsync(int? id)
        {
            var model = id.HasValue ?
                await _friendRepository.GetByIdAsync(id.Value) : CreateFriend();

            InitFriend(model);
            InitPhoneNumber(model.PhoneNumbers);

            await LoadProgLanguagesAsync();
        }

        #region commands
        public ICommand SaveCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand AddPhoneCommand { get; private set; }
        public ICommand RemovePhoneCommand { get; private set; }

        private void InitCommends()
        {
            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExcute);
            DeleteCommand = new DelegateCommand(OnDeleteCommandExecute);
            AddPhoneCommand = new DelegateCommand(OnAddPhoneExecute);
            RemovePhoneCommand = new DelegateCommand(OnRemovePhoneExecute, OnRemovePhoneCanExecute);
        }

        private bool OnSaveCanExcute()
        {
            return Friend != null && !Friend.HasErrors && HasChanges
                && PhoneNumbers.All(x => !x.HasErrors);
        }

        private async void OnSaveExecute()
        {
            await _friendRepository.SaveAsync();

            HasChanges = _friendRepository.HasChanges();

            _eventAggregator.GetEvent<AfterDetailSavedEvent>().Publish(
                new AfterDetailSavedEventArgs
                {
                    Id = Friend.Id,
                    DisplayMember = $"{Friend.FirstName} {Friend.LastName}",
                    VMName = nameof(FriendDetailViewModel)
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

            _eventAggregator.GetEvent<AfterDetailDeletedEvent>().Publish(
                new AfterDetailDeletedEventArgs
                {
                    Id = Friend.Id,
                    VMName = nameof(FriendDetailViewModel)
                });
        }

        private void OnAddPhoneExecute()
        {
            var phone = new PhoneNumberWrapper(new PhoneNumber());
            phone.PropertyChanged += PhoneNumbers_PropertyChange;
            PhoneNumbers.Add(phone);
            Friend.Model.PhoneNumbers.Add(phone.Model);
            phone.Number = ""; // trigger validation
        }

        private bool OnRemovePhoneCanExecute()
        {
            return SelectedPhoneNumber != null;
        }

        private void OnRemovePhoneExecute()
        {
            SelectedPhoneNumber.PropertyChanged += PhoneNumbers_PropertyChange;
            _friendRepository.RemovePhoneNumber(SelectedPhoneNumber.Model);
            PhoneNumbers.Remove(SelectedPhoneNumber);
            SelectedPhoneNumber = null;
            HasChanges = _friendRepository.HasChanges();
            (SaveCommand as DelegateCommand).RaiseCanExecuteChanged();
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

        private void InitPhoneNumber(IEnumerable<PhoneNumber> phoneNumbers)
        {
            foreach (var wrapper in PhoneNumbers)
                wrapper.PropertyChanged -= PhoneNumbers_PropertyChange;


            PhoneNumbers.Clear();
            foreach(var phone in phoneNumbers)
            {
                var wrapper = new PhoneNumberWrapper(phone);
                PhoneNumbers.Add(wrapper);
                wrapper.PropertyChanged += PhoneNumbers_PropertyChange;
            }
        }

        private void PhoneNumbers_PropertyChange(object sender, PropertyChangedEventArgs e)
        {
            if (!HasChanges)
                HasChanges = _friendRepository.HasChanges();

            if (e.PropertyName == nameof(PhoneNumberWrapper.HasErrors))
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
