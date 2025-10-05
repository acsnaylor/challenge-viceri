using ChallengeViceri.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ChallengeViceri.Domain.Entities;

namespace ChallengeViceri.Infrastructure.Data.Repositories
{
    public class RepositoryAsync<T> : IRepositoryAsync<T> where T : class
    {
        protected readonly ChallengeViceriContext _context;
        protected readonly DbSet<T> _entities;
        public RepositoryAsync(ChallengeViceriContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public Action<object>? BeforeEntityCreated { get; set; }

        public async Task<int> CountByAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) =>
            await _entities.CountAsync(predicate, cancellationToken);

        public async Task<int> CountAsync(CancellationToken cancellationToken = default) =>
            await _entities.CountAsync(cancellationToken);

        public void Delete(T entity)
        {
            _entities.Remove(entity);
        }

        public void Dispose() => _context.Dispose();

        public async Task ExecuteTransactionAsync(Func<Task> action, CancellationToken cancellationToken = default)
        {
            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                await action();
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) =>
            await _entities.AnyAsync(predicate, cancellationToken);

        public async Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>>? predicate = null,
            string? includeProperties = null,
            bool asNoTracking = true,
            CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = asNoTracking ? _entities.AsNoTracking() : _entities;

            if (predicate != null)
                query = query.Where(predicate);

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(includeProperty.Trim());
            }

            return await query.ToArrayAsync(cancellationToken);
        }

        public virtual async Task<T?> GetAsync(
            Expression<Func<T, bool>> predicate,
            string? includeProperties = null,
            bool asNoTracking = true,
            CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = asNoTracking ? _entities.AsNoTracking() : _entities;

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(includeProperty.Trim());
            }

            return await query.FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public virtual async Task<T?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
        {
            return await _entities.FindAsync(new object[] { id }, cancellationToken);
        }

        public virtual async Task<T?> GetByIdAsync(
            object id,
            string? includeProperties = null,
            CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = _entities;

            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(includeProperty.Trim());
            }

            return await query.SingleOrDefaultAsync(e => EF.Property<object>(e, "Id").Equals(id), cancellationToken);
        }

        public async Task<IEnumerable<T>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default) =>
            await _entities.AsNoTracking()
                           .Skip((pageNumber - 1) * pageSize)
                           .Take(pageSize)
                           .ToArrayAsync(cancellationToken);

        public async Task<T> InsertAsync(T entity, CancellationToken cancellationToken = default)
        {
            BeforeEntityCreated?.Invoke(entity);
            await _entities.AddAsync(entity, cancellationToken);
            return entity;
        }

        public void Update(T entity)
        {
            _entities.Update(entity);
        }

        public virtual async Task<(IEnumerable<T> items, int totalCount)> GetPagedByAsync(
            Expression<Func<T, bool>> predicate,
            int pageNumber,
            int pageSize,
            Expression<Func<T, object>>? orderBy = null,
            bool ascending = true,
            string? includeProperties = null,
            bool includeTotalCount = true,
            CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = _entities.AsNoTracking();

            if (predicate != null)
                query = query.Where(predicate);

            int totalCount = includeTotalCount ? await query.CountAsync(cancellationToken) : 0;

            if (orderBy != null)
                query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(includeProperty.Trim());
            }

            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToArrayAsync(cancellationToken);

            return (items, totalCount);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public IQueryable<T> Get(bool asNoTracking = true)
        {
            return asNoTracking ? _entities.AsNoTracking() : _entities;
        }

        public EntityEntry<T> Entry(T entity)
        {
            return _context.Entry(entity);
        }

        public async Task ReplaceManyToManyAsync<TEntity>(
            Guid entityId,
            Func<Task<T?>> getEntityWithNavigation,
            IEnumerable<Guid> newRelatedIds,
            Func<IEnumerable<Guid>, Task<List<TEntity>>> getRelatedEntities,
            Func<T, ICollection<TEntity>> getNavigationCollection,
            CancellationToken cancellationToken = default)
            where TEntity : class
        {
            var entity = await getEntityWithNavigation();
            if (entity == null)
                return;

            var navigationCollection = getNavigationCollection(entity);
            navigationCollection.Clear();

            var relatedEntities = await getRelatedEntities(newRelatedIds);
            foreach (var related in relatedEntities)
            {
                navigationCollection.Add(related);
            }

            _context.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
