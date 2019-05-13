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

        private IFriendDetailViewModel _friendDetailVM;
        public IFriendDetailViewModel FriendDetailVM
        {
            get { return _friendDetailVM; }
            private set
            {
                _friendDetailVM = value;
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
                .GetEvent<OpenFriendDetailViewEvent>()
                .Subscribe(OnOpenFriendDetailViewEvent);

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
            OnOpenFriendDetailViewEvent(null);
        }
        #endregion

        #region public methods
        public async Task LoadAsync()
        {
            await NavigationVM.LoadAsync();
        }
        #endregion

        #region private methods
        private async void OnOpenFriendDetailViewEvent(int? id)
        {
            if (FriendDetailVM != null && FriendDetailVM.HasChanges)
            {
                var result = _messageDialogService.ShowOkCancelDialog(
                    "You've made changes. Navigate away?",
                    "Question");

                if (result == MessageDialogResult.Cancel)
                    return;
            }

            FriendDetailVM = _friendDetailVMCreator();

            await FriendDetailVM.LoadByIdAsync(id);
        }
        #endregion
    }
}
