using FriendOrganizer.Model;
using FriendOrganizer.UI.Data.Repositories;
using FriendOrganizer.UI.View.Services;
using FriendOrganizer.UI.Wrapper;
using Prism.Commands;
using Prism.Events;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.ViewModel
{
    public class MeetingDetailViewModel : BaseDetailViewModel, IMeetingDetailViewModel
    {
        #region fields
        private IMeetingRepository _meetingRepository;
        private IMessageDialogService _messageDialogService;

        public MeetingWrapper _meeting;
        #endregion

        #region constructors
        public MeetingDetailViewModel(
            IEventAggregator eventAggregator,
            IMeetingRepository meetingRepository,
            IMessageDialogService messageDialogService)
            : base(eventAggregator)
        {
            _meetingRepository = meetingRepository;
            _messageDialogService = messageDialogService;
        }
        #endregion

        #region properties
        public MeetingWrapper Meeting
        {
            get { return _meeting; }
            set { _meeting = value; OnPropertyChanged(); }
        }
        #endregion

        #region IDetailViewModel implementation
        public override async Task LoadByIdAsync(int? id)
        {
            var model = id.HasValue ?
                await _meetingRepository.GetByIdAsync(id.Value) : CreateMeeting();

            InitMeeting(model);
        }
        #endregion

        #region commands
        protected override async void OnSaveExecute()
        {
            await _meetingRepository.SaveAsync();
            HasChanges = _meetingRepository.HasChanges();
            RiseDetailSaveCommand(Meeting.Id, Meeting.Title);
        }

        protected override bool OnSaveCanExcute()
        {
            return Meeting != null && !Meeting.HasErrors && HasChanges;
        }

        protected override async void OnDeleteCommandExecute()
        {
            var result = _messageDialogService.ShowOkCancelDialog(
                $"Do you really want to delete {Meeting.Title}",
                "Question");

            if (result == MessageDialogResult.Cancel)
                return;

            _meetingRepository.Remove(Meeting.Model);
            await _meetingRepository.SaveAsync();

            RiseDetailDeleteCommand(Meeting.Id);
        }
        #endregion

        #region methods
        private Meeting CreateMeeting()
        {
            var meeting = new Meeting();
            _meetingRepository.Add(meeting);

            return meeting;
        }

        private void InitMeeting(Meeting model)
        {
            Meeting = new MeetingWrapper(model);
            Meeting.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                    HasChanges = _meetingRepository.HasChanges();

                if (e.PropertyName == nameof(Meeting.HasErrors))
                    (SaveCommand as DelegateCommand).RaiseCanExecuteChanged();
            };

            (SaveCommand as DelegateCommand).RaiseCanExecuteChanged();
        }
        #endregion
    }
}
