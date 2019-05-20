using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;
using System.Data.Entity;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data.Repositories
{
    public class MeetingRepository : GenericRepository<Meeting, FriendOrganizerDBContext>, IMeetingRepository
    {
        public MeetingRepository(FriendOrganizerDBContext context) : base(context) { }

        public override async Task<Meeting> GetByIdAsync(int id)
        {
            return await _context.Meetings
                .Include(x => x.Friends)
                .SingleAsync(x => x.Id == id);
        }
    }
}
