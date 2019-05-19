using System.Threading.Tasks;

namespace FriendOrganizer.UI.ViewModel
{
    public interface IDetailViewModel
    {
        Task LoadByIdAsync(int? id);
        bool HasChanges { get; }
    }
}
