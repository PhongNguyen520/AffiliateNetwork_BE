using SWD392_AffiliLinker.Core.Base;
using SWD392_AffiliLinker.Repositories.Entities;
using System.Linq.Expressions;

namespace SWD392_AffiliLinker.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        // query
        IQueryable<T> Entities { get; }

        // non async
        IEnumerable<T> GetAll();
        T? GetById(object id);
        void Insert(T obj);
        void InsertRange(IList<T> obj);
        void Update(T obj);
        void Delete(object id);
        void Save();

        // async
        Task<IList<T>> GetAllAsync();
        Task<BasePaginatedList<T>> GetPagging(IQueryable<T> query, int? index, int? pageSize);
        Task<T?> GetByIdAsync(object id);
        Task InsertAsync(T obj);
        Task UpdateAsync(T obj);
        Task DeleteAsync(object id);
        Task DeleteAsync(params object[] keyValues);
        Task SaveAsync();
        Task<T?> FindAsync(params object[] keyValues);

        Task<int> CountAsync();
        Task<IList<T>> SearchAsync(Expression<Func<T, bool>> filter);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> param);
        Task<T?> FindByAndInclude(Expression<Func<T, bool>> param, Expression<Func<T, object>> include);


	}
}
