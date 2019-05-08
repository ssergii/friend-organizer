using System.Threading.Tasks;

namespace FriendOrganizer.UI.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        INavigationViewModel _navigationVM;

        #region constructors
        public MainViewModel(INavigationViewModel navigationVM )
        {
            _navigationVM = navigationVM;

        }
        #endregion

        #region public methods
        public async Task LoadAsync()
        {
            await _navigationVM.LoadAsync();
        }
        #endregion
    }
}
