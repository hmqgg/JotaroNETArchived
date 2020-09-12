using Jotaro.Repository.Paginate;
using Jotaro.Repository.Repositories.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Jotaro.Server.Repositories.Mongo
{
    public class MongoKeylessRepository<T> : IKeylessRepository<T> where T : class
    {
        protected readonly IMongoCollection<T> collection;

        public MongoKeylessRepository(IMongoCollection<T> collection)
        {
            this.collection = collection;
        }

        public Task InsertAsync(T entity, CancellationToken cancellationToken = default)
        {
            return collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
        }

        public async ValueTask<int> InsertRangeAsync(params T[] entities)
        {
            // Can not get the actual number.
            await collection.InsertManyAsync(entities);
            return entities.Length;
        }

        public async ValueTask<int> InsertRangeAsync(IEnumerable<T> entities,
            CancellationToken cancellationToken = default)
        {
            // Can not get the actual number.
            var list = entities.ToList();
            await collection.InsertManyAsync(list, cancellationToken: cancellationToken);
            return list.Count;
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
        {
            var cursor = await collection.FindAsync(predicate ?? (_ => true), cancellationToken: cancellationToken);
            return await cursor.AnyAsync(cancellationToken);
        }

        public async ValueTask<int> CountAsync(Expression<Func<T, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
        {
            var count = predicate == null
                ? await collection.EstimatedDocumentCountAsync(cancellationToken: cancellationToken)
                : await collection.CountDocumentsAsync(predicate, null, cancellationToken);

            // Will NOT throw OverflowException if > int.
            return (int) count;
        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>>? predicate = null)
        {
            return predicate == null ? collection.AsQueryable() : collection.AsQueryable().Where(predicate);
        }

        public async Task<List<T>> FindByAsync(Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            CancellationToken cancellationToken = default)
        {
            var cursor = await collection.FindAsync(predicate ?? (_ => true), cancellationToken: cancellationToken);
            // In memory sorting.
            var result = new List<T>();

            await foreach (var item in ToAsyncEnumerable(cursor).WithCancellation(cancellationToken))
            {
                result.Add(item);
            }

            var queryable = result.AsQueryable();
            if (orderBy != null)
            {
                queryable = orderBy.Invoke(queryable);
            }

            return queryable.ToList();
        }

        public async ValueTask<T?> FirstOrDefaultAsync(Expression<Func<T, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
        {
            var cursor = await collection.FindAsync(predicate ?? (_ => true), cancellationToken: cancellationToken);
            return await cursor.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IPaginate<T>> GetPageAsync(Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            int size = 50,
            int index = 0,
            CancellationToken cancellationToken = default)
        {
            var list = await FindByAsync(predicate, orderBy, cancellationToken);
            return list.ToPaginate(size, index);
        }

        public async Task<IPaginate<TResult>> GetPageAsync<TResult>(Func<T, TResult> conversion,
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            int size = 50,
            int index = 0,
            CancellationToken cancellationToken = default) where TResult : class
        {
            var list = await FindByAsync(predicate, orderBy, cancellationToken);
            return list.ToPaginate(conversion, size, index);
        }

        public async ValueTask<int> RemoveByAsync(Expression<Func<T, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
        {
            var result = await collection.DeleteManyAsync(predicate ?? (_ => true), cancellationToken);
            return (int) result.DeletedCount;
        }

        public async ValueTask<int> UpdateByAsync(Action<T> action,
            Expression<Func<T, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
        {
            var cursor = await collection.FindAsync(predicate ?? (_ => true), cancellationToken: cancellationToken);

            var result = 0;
            await foreach (var item in ToAsyncEnumerable(cursor).WithCancellation(cancellationToken))
            {
                var filter = item.ToBsonDocument();
                action.Invoke(item);

                // Slow because replace one by one.
                var replaceResult = await collection.ReplaceOneAsync(filter, item, cancellationToken: cancellationToken);

                result += (int) replaceResult.ModifiedCount;
            }

            return result;
        }

        public async ValueTask<int> UpdateByAsync(Expression<Func<T, T>> extend,
            Expression<Func<T, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
        {
            var cursor = await collection.FindAsync(predicate ?? (_ => true), cancellationToken: cancellationToken);
            var func = extend.Compile();

            var result = 0;
            await foreach (var item in ToAsyncEnumerable(cursor).WithCancellation(cancellationToken))
            {
                var filter = item.ToBsonDocument();
                var newItem = func.Invoke(item);

                // Slow because replace one by one.
                var replaceResult = await collection.ReplaceOneAsync(filter, newItem, cancellationToken: cancellationToken);

                result += (int) replaceResult.ModifiedCount;
            }

            return result;
        }

        private static async IAsyncEnumerable<T> ToAsyncEnumerable(IAsyncCursor<T> asyncCursor)
        {
            while (await asyncCursor.MoveNextAsync())
            {
                foreach (var current in asyncCursor.Current)
                {
                    yield return current;
                }
            }
        }
    }
}
