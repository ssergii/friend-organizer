using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data
{
    public class FriendDataService : IDataService<Friend>
    {
        private Func<FriendOrganizerDBContext> _contextCreator;

        public FriendDataService(Func<FriendOrganizerDBContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }

        public async Task<Friend> GetByIdAsync(int Id)
        {
            using (var ctx = _contextCreator())
            {
                return await ctx.Friends.AsNoTracking().SingleAsync(x => x.Id == Id);
            }
        }

        public async void SaveAsync(Friend friend)
        {
            using (var ctx = _contextCreator())
            {
                ctx.Friends.Attach(friend);
                ctx.Entry(friend).State = EntityState.Modified;
                await ctx.SaveChangesAsync();
            }
        }
    }
}
