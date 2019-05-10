using System.Threading.Tasks;

namespace FriendOrganizer.UI.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        public INavigationViewModel NavigationVM { get; }
        public IFriendDetailViewModel FriendDetailVM { get; }

        #region constructors
        public MainViewModel(
            INavigationViewModel navigationVM,
            IFriendDetailViewModel friendDetailVM)
        {
            NavigationVM = navigationVM;
            FriendDetailVM = friendDetailVM;
        }
        #endregion

        #region public methods
        public async Task LoadAsync()
        {
            await NavigationVM.LoadAsync();
        }
        #endregion
    }
}
