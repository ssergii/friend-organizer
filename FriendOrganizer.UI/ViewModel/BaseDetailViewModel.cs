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
        private bool _hasChanges;

        protected IEventAggregator _eventAggregator;
        #endregion

        public BaseDetailViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExcute);
            DeleteCommand = new DelegateCommand(OnDeleteCommandExecute);
        }

        #region commands
        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }

        protected abstract void OnSaveExecute();
        protected abstract bool OnSaveCanExcute();
        protected abstract void OnDeleteCommandExecute();
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

        protected void RiseDetaildeleteCommand(int modelId)
        {
            _eventAggregator.GetEvent<AfterDetailDeletedEvent>().Publish(
                new AfterDetailDeletedEventArgs
                {
                    Id = modelId,
                    VMName = GetType().Name
                });
        }
    }
}
