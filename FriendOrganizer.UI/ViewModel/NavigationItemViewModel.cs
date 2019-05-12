using Prism.Commands;
using System.Windows.Input;
using System;
using Prism.Events;
using FriendOrganizer.UI.Event;

namespace FriendOrganizer.UI.ViewModel
{
    public class NavigationItemViewModel : BaseViewModel
    {
        #region fields and properties
        private IEventAggregator _eventAggregator;

        public int Id { get; }

        private string _displayMember;
        public string DisplayMember
        {
            get { return _displayMember; }
            set
            {
                _displayMember = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public NavigationItemViewModel(int id, string displayMember,
            IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            Id = id;
            DisplayMember = displayMember;
        }

        #region commands
        private ICommand _openFriendDetailViewCommand;
        public ICommand OpenFriendDetailViewCommand
        {
            get
            {
                if (_openFriendDetailViewCommand == null)
                    _openFriendDetailViewCommand = new DelegateCommand(OnOpenFriendDetailViewExecute);

                return _openFriendDetailViewCommand;
            }
        }

        private void OnOpenFriendDetailViewExecute()
        {
            _eventAggregator
                .GetEvent<OpenFriendDetailViewEvent>()
                .Publish(Id);
        }
        #endregion
    }
}
