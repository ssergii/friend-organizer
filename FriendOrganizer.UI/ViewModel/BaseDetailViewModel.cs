using FriendOrganizer.UI.Event;
using Prism.Commands;
using Prism.Events;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FriendOrganizer.UI.ViewModel
{
    public abstract class BaseDetailViewModel : BaseViewModel, IDetailViewModel
    {
        #region fields
        protected IEventAggregator _eventAggregator;

        private bool _hasChanges;
        #endregion

        #region constructors
        public BaseDetailViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExcute);
            DeleteCommand = new DelegateCommand(OnDeleteCommandExecute);
        }
        #endregion

        #region IDetailViewModel implementation
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

        public abstract Task LoadByIdAsync(int? id);
        #endregion

        #region commands
        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }

        protected abstract void OnSaveExecute();
        protected abstract bool OnSaveCanExcute();
        protected abstract void OnDeleteCommandExecute();

        protected void RiseDetailSaveCommand(int modelId, string displayMemeber)
        {
            _eventAggregator.GetEvent<AfterDetailSavedEvent>().Publish(
                new AfterDetailSavedEventArgs
                {
                    Id = modelId,
                    DisplayMember = displayMemeber,
                    VMName = GetType().Name
                });
        }

        protected void RiseDetailDeleteCommand(int modelId)
        {
            _eventAggregator.GetEvent<AfterDetailDeletedEvent>().Publish(
                new AfterDetailDeletedEventArgs
                {
                    Id = modelId,
                    VMName = GetType().Name
                });
        }
        #endregion
    }
}
