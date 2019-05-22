using FriendOrganizer.UI.Event;
using FriendOrganizer.UI.View.Services;
using Prism.Commands;
using Prism.Events;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FriendOrganizer.UI.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private IEventAggregator _eventAggregator;
        private Func<IFriendDetailViewModel> _friendDetailVMCreator;
        private Func<IMeetingDetailViewModel> _meetingDetailVMCreator;
        private IMessageDialogService _messageDialogService;

        public INavigationViewModel NavigationVM { get; }

        private IDetailViewModel _detailVM;
        public IDetailViewModel DetailVM
        {
            get { return _detailVM; }
            private set
            {
                _detailVM = value;
                OnPropertyChanged();
            }
        }

        #region constructors
        public MainViewModel(
            INavigationViewModel navigationVM,
            Func<IFriendDetailViewModel> friendDetailVMCreator,
            Func<IMeetingDetailViewModel> meetingDetailVMCreator,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService)
        {
            _eventAggregator = eventAggregator;
            _friendDetailVMCreator = friendDetailVMCreator;
            _meetingDetailVMCreator = meetingDetailVMCreator;
            _messageDialogService = messageDialogService;

            _eventAggregator
                .GetEvent<OpenDetailViewEvent>()
                .Subscribe(OnOpenDetailViewEvent);
            _eventAggregator.GetEvent<AfterDetailDeletedEvent>()
                .Subscribe(AfterDetailDeleted);

            NavigationVM = navigationVM;
        }
        #endregion

        #region commands
        private ICommand _createNewDetailCommand;
        public ICommand CreateNewDetailCommand
        {
            get
            {
                if (_createNewDetailCommand == null)
                    _createNewDetailCommand = new DelegateCommand<Type>(OnCreateNewDetilExecuet);

                return _createNewDetailCommand;
            }
        }

        private void OnCreateNewDetilExecuet(Type viewModelType)
        {
            OnOpenDetailViewEvent(new OpenDetailViewEventArgs { VMName = viewModelType.Name });
        }
        #endregion

        #region public methods
        public async Task LoadAsync()
        {
            await NavigationVM.LoadAsync();
        }
        #endregion

        #region private methods
        private async void OnOpenDetailViewEvent(OpenDetailViewEventArgs args)
        {
            if (DetailVM != null && DetailVM.HasChanges)
            {
                var result = _messageDialogService.ShowOkCancelDialog(
                    "You've made changes. Navigate away?",
                    "Question");

                if (result == MessageDialogResult.Cancel)
                    return;
            }

            switch (args.VMName)
            {
                case nameof(FriendDetailViewModel):
                    DetailVM = _friendDetailVMCreator();
                    break;
                case nameof(MeetingDetailViewModel):
                    DetailVM = _meetingDetailVMCreator();
                    break;
                default:
                    throw new Exception($"ViewModel ${args.VMName} not mapped");
            }

            await DetailVM.LoadByIdAsync(args.Id);
        }

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            DetailVM = null;
        }
        #endregion
    }
}
