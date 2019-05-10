using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data
{
    public interface IDataService<T>
    {
        Task<T> GetByIdAsync(int id);
        void SaveAsync(T obj);
    }
}