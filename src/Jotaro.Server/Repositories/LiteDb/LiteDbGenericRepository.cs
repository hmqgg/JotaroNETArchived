using Jotaro.Entity.Interfaces;
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
    public class LiteDbGenericRepository<T, TId> : LiteDbKeylessRepository<T>, IGenericRepository<T, TId>
        where T : class, IHasId<TId>
    {
        public LiteDbGenericRepository(ILiteCollection<T> collection) : base(collection)
        {
        }

        public T? Find(TId id)
        {
            return collection.FindOne(Query.EQ(nameof(IHasId<T>.Id), id as BsonValue));
        }

        public ValueTask<T?> FindAsync(TId id, CancellationToken cancellationToken = default)
        {
            return new ValueTask<T?>(Find(id));
        }

        public Task UpdateAsync(TId id, Expression<Func<T, T>> extend, CancellationToken cancellationToken = default)
        {
            // Dangerous if unable to ensure uniqueness with Key.
            var entity = Find(id);
            if (entity != null)
            {
                entity = extend.Compile().Invoke(entity);
                collection.Update(entity);
            }

            return Task.CompletedTask;
        }

        public Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            collection.Update(entity);
            return Task.CompletedTask;
        }

        public ValueTask<int> UpdateRangeAsync(params T[] entities)
        {
            return new ValueTask<int>(collection.Update(entities));
        }

        public ValueTask<int> UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            return new ValueTask<int>(collection.Update(entities));
        }

        public Task DeleteAsync(TId id, CancellationToken cancellationToken = default)
        {
            // Dangerous if unable to ensure uniqueness with Key.
            var doc = collection.Delete(new BsonValue(id));
            return Task.CompletedTask;
        }

        public ValueTask<int> DeleteRangeAsync(params TId[] ids)
        {
            // It depends LiteDB to translate .Contains().
            return new ValueTask<int>(collection.DeleteMany(x => ids.Contains(x.Id)));
        }

        public ValueTask<int> DeleteRangeAsync(IEnumerable<TId> ids, CancellationToken cancellationToken = default)
        {
            // It depends LiteDB to translate .Contains().
            var idList = ids.ToList();
            return new ValueTask<int>(collection.DeleteMany(x => idList.Contains(x.Id)));
        }

        public Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            // Dangerous if unable to ensure uniqueness with Key.
            collection.Delete(new BsonValue(entity.Id));
            return Task.CompletedTask;
        }

        public ValueTask<int> DeleteRangeAsync(params T[] entities)
        {
            // Dangerous if unable to ensure uniqueness with Key.
            // It depends LiteDB to translate .Contains().
            var idList = entities.Select(x => x.Id).ToList();
            return new ValueTask<int>(collection.DeleteMany(x => idList.Contains(x.Id)));
        }

        public ValueTask<int> DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            // Dangerous if unable to ensure uniqueness with Key.
            // It depends LiteDB to translate .Contains().
            var idList = entities.Select(x => x.Id).ToList();
            return new ValueTask<int>(collection.DeleteMany(x => idList.Contains(x.Id)));
        }
    }
}
