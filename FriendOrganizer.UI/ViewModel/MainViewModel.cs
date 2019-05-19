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
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService)
        {
            _eventAggregator = eventAggregator;
            _friendDetailVMCreator = friendDetailVMCreator;
            _messageDialogService = messageDialogService;

            _eventAggregator
                .GetEvent<OpenDetailViewEvent>()
                .Subscribe(OnOpenDetailViewEvent);
            _eventAggregator.GetEvent<AfterFriendDeleteEvent>()
                .Subscribe(AfterFriendDeleted);

            NavigationVM = navigationVM;
        }
        #endregion

        #region commands
        private ICommand _createNewFriendCommand;
        public ICommand CreateNewFriendCommand
        {
            get
            {
                if (_createNewFriendCommand == null)
                    _createNewFriendCommand = new DelegateCommand(OnCreateNewFriendCommandExecuet);

                return _createNewFriendCommand;
            }
        }

        private void OnCreateNewFriendCommandExecuet()
        {
            OnOpenDetailViewEvent(null);
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
            }

            await DetailVM.LoadByIdAsync(args.Id);
        }

        private void AfterFriendDeleted(int id)
        {
            DetailVM = null;
        }
        #endregion
    }
}
