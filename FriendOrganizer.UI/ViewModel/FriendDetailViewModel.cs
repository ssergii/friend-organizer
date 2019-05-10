using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;

namespace FriendOrganizer.UI.ViewModel
{
    public class FriendDetailViewModel : BaseViewModel, IFriendDetailViewModel
    {
        #region fields and properties
        private IDataService<Friend> _dataService;

        private Friend _friend;
        public Friend Friend
        {
            get { return _friend; }
            set
            {
                _friend = value;
                OnPropertyChanged();
            }
        }
        #endregion


        public FriendDetailViewModel(IDataService<Friend> dataService)
        {
            _dataService = dataService;
        }

        public async void LoadByIdAsync(int id)
        {
             Friend = await _dataService.GetByIdAsync(id);
        }
    }
}
