using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.ViewModel
{
    public class NavigationViewModel : INavigationViewModel
    {
        private IFriendLookupDataService _dataService;

        public NavigationViewModel(IFriendLookupDataService dataService)
        {
            _dataService = dataService;
        }

        public ObservableCollection<LookupItem> Friends { get; }

        public async Task LoadAsync()
        {
            Friends.Clear();

            var lookup = await _dataService.GetFriendLookupAsync();
            lookup.ToList().ForEach(x =>
                Friends.Add(x));
        }
    }
}
