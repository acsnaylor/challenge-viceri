using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace ChallengeViceri.Infrastructure.Data.Repositories
{
    public interface IRepositoryAsync<T> : IDisposable where T : class
    {
        Task<int> CountAsync(CancellationToken cancellationToken = default);
        Task<int> CountByAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>>? predicate = null,
            string? includeProperties = null,
            bool asNoTracking = true,
            CancellationToken cancellationToken = default);

        Task<T?> GetAsync(
            Expression<Func<T, bool>> predicate,
            string? includeProperties = null,
            bool asNoTracking = true,
            CancellationToken cancellationToken = default);

        Task<T?> GetByIdAsync(object id, CancellationToken cancellationToken = default);
        Task<T?> GetByIdAsync(object id, string? includeProperties = null, CancellationToken cancellationToken = default);

        Task<(IEnumerable<T> items, int totalCount)> GetPagedByAsync(
            Expression<Func<T, bool>> predicate,
            int pageNumber,
            int pageSize,
            Expression<Func<T, object>>? orderBy = null,
            bool ascending = true,
            string? includeProperties = null,
            bool includeTotalCount = true,
            CancellationToken cancellationToken = default);

        Task<T> InsertAsync(T entity, CancellationToken cancellationToken = default);

        void Update(T entity);

        void Delete(T entity);

        void SaveChanges();
        IQueryable<T> Get(bool asNoTracking = true);

        EntityEntry<T> Entry(T entity);
    }
}

