using Jotaro.Entity.Interfaces;
using Jotaro.Repository.Paginate;
using Jotaro.Repository.Repositories.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Jotaro.Repository.Repositories.InMemory
{
    /// <summary>
    /// Singleton, for testing purpose only.
    /// </summary>
    public class InMemoryRepository<T, TId> : IGenericRepository<T, TId> where T : class, IHasId<TId>
    {
        private readonly ConcurrentDictionary<TId, T> dictionary;

        public InMemoryRepository()
        {
            dictionary = new ConcurrentDictionary<TId, T>();
        }

        public InMemoryRepository(ConcurrentDictionary<TId, T> dict)
        {
            dictionary = dict;
        }

        public Task InsertAsync(T entity, CancellationToken cancellationToken = default)
        {
            dictionary.TryAdd(entity.Id, entity);
            return Task.CompletedTask;
        }

        public ValueTask<int> InsertRangeAsync(params T[] entities)
        {
            var result = 0;

            // Not worth it to use TPL, but just a test.
            Parallel.ForEach(entities, entity =>
            {
                if (dictionary.TryAdd(entity.Id, entity))
                {
                    Interlocked.Increment(ref result);
                }
            });

            return new ValueTask<int>(result);
        }

        public ValueTask<int> InsertRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            var result = 0;

            var po = new ParallelOptions
            {
                CancellationToken = cancellationToken
            };

            Parallel.ForEach(entities, po, entity =>
            {
                if (dictionary.TryAdd(entity.Id, entity))
                {
                    Interlocked.Increment(ref result);
                }

                po.CancellationToken.ThrowIfCancellationRequested();
            });

            return new ValueTask<int>(result);
        }

        public Task<bool> AnyAsync(Expression<Func<T, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(
                predicate == null ? dictionary.Any() : dictionary.Values.AsQueryable().Any(predicate));
        }

        public ValueTask<int> CountAsync(Expression<Func<T, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
        {
            return new ValueTask<int>(dictionary.Count);
        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>>? predicate = null)
        {
            var queryable = dictionary.Values.AsQueryable();

            if (predicate != null)
            {
                queryable = queryable.Where(predicate);
            }

            return queryable;
        }

        public Task<List<T>> FindByAsync(Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            CancellationToken cancellationToken = default)
        {
            var queryable = dictionary.Values.AsQueryable();

            if (predicate != null)
            {
                queryable = queryable.Where(predicate);
            }

            if (orderBy != null)
            {
                queryable = orderBy(queryable);
            }

            return Task.FromResult(queryable.ToList());
        }

        public ValueTask<T?> FirstOrDefaultAsync(Expression<Func<T, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
        {
            var queryable = dictionary.Values.AsQueryable();

            if (predicate != null)
            {
                queryable = queryable.Where(predicate);
            }

            return new ValueTask<T?>(queryable.FirstOrDefault());
        }

        public Task<IPaginate<T>> GetPageAsync(Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            int size = 50,
            int index = 0,
            CancellationToken cancellationToken = default)
        {
            var queryable = dictionary.Values.AsQueryable();

            if (predicate != null)
            {
                queryable = queryable.Where(predicate);
            }

            if (orderBy != null)
            {
                queryable = orderBy(queryable);
            }

            // To test different approach of ToPaginate extensions, here use IEnumerable.ToPaginate.
            var list = queryable.ToList();
            return Task.FromResult(list.ToPaginate(size, index));
        }

        public Task<IPaginate<TResult>> GetPageAsync<TResult>(Func<T, TResult> conversion,
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            int size = 50,
            int index = 0,
            CancellationToken cancellationToken = default) where TResult : class
        {
            var queryable = dictionary.Values.AsQueryable();

            if (predicate != null)
            {
                queryable = queryable.Where(predicate);
            }

            if (orderBy != null)
            {
                queryable = orderBy(queryable);
            }

            return Task.FromResult(queryable.ToPaginate(conversion, size, index));
        }

        public T? Find(TId id)
        {
            return dictionary.TryGetValue(id, out var result) ? result : null;
        }

        public ValueTask<T?> FindAsync(TId id, CancellationToken cancellationToken = default)
        {
            return new ValueTask<T?>(dictionary.TryGetValue(id, out var result) ? result : null);
        }

        public ValueTask<int> UpdateByAsync(Action<T> action,
            Expression<Func<T, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
        {
            var queryable = dictionary.Values.AsQueryable();

            if (predicate != null)
            {
                queryable = queryable.Where(predicate);
            }

            var i = 0;
            foreach (var item in queryable)
            {
                i++;
                action(item);
            }

            return new ValueTask<int>(i);
        }

        public ValueTask<int> UpdateByAsync(Expression<Func<T, T>> extend,
            Expression<Func<T, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
        {
            var queryable = dictionary.Values.AsQueryable();

            if (predicate != null)
            {
                queryable = queryable.Where(predicate);
            }

            var func = extend.Compile();

            var i = 0;
            foreach (var item in queryable)
            {
                if (dictionary.TryUpdate(item.Id, func(item), item))
                {
                    i++;
                }
            }

            return new ValueTask<int>(i);
        }

        public Task UpdateAsync(TId id, Expression<Func<T, T>> extend, CancellationToken cancellationToken = default)
        {
            if (dictionary.TryGetValue(id, out var entity))
            {
                dictionary[id] = extend.Compile().Invoke(entity);
            }

            return Task.CompletedTask;
        }

        public Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            if (dictionary.ContainsKey(entity.Id))
            {
                dictionary[entity.Id] = entity;
            }

            return Task.CompletedTask;
        }

        public ValueTask<int> UpdateRangeAsync(params T[] entities)
        {
            var result = 0;

            Parallel.ForEach(entities, entity =>
            {
                if (dictionary.ContainsKey(entity.Id))
                {
                    dictionary[entity.Id] = entity;
                    Interlocked.Increment(ref result);
                }
            });

            return new ValueTask<int>(result);
        }

        public ValueTask<int> UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            var result = 0;

            var po = new ParallelOptions
            {
                CancellationToken = cancellationToken
            };

            Parallel.ForEach(entities, po, entity =>
            {
                if (dictionary.ContainsKey(entity.Id))
                {
                    dictionary[entity.Id] = entity;
                    Interlocked.Increment(ref result);
                }

                po.CancellationToken.ThrowIfCancellationRequested();
            });

            return new ValueTask<int>(result);
        }

        public ValueTask<int> RemoveByAsync(Expression<Func<T, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
        {
            var queryable = dictionary.Values.AsQueryable();

            if (predicate != null)
            {
                queryable = queryable.Where(predicate);
            }

            var idList = queryable.Select(v => v.Id).ToList();
            var result = idList.Sum(id => dictionary.Remove(id, out _) ? 1 : 0);

            return new ValueTask<int>(result);
        }

        public Task DeleteAsync(TId id, CancellationToken cancellationToken = default)
        {
            dictionary.Remove(id, out _);
            return Task.CompletedTask;
        }

        public ValueTask<int> DeleteRangeAsync(params TId[] ids)
        {
            var result = 0;

            Parallel.ForEach(ids, id =>
            {
                if (dictionary.Remove(id, out _))
                {
                    Interlocked.Increment(ref result);
                }
            });

            return new ValueTask<int>(result);
        }

        public ValueTask<int> DeleteRangeAsync(IEnumerable<TId> ids, CancellationToken cancellationToken = default)
        {
            var result = 0;

            var po = new ParallelOptions
            {
                CancellationToken = cancellationToken
            };

            Parallel.ForEach(ids, po, id =>
            {
                if (dictionary.Remove(id, out _))
                {
                    Interlocked.Increment(ref result);
                }

                po.CancellationToken.ThrowIfCancellationRequested();
            });

            return new ValueTask<int>(result);
        }

        public Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            dictionary.Remove(entity.Id, out _);
            return Task.CompletedTask;
        }

        public ValueTask<int> DeleteRangeAsync(params T[] entities)
        {
            var result = 0;

            Parallel.ForEach(entities, entity =>
            {
                if (dictionary.Remove(entity.Id, out _))
                {
                    Interlocked.Increment(ref result);
                }
            });

            return new ValueTask<int>(result);
        }

        public ValueTask<int> DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            var result = 0;

            var po = new ParallelOptions
            {
                CancellationToken = cancellationToken
            };

            Parallel.ForEach(entities, po, entity =>
            {
                if (dictionary.Remove(entity.Id, out _))
                {
                    Interlocked.Increment(ref result);
                }

                po.CancellationToken.ThrowIfCancellationRequested();
            });

            return new ValueTask<int>(result);
        }
    }
}
