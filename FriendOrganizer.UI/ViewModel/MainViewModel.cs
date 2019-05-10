using System.Threading.Tasks;

namespace FriendOrganizer.UI.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        public INavigationViewModel NavigationVM { get; }

        #region constructors
        public MainViewModel(INavigationViewModel navigationVM )
        {
            NavigationVM = navigationVM;

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
