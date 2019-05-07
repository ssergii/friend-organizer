using FriendOrganizer.Model;
using System.Collections.Generic;

namespace FriendOrganizer.UI.Data
{
    public class FriendDataService : IDataService<Friend>
    {
        public IEnumerable<Friend> GetAll()
        {
            // TODO: load data from data base
            yield return new Friend { FirstName = "Jan", LastName = "Kowalski" };
            yield return new Friend { FirstName = "Adam", LastName = "Mickiewicz" };
            yield return new Friend { FirstName = "Kazimierz", LastName = "Wielki" };
            yield return new Friend { FirstName = "Andrzj", LastName = "Nowak" };
        }
    }
}
