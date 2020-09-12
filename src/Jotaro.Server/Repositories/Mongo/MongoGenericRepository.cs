using Jotaro.Entity.Interfaces;
using Jotaro.Repository.Repositories.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Jotaro.Server.Repositories.Mongo
{
    public class MongoGenericRepository<T, TId> : MongoKeylessRepository<T>, IGenericRepository<T, TId>
        where T : class, IHasId<TId>
    {
        public MongoGenericRepository(IMongoCollection<T> collection) : base(collection)
        {
        }

        public T? Find(TId id)
        {
            return collection.Find(x => x.Id!.Equals(id)).FirstOrDefault();
        }

        public async ValueTask<T?> FindAsync(TId id, CancellationToken cancellationToken = default)
        {
            return await collection.Find(x => x.Id!.Equals(id)).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task UpdateAsync(TId id,
            Expression<Func<T, T>> extend,
            CancellationToken cancellationToken = default)
        {
            var item = await FindAsync(id, cancellationToken);
            if (item != null)
            {
                item = extend.Compile().Invoke(item);

                await collection.ReplaceOneAsync(x => x.Id!.Equals(id), item, cancellationToken: cancellationToken);
            }
        }

        public Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            return collection.ReplaceOneAsync(x => x.Id!.Equals(entity.Id), entity,
                cancellationToken: cancellationToken);
        }

        public async ValueTask<int> UpdateRangeAsync(params T[] entities)
        {
            var result = 0;
            foreach (var entity in entities)
            {
                var replaceResult = await collection.ReplaceOneAsync(x => x.Id!.Equals(entity.Id), entity);
                result += (int) replaceResult.ModifiedCount;
            }

            return result;
        }

        public async ValueTask<int> UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            var result = 0;
            foreach (var entity in entities)
            {
                var replaceResult = await collection.ReplaceOneAsync(x => x.Id!.Equals(entity.Id), entity,
                    cancellationToken: cancellationToken);
                result += (int) replaceResult.ModifiedCount;
            }

            return result;
        }

        public Task DeleteAsync(TId id, CancellationToken cancellationToken = default)
        {
            return collection.DeleteOneAsync(x => x.Id!.Equals(id), cancellationToken);
        }

        public async ValueTask<int> DeleteRangeAsync(params TId[] ids)
        {
            var result = await collection.DeleteManyAsync(x => ids.Contains(x.Id));
            return (int) result.DeletedCount;
        }

        public async ValueTask<int> DeleteRangeAsync(IEnumerable<TId> ids, CancellationToken cancellationToken = default)
        {
            var idList = ids.ToArray();
            var result = await collection.DeleteManyAsync(x => idList.Contains(x.Id), cancellationToken);
            return (int) result.DeletedCount;
        }

        public Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            return collection.DeleteOneAsync(x => x.Id!.Equals(entity.Id), cancellationToken);
        }

        public async ValueTask<int> DeleteRangeAsync(params T[] entities)
        {
            var ids = entities.Select(x => x.Id).ToArray();
            var result = await collection.DeleteManyAsync(x => ids.Contains(x.Id));
            return (int) result.DeletedCount;
        }

        public async ValueTask<int> DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            var ids = entities.Select(x => x.Id).ToArray();
            var result = await collection.DeleteManyAsync(x => ids.Contains(x.Id), cancellationToken);
            return (int) result.DeletedCount;
        }
    }
}
