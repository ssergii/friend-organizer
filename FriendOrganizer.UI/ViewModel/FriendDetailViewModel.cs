using FriendOrganizer.Model;
using FriendOrganizer.UI.Data.Lookups;
using FriendOrganizer.UI.Data.Repositories;
using FriendOrganizer.UI.View.Services;
using FriendOrganizer.UI.Wrapper;
using Prism.Commands;
using Prism.Events;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FriendOrganizer.UI.ViewModel
{
    public class FriendDetailViewModel : BaseDetailViewModel, IFriendDetailViewModel
    {
        #region fields and properties
        private IFriendRepository _friendRepository;
        private IMessageDialogService _messageDialogService;
        private IProgLanguageLookupDataService _progLanDataService;

        private FriendWrapper _friend;
        private PhoneNumberWrapper _selectedPhoneNumber;
        #endregion

        #region constructors
        public FriendDetailViewModel(
            IFriendRepository repository,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService,
            IProgLanguageLookupDataService progLanDataService)
            : base(eventAggregator)
        {
            _friendRepository = repository;
            _messageDialogService = messageDialogService;
            _progLanDataService = progLanDataService;

            ProgLanguages = new ObservableCollection<LookupItem>();
            PhoneNumbers = new ObservableCollection<PhoneNumberWrapper>();

            AddPhoneCommand = new DelegateCommand(OnAddPhoneExecute);
            RemovePhoneCommand = new DelegateCommand(OnRemovePhoneExecute, OnRemovePhoneCanExecute);
        }
        #endregion

        #region properties
        public FriendWrapper Friend
        {
            get { return _friend; }
            set
            {
                _friend = value;
                OnPropertyChanged();
            }
        }

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

        public ObservableCollection<LookupItem> ProgLanguages { get; }
        public ObservableCollection<PhoneNumberWrapper> PhoneNumbers { get; }
        #endregion

        #region IDetailViewModel implementation
        public override async Task LoadByIdAsync(int? id)
        {
            var model = id.HasValue ?
                await _friendRepository.GetByIdAsync(id.Value) : CreateFriend();

            InitFriend(model);
            InitPhoneNumber(model.PhoneNumbers);

            await LoadProgLanguagesAsync();
        }
        #endregion

        #region commands
        public ICommand AddPhoneCommand { get; }
        public ICommand RemovePhoneCommand { get; }

        protected override async void OnSaveExecute()
        {
            await _friendRepository.SaveAsync();
            HasChanges = _friendRepository.HasChanges();
            RiseDetailSaveCommand(Friend.Id, $"{Friend.FirstName} {Friend.LastName}");
        }

        protected override bool OnSaveCanExcute()
        {
            return Friend != null && !Friend.HasErrors && HasChanges
                && PhoneNumbers.All(x => !x.HasErrors);
        }

        protected override async void OnDeleteCommandExecute()
        {
            var result = _messageDialogService.ShowOkCancelDialog(
                $"Do you really want to delete {Friend.FirstName} {Friend.LastName}",
                "Question");

            if (result == MessageDialogResult.Cancel)
                return;

            _friendRepository.Remove(Friend.Model);
            await _friendRepository.SaveAsync();

            RiseDetailDeleteCommand(Friend.Id);
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
