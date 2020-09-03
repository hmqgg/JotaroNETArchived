using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Jotaro.Repository.Paginate.EFCore
{
    public static class EfPaginateExtensions
    {
        /// <summary>
        /// For EFCore query only.
        /// </summary>
        public static async Task<IPaginate<T>> ToPaginateAsync<T>(this IQueryable<T> source, int size, int index = 0,
            CancellationToken cancellationToken = default)
        {
            // Size must be greater than 0.
            if (size < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(size), "Page size must be greater than 0");
            }

            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Page index must be greater than 0");
            }

            var totalItemsCount = await source.CountAsync(cancellationToken).ConfigureAwait(false);
            var totalPagesCount = (int)Math.Ceiling(totalItemsCount / (double)size);

            // Fix page index.
            index = Math.Min(index, totalPagesCount - 1);

            // Actual query items.
            var items = await source.Skip(index * size).Take(size).ToListAsync(cancellationToken).ConfigureAwait(false);

            return new BasePaginate<T>(index, size, items, items.Count, totalPagesCount, totalItemsCount);
        }

        /// <summary>
        /// For EFCore query only.
        /// </summary>
        public static async Task<IPaginate<TResult>> ToPaginateAsync<TEntity, TResult>(this IQueryable<TEntity> source,
            Func<TEntity, TResult> conversion, int size, int index = 0,
            CancellationToken cancellationToken = default)
        {
            // Size must be greater than 0.
            if (size < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(size), "Page size must be greater than 0");
            }

            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Page index must be greater than 0");
            }

            var totalItemsCount = await source.CountAsync(cancellationToken).ConfigureAwait(false);
            var totalPagesCount = (int)Math.Ceiling(totalItemsCount / (double)size);

            // Fix page index.
            index = Math.Min(index, totalPagesCount - 1);

            // Actual query items.
            var items = source.Skip(index * size).Take(size).AsAsyncEnumerable();
            var results = new List<TResult>(size);

            await foreach (var item in items.WithCancellation(cancellationToken))
            {
                var result = conversion(item);
                results.Add(result);
            }

            return new ConversionPaginate<TEntity, TResult>(index, size, results, results.Count, totalPagesCount, totalItemsCount);
        }
    }
}
