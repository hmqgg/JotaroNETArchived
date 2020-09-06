using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Jotaro.Repository.Paginate
{
    public static class PaginateAsyncExtensions
    {
        /// <summary>
        /// For IQueryable|IAsyncEnumerable only.
        /// </summary>
        public static async Task<IPaginate<T>> ToPaginateAsync<T>(this IQueryable<T> source, int size, int index = 0,
            CancellationToken cancellationToken = default)
        {
            ThrowHelper.ThrowArgumentOutOfRangeExceptionWhenCheckFailed(size, index);

            // Count.
            var totalItemsCount = source.Count();
            var totalPagesCount = (int)Math.Ceiling(totalItemsCount / (double)size);

            // Fix page index.
            index = Math.Min(index, totalPagesCount - 1);

            // Actual query items.
            var items = await source.Skip(index * size).Take(size).ToListAsync(cancellationToken).ConfigureAwait(false);

            return new BasePaginate<T>(index, size, items, items.Count, totalPagesCount, totalItemsCount);
        }

        /// <summary>
        /// For IQueryable|IAsyncEnumerable only.
        /// </summary>
        public static async Task<IPaginate<TResult>> ToPaginateAsync<TEntity, TResult>(this IQueryable<TEntity> source,
            Func<TEntity, TResult> conversion, int size, int index = 0,
            CancellationToken cancellationToken = default)
        {
            ThrowHelper.ThrowArgumentOutOfRangeExceptionWhenCheckFailed(size, index);

            var totalItemsCount = source.Count();
            var totalPagesCount = (int)Math.Ceiling(totalItemsCount / (double)size);

            // Fix page index.
            index = Math.Min(index, totalPagesCount - 1);

            // Actual query items.
            var items = await source.Skip(index * size).Take(size).ToListAsync(conversion, cancellationToken).ConfigureAwait(false);

            return new ConversionPaginate<TEntity, TResult>(index, size, items, items.Count, totalPagesCount, totalItemsCount);
        }

        private static async Task<List<TSource>> ToListAsync<TSource>(this IQueryable<TSource> source,
            CancellationToken cancellationToken = default)
        {
            var list = new List<TSource>();

            if (source is IAsyncEnumerable<TSource> asyncEnumerable)
            {
                await foreach (var element in asyncEnumerable.WithCancellation(cancellationToken))
                {
                    list.Add(element);
                }
            }
            else
            {
                // Fallback to sync.
                foreach (var element in source)
                {
                    list.Add(element);
                    cancellationToken.ThrowIfCancellationRequested();
                }
            }

            return list;
        }

        private static async Task<List<TResult>> ToListAsync<TSource, TResult>(this IQueryable<TSource> source,
            Func<TSource, TResult> conversion,
            CancellationToken cancellationToken = default)
        {
            var list = new List<TResult>();

            if (source is IAsyncEnumerable<TSource> asyncEnumerable)
            {
                await foreach (var element in asyncEnumerable.WithCancellation(cancellationToken))
                {
                    list.Add(conversion(element));
                }
            }
            else
            {
                // Fallback to sync.
                foreach (var element in source)
                {
                    list.Add(conversion(element));
                    cancellationToken.ThrowIfCancellationRequested();
                }
            }

            return list;
        }
    }
}
