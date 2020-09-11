using Jotaro.Entity.Interfaces;
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
    public class EfCoreGenericRepository<T, TId> : EfCoreKeylessRepository<T>, IGenericRepository<T, TId>
        where T : class, IHasId<TId>
    {
        public EfCoreGenericRepository(DbContext context) : base(context)
        {
        }

        public T? Find(TId id)
        {
            return context.Set<T>().AsNoTracking().FirstOrDefault(x => x.Id!.Equals(id));
        }

        public async ValueTask<T?> FindAsync(TId id, CancellationToken cancellationToken = default)
        {
            return await context.Set<T>().AsNoTracking().FirstOrDefaultAsync(x => x.Id!.Equals(id), cancellationToken);
        }

        public async Task UpdateAsync(TId id, Expression<Func<T, T>> extend, CancellationToken cancellationToken = default)
        {
            var item = await context.Set<T>().AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id!.Equals(id), cancellationToken);
            if (item != null)
            {
                context.Set<T>().Update(extend.Compile().Invoke(item));

                await context.SaveChangesAsync(cancellationToken);

                context.ChangeTracker.Clear();
            }

            // Do not upsert.
        }

        public Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            context.Set<T>().Update(entity);
            return context.SaveChangesAsync(cancellationToken)
                .ContinueWith(_ => context.ChangeTracker.Clear(), cancellationToken);
        }

        public async ValueTask<int> UpdateRangeAsync(params T[] entities)
        {
            context.Set<T>().UpdateRange(entities);
            var result = await context.SaveChangesAsync();
            context.ChangeTracker.Clear();
            return result;
        }

        public async ValueTask<int> UpdateRangeAsync(IEnumerable<T> entities,
            CancellationToken cancellationToken = default)
        {
            context.Set<T>().UpdateRange(entities);
            var result =  await context.SaveChangesAsync(cancellationToken);
            context.ChangeTracker.Clear();
            return result;
        }

        public async Task DeleteAsync(TId id, CancellationToken cancellationToken = default)
        {
            var item = await FindAsync(id, cancellationToken);
            if (item != null)
            {
                context.Set<T>().Remove(item);

                await context.SaveChangesAsync(cancellationToken);
            }

            context.ChangeTracker.Clear();
        }

        public async ValueTask<int> DeleteRangeAsync(params TId[] ids)
        {
            foreach (var id in ids)
            {
                var item = await FindAsync(id);
                if (item != null)
                {
                    context.Set<T>().Remove(item);
                }
            }

            var result = await context.SaveChangesAsync();
            context.ChangeTracker.Clear();
            return result;
        }

        public async ValueTask<int> DeleteRangeAsync(IEnumerable<TId> ids,
            CancellationToken cancellationToken = default)
        {
            foreach (var id in ids)
            {
                var item = await FindAsync(id, cancellationToken);
                if (item != null)
                {
                    context.Set<T>().Remove(item);
                }
            }

            var result = await context.SaveChangesAsync(cancellationToken);
            context.ChangeTracker.Clear();
            return result;
        }

        public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            context.Set<T>().Remove(entity);
            await context.SaveChangesAsync(cancellationToken);
            context.ChangeTracker.Clear();
        }

        public async ValueTask<int> DeleteRangeAsync(params T[] entities)
        {
            context.Set<T>().RemoveRange(entities);
            var result = await context.SaveChangesAsync();
            context.ChangeTracker.Clear();
            return result;
        }

        public async ValueTask<int> DeleteRangeAsync(IEnumerable<T> entities,
            CancellationToken cancellationToken = default)
        {
            context.Set<T>().RemoveRange(entities);
            var result = await context.SaveChangesAsync(cancellationToken);
            context.ChangeTracker.Clear();
            return result;
        }
    }
}
