using System;
using System.Collections.Generic;
using System.Linq;

namespace Jotaro.Repository.Paginate
{
    internal class ConversionPaginate<TEntity, TResult> : IPaginate<TResult>
    {
        public int Index { get; }
        public int Size { get; }
        public IList<TResult> Items { get; }
        public int Count { get; }
        public int TotalPagesCount { get; }
        public int TotalItemsCount { get; }

        /// <summary>
        /// Empty Paginate constructor.
        /// </summary>
        internal ConversionPaginate()
        {
            Items = Array.Empty<TResult>();
        }

        /// <summary>
        /// For extensions only.
        /// </summary>
        internal ConversionPaginate(int index, int size, IList<TResult> items, int count, int totalPagesCount, int totalItemsCount)
        {
            Index = index;
            Size = size;
            Items = items;
            Count = count;
            TotalPagesCount = totalPagesCount;
            TotalItemsCount = totalItemsCount;
        }

        internal ConversionPaginate(IEnumerable<TEntity> source, Func<TEntity, TResult> conversion, int size, int index)
        {
            ThrowHelper.ThrowArgumentOutOfRangeExceptionWhenCheckFailed(size, index);

            if (source is IQueryable<TEntity> query)
            {
                TotalItemsCount = query.Count();
                TotalPagesCount = (int) Math.Ceiling(TotalItemsCount / (double) size);

                // Fix page index.
                Index = Math.Min(index, TotalPagesCount - 1);
                Size = size;

                Items = query.Skip(Index * Size).Take(Size).AsEnumerable().Select(conversion).ToList();
                Count = Items.Count;
            }
            else
            {
                // Needs to prevent it from multiple enumeration, but it is dangerous to database query.
                var enumerable = source as TEntity[] ?? source.ToArray();

                TotalItemsCount = enumerable.Length;
                TotalPagesCount = (int) Math.Ceiling(TotalItemsCount / (double) size);

                // Fix page index.
                Index = Math.Min(index, TotalPagesCount - 1);
                Size = size;

                Items = enumerable.Skip(Index * Size).Take(Size).Select(conversion).ToList();
                Count = Items.Count;
            }
        }
    }
}
