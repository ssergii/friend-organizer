using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FriendOrganizer.UI.Data
{
    public class FriendDataService : IDataService<Friend>
    {
        private Func<FriendOrganizerDBContext> _contextCreator;

        public FriendDataService(Func<FriendOrganizerDBContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }

        public IEnumerable<Friend> GetAll()
        {
            using (var ctx = _contextCreator())
            {
                return ctx.Friends.AsNoTracking().ToList();
            }
        }
    }
}
