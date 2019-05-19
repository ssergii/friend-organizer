using Prism.Commands;
using System.Windows.Input;
using Prism.Events;
using FriendOrganizer.UI.Event;

namespace FriendOrganizer.UI.ViewModel
{
    public class NavigationItemViewModel : BaseViewModel
    {
        #region fields and properties
        private IEventAggregator _eventAggregator;
        private string _detailsViewModelName;

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

        public NavigationItemViewModel(
            int id,
            string displayMember,
            string detailsViewModelName,
            IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _detailsViewModelName = detailsViewModelName;

            Id = id;
            DisplayMember = displayMember;
        }

        #region commands
        private ICommand _openDetailViewCommand;
        public ICommand OpenDetailViewCommand
        {
            get
            {
                if (_openDetailViewCommand == null)
                    _openDetailViewCommand = new DelegateCommand(OnOpenDetailViewExecute);

                return _openDetailViewCommand;
            }
        }

        private void OnOpenDetailViewExecute()
        {
            _eventAggregator
                .GetEvent<OpenDetailViewEvent>()
                .Publish(new OpenDetailViewEventArgs
                {
                    Id = Id, VMName = _detailsViewModelName
                });
        }
        #endregion
    }
}
