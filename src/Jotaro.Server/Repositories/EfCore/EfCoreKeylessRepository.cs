using Jotaro.Repository.Paginate;
using Jotaro.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Jotaro.Server.Repositories.EfCore
{
    public class EfCoreKeylessRepository<T> : IKeylessRepository<T> where T : class
    {
        protected readonly DbContext context;

        public EfCoreKeylessRepository(DbContext context)
        {
            this.context = context;
        }

        public async Task InsertAsync(T entity, CancellationToken cancellationToken = default)
        {
            await context.Set<T>().AddAsync(entity, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            context.ChangeTracker.Clear();
        }

        public async ValueTask<int> InsertRangeAsync(params T[] entities)
        {
            await context.Set<T>().AddRangeAsync(entities);
            var result = await context.SaveChangesAsync();
            context.ChangeTracker.Clear();
            return result;
        }

        public async ValueTask<int> InsertRangeAsync(IEnumerable<T> entities,
            CancellationToken cancellationToken = default)
        {
            await context.Set<T>().AddRangeAsync(entities, cancellationToken);
            var result = await context.SaveChangesAsync(cancellationToken);
            context.ChangeTracker.Clear();
            return result;
        }

        public Task<bool> AnyAsync(Expression<Func<T, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
        {
            return predicate == null
                ? context.Set<T>().AnyAsync(cancellationToken)
                : context.Set<T>().AnyAsync(predicate, cancellationToken);
        }

        public async ValueTask<int> CountAsync(Expression<Func<T, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
        {
            return predicate == null
                ? await context.Set<T>().CountAsync(cancellationToken)
                : await context.Set<T>().CountAsync(predicate, cancellationToken);
        }

        private IQueryable<T> FindByAsTracking(Expression<Func<T, bool>>? predicate = null)
        {
            return predicate == null
                ? context.Set<T>()
                : context.Set<T>().Where(predicate);
        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>>? predicate = null)
        {
            return FindByAsTracking(predicate).AsNoTracking();
        }

        public Task<List<T>> FindByAsync(Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            CancellationToken cancellationToken = default)
        {
            IQueryable<T> set = FindBy(predicate);

            if (orderBy != null)
            {
                set = orderBy(set);
            }

            return set.ToListAsync(cancellationToken);
        }

        public async ValueTask<T?> FirstOrDefaultAsync(Expression<Func<T, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
        {
            var item = predicate == null
                ? await context.Set<T>().AsNoTracking().FirstOrDefaultAsync(cancellationToken)
                : await context.Set<T>().AsNoTracking().FirstOrDefaultAsync(predicate, cancellationToken);

            return item;
        }

        public Task<IPaginate<T>> GetPageAsync(Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            int size = 50,
            int index = 0,
            CancellationToken cancellationToken = default)
        {
            IQueryable<T> set = FindBy(predicate);

            if (orderBy != null)
            {
                set = orderBy(set);
            }

            return set.ToPaginateAsync(size, index, cancellationToken);
        }

        public Task<IPaginate<TResult>> GetPageAsync<TResult>(Func<T, TResult> conversion,
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            int size = 50,
            int index = 0,
            CancellationToken cancellationToken = default) where TResult : class
        {
            IQueryable<T> set = FindBy(predicate);

            if (orderBy != null)
            {
                set = orderBy(set);
            }

            return set.ToPaginateAsync(conversion, size, index, cancellationToken);
        }

        public async ValueTask<int> UpdateByAsync(Action<T> action,
            Expression<Func<T, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
        {
            // As tracking here.
            await foreach (var item in FindByAsTracking(predicate).AsAsyncEnumerable()
                .WithCancellation(cancellationToken))
            {
                if (item != null)
                {
                    action.Invoke(item);
                }
            }

            var result = await context.SaveChangesAsync(cancellationToken);
            context.ChangeTracker.Clear();
            return result;
        }

        public async ValueTask<int> UpdateByAsync(Expression<Func<T, T>> extend,
            Expression<Func<T, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
        {
            // No tracking here.
            await foreach (var item in FindBy(predicate).Select(extend).AsAsyncEnumerable()
                .WithCancellation(cancellationToken))
            {
                // Actually it needs key to work, otherwise throw exceptions.
                if (item != null)
                {
                    context.Update(item);
                }
            }

            var result = await context.SaveChangesAsync(cancellationToken);
            context.ChangeTracker.Clear();
            return result;
        }

        public async ValueTask<int> RemoveByAsync(Expression<Func<T, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
        {
            // Limited by EF Core.
            // As tracking here.
            await foreach (var item in FindByAsTracking(predicate).AsAsyncEnumerable()
                .WithCancellation(cancellationToken))
            {
                context.Set<T>().Remove(item);
            }

            context.Set<T>().RemoveRange(FindBy(predicate));

            var result = await context.SaveChangesAsync(cancellationToken);
            context.ChangeTracker.Clear();
            return result;
        }
    }
}
