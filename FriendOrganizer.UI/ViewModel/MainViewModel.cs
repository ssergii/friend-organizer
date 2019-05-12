using FriendOrganizer.UI.Event;
using Prism.Events;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace FriendOrganizer.UI.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private IEventAggregator _eventAggregator;
        public Func<IFriendDetailViewModel> _friendDetailVMCreator;

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
            IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _friendDetailVMCreator = friendDetailVMCreator;

            _eventAggregator
                .GetEvent<OpenFriendDetailViewEvent>()
                .Subscribe(OnOpenFriendDetailViewEvent);

            NavigationVM = navigationVM;
        }
        #endregion

        #region public methods
        public async Task LoadAsync()
        {
            await NavigationVM.LoadAsync();
        }
        #endregion

        #region private methods
        private async void OnOpenFriendDetailViewEvent(int id)
        {
            if (FriendDetailVM != null && FriendDetailVM.HasChanges)
            {
                var result = MessageBox.Show(
                    "You've made changes. Navigate away?",
                    "Question",
                    MessageBoxButton.OKCancel);

                if (result == MessageBoxResult.Cancel)
                    return;
            }

            FriendDetailVM = _friendDetailVMCreator();

            await FriendDetailVM.LoadByIdAsync(id);
        }
        #endregion
    }
}
