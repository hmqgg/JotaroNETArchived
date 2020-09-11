using Jotaro.Repository.Paginate;
using Jotaro.Repository.Repositories.Interfaces;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Jotaro.Server.Repositories.LiteDb
{
    public class LiteDbKeylessRepository<T> : IKeylessRepository<T> where T : class
    {
        protected readonly ILiteCollection<T> collection;

        public LiteDbKeylessRepository(ILiteCollection<T> collection)
        {
            this.collection = collection;
        }

        public Task InsertAsync(T entity, CancellationToken cancellationToken = default)
        {
            collection.Insert(entity);
            return Task.CompletedTask;
        }

        public ValueTask<int> InsertRangeAsync(params T[] entities)
        {
            return new ValueTask<int>(collection.InsertBulk(entities));
        }

        public ValueTask<int> InsertRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            return new ValueTask<int>(collection.InsertBulk(entities));
        }

        public Task<bool> AnyAsync(Expression<Func<T, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(predicate == null ? collection.Count() > 0 : collection.Exists(predicate));
        }

        public ValueTask<int> CountAsync(Expression<Func<T, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
        {
            return new ValueTask<int>(predicate == null ? collection.Count() : collection.Count(predicate));
        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>>? predicate = null)
        {
            // Dangerous to use this, create the IQueryable in memory.
            var list = predicate == null ? collection.Query().ToList() : collection.Query().Where(predicate).ToList();
            return list.AsQueryable();
        }

        public Task<List<T>> FindByAsync(Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            CancellationToken cancellationToken = default)
        {
            // Dangerous to use this.
            var queryable = FindBy(predicate);
            // Order afterwards.
            if (orderBy != null)
            {
                queryable = orderBy(queryable);
            }

            return Task.FromResult(queryable.ToList());
        }

        public ValueTask<T?> FirstOrDefaultAsync(Expression<Func<T, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
        {
            var query = predicate == null ? collection.Query() : collection.Query().Where(predicate);
            return new ValueTask<T?>(query.FirstOrDefault());
        }

        public Task<IPaginate<T>> GetPageAsync(Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            int size = 50,
            int index = 0,
            CancellationToken cancellationToken = default)
        {
            var queryable = FindBy(predicate);
            // Order afterwards.
            if (orderBy != null)
            {
                queryable = orderBy(queryable);
            }

            return queryable.ToPaginateAsync(size, index, cancellationToken);
        }

        public Task<IPaginate<TResult>> GetPageAsync<TResult>(Func<T, TResult> conversion,
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            int size = 50,
            int index = 0,
            CancellationToken cancellationToken = default) where TResult : class
        {
            var queryable = FindBy(predicate);
            // Order afterwards.
            if (orderBy != null)
            {
                queryable = orderBy(queryable);
            }

            return queryable.ToPaginateAsync(conversion, size, index, cancellationToken);
        }

        public ValueTask<int> UpdateByAsync(Action<T> action,
            Expression<Func<T, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
        {
            // Fallback to foreach update.
            var queryable = FindBy(predicate);

            var i = 0;

            foreach (var item in queryable)
            {
                action.Invoke(item);
                if (collection.Update(item))
                {
                    i++;
                }
            }

            return new ValueTask<int>(i);
        }

        public ValueTask<int> UpdateByAsync(Expression<Func<T, T>> extend,
            Expression<Func<T, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
        {
            return new ValueTask<int>(collection.UpdateMany(extend, predicate ?? (_ => true)));
        }

        public ValueTask<int> RemoveByAsync(Expression<Func<T, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
        {
            return new ValueTask<int>(predicate == null ? collection.DeleteAll() : collection.DeleteMany(predicate));
        }
    }
}
