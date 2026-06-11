using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace Cinema.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(
           Expression<Func<T, bool>>? filter = null,
           Expression<Func<T, object>>[]? includes = null,
           bool IsTracking = true
       );

        Task<T?> GetOneAsync(
           Expression<Func<T, bool>>? filter = null,
           Expression<Func<T, object>>[]? includes = null,
           bool IsTracking = true
       );

          Task<EntityEntry<T>> CreateAsync(T entity);

        void Update(T entity);

        void Delete(T entity);

        Task<int> CommitAsync();
    }
}
