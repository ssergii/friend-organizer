using System.Collections.Generic;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data
{
    public interface IDataService<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
    }
}