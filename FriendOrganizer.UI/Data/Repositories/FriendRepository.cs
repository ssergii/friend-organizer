using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;
using System.Data.Entity;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data.Repositories
{
    public class FriendRepository : GenericRepository<Friend, FriendOrganizerDBContext>, IFriendRepository
    {
        public FriendRepository(FriendOrganizerDBContext context) : base(context) { }

        public override async Task<Friend> GetByIdAsync(int Id)
        {
            return await _context.Friends
                .Include(x => x.PhoneNumbers)
                .SingleAsync(x => x.Id == Id);
        }

        public void RemovePhoneNumber(PhoneNumber model)
        {
            _context.PhoneNumbers.Remove(model);
        }
    }
}
