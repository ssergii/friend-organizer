using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data.Repositories
{
    public interface IRepository<T>
    {
        Task<T> GetByIdAsync(int id);
        Task SaveAsync();
        bool HasChanges();
        void Add(T obj);
        void Remove(T obj);
    }
}