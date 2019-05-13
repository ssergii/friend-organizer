﻿using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;
using System.Data.Entity;
using System.Threading.Tasks;
using System;

namespace FriendOrganizer.UI.Data.Repositories
{
    public class FriendRepository : IRepository<Friend>
    {
        private FriendOrganizerDBContext _context;

        public FriendRepository(FriendOrganizerDBContext context)
        {
            _context = context;
        }

        public void Add(Friend obj)
        {
            _context.Friends.Add(obj);
        }

        public async Task<Friend> GetByIdAsync(int Id)
        {
            return await _context.Friends.SingleAsync(x => x.Id == Id);
        }

        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
