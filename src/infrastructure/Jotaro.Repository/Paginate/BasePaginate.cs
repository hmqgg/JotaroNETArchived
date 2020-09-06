using System;
using System.Collections.Generic;
using System.Linq;

namespace Jotaro.Repository.Paginate
{
    internal class BasePaginate<T> : IPaginate<T>
    {
        public int Index { get; }
        public int Size { get; }
        public IList<T> Items { get; }
        public int Count { get; }
        public int TotalPagesCount { get; }
        public int TotalItemsCount { get; }

        /// <summary>
        /// Empty Paginate constructor.
        /// </summary>
        internal BasePaginate()
        {
            Items = Array.Empty<T>();
        }

        /// <summary>
        /// For extensions only.
        /// </summary>
        internal BasePaginate(int index, int size, IList<T> items, int count, int totalPagesCount, int totalItemsCount)
        {
            Index = index;
            Size = size;
            Items = items;
            Count = count;
            TotalPagesCount = totalPagesCount;
            TotalItemsCount = totalItemsCount;
        }

        /// <summary>
        /// Standard Paginate constructor.
        /// </summary>
        internal BasePaginate(IEnumerable<T> source, int size, int index)
        {
            ThrowHelper.ThrowArgumentOutOfRangeExceptionWhenCheckFailed(size, index);

            if (source is IQueryable<T> query)
            {
                TotalItemsCount = query.Count();
                TotalPagesCount = (int) Math.Ceiling(TotalItemsCount / (double) size);

                // Fix page index.
                Index = Math.Min(index, TotalPagesCount - 1);
                Size = size;

                Items = query.Skip(Index * Size).Take(Size).ToList();
                Count = Items.Count;
            }
            else
            {
                // Needs to prevent it from multiple enumeration, but it is dangerous when it pulls all table.
                var enumerable = source as T[] ?? source.ToArray();

                TotalItemsCount = enumerable.Length;
                TotalPagesCount = (int) Math.Ceiling(TotalItemsCount / (double) size);

                // Fix page index.
                Index = Math.Min(index, TotalPagesCount - 1);
                Size = size;

                Items = enumerable.Skip(Index * Size).Take(Size).ToList();
                Count = Items.Count;
            }
        }
    }
}
