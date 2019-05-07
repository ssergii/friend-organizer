using System.Collections.Generic;

namespace FriendOrganizer.UI.Data
{
    public interface IDataService<T>
    {
        IEnumerable<T> GetAll();
    }
}