using System.Data.Entity;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data.Repositories
{
    public class GenericRepository<TEntity, TContext> : IGenericRepository<TEntity>
        where TEntity : class
        where TContext : DbContext
    {
        protected readonly TContext _context;

        public GenericRepository(TContext context)
        {
            _context = context;
        }

        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
            return await _context.Set<TEntity>()
                .FindAsync(id);
        }

        public void Add(TEntity obj)
        {
            _context.Set<TEntity>().Add(obj);
        }

        public void Remove(TEntity obj)
        {
            _context.Set<TEntity>().Remove(obj);
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
