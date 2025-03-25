using Microsoft.EntityFrameworkCore;
using SWD392_AffiliLinker.Core.Base;
using SWD392_AffiliLinker.Repositories.Context;
using SWD392_AffiliLinker.Repositories.Interfaces;
using System.Linq.Expressions;

namespace SWD392_AffiliLinker.Repositories.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly DatabaseContext _context;
        protected readonly DbSet<T> _dbSet;
        public GenericRepository(DatabaseContext dbContext)
        {
            _context = dbContext;
            _dbSet = _context.Set<T>();
        }
        public IQueryable<T> Entities => _context.Set<T>();

        public void Delete(object id)
        {
            T entity = _dbSet.Find(id) ?? throw new Exception();
            _dbSet.Remove(entity);
        }

        // Delete method (async) for composite keys
        public async Task DeleteAsync(params object[] keyValues)
        {
            T entity = await _dbSet.FindAsync(keyValues) ?? throw new Exception("Entity not found.");
            _dbSet.Remove(entity);
        }

        public async Task DeleteAsync(object id)
        {
            T entity = await _dbSet.FindAsync(id) ?? throw new Exception();
            _dbSet.Remove(entity);
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.AsEnumerable();
        }

        public async Task<IList<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public T? GetById(object id)
        {
            return _dbSet.Find(id);
        }

        public async Task<T?> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<BasePaginatedList<T>> GetPagging(IQueryable<T> query, int? index, int? pageSize)
        {
            query = query.AsNoTracking();
			int count = await query.CountAsync();
			int pageIndex = (index > 0 ? index : 1) ?? 1;
            int pageSizeValue = (pageSize > 0 ? pageSize : 6) ?? 6;

			List<T> items = await query
				.Skip((pageIndex - 1) * pageSizeValue)
				.Take(pageSizeValue)
				.ToListAsync();
			return new BasePaginatedList<T>(items, count, index, pageSize);
        }

        public void Insert(T obj)
        {
            _dbSet.Add(obj);
        }

        public async Task InsertAsync(T obj)
        {
            await _dbSet.AddAsync(obj);
        }

        public void InsertRange(IList<T> obj)
        {
            _dbSet.AddRange(obj);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            var check = await _context.SaveChangesAsync();
            if(check == 0)
            {
                throw new Exception("Don't SaveChange!!!");
            }
        }

        public async Task<T?> FindAsync(params object[] keyValues) => await _dbSet.FindAsync(keyValues);
        public void Update(T obj)
        {
            _dbSet.Entry(obj).State = EntityState.Modified;
        }

        public Task UpdateAsync(T obj)
        {
            return Task.FromResult(_dbSet.Update(obj));
        }

        public async Task<IList<T>> SearchAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbSet.Where(filter).ToListAsync();
        }

        public async Task<int> CountAsync()
        {
            return await _context.Set<T>().CountAsync();
        }

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> param)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(param);
        }

        public async Task<T?> FindByAndInclude(Expression<Func<T, bool>> param, Expression<Func<T, object>> include)
        {
            return await _context.Set<T>().Include(include).FirstOrDefaultAsync(param);
        }
    }
}
