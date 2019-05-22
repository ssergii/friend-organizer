using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data.Lookups
{
    public class LookupDataService : IFriendLookupDataService, IProgLanguageLookupDataService, IMeetingLookupDataService
    {
        private Func<FriendOrganizerDBContext> _contextCreator;

        public LookupDataService(Func<FriendOrganizerDBContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }

        public async Task<IEnumerable<LookupItem>> GetFriendLookupAsync()
        {
            using (var context = _contextCreator())
            {
                return await context.Friends.AsNoTracking()
                    .Select(x => new LookupItem { Id = x.Id, DisplayMember = x.FirstName +" " + x.LastName })
                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<LookupItem>> GetProgLanguagesLookupAsync()
        {
            using (var context = _contextCreator())
            {
                return await context.ProgrammingLanguages.AsNoTracking()
                    .Select(x => new LookupItem { Id = x.Id, DisplayMember = x.Name })
                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<LookupItem>> GetMeetingLookupAsync()
        {
            using(var context = _contextCreator())
            {
                return await context.Meetings.AsNoTracking()
                    .Select(x => new LookupItem { Id = x.Id, DisplayMember = x.Title })
                    .ToListAsync();
            }
        }
    }
}
