using System.Collections.Generic;

namespace Jotaro.Repository.Paginate
{
    public interface IPaginate<T>
    {
        /// <summary>
        /// Page index, from 0.
        /// </summary>
        int Index { get; }

        /// <summary>
        /// Page size.
        /// </summary>
        int Size { get; }

        /// <summary>
        /// Actual items.
        /// </summary>
        IList<T> Items { get; }

        /// <summary>
        /// Actual items count.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// All pages count.
        /// </summary>
        int TotalPagesCount { get; }

        /// <summary>
        /// All items count.
        /// </summary>
        int TotalItemsCount { get; }

        /// <summary>
        /// Has previous page.
        /// </summary>
        bool HasPrevious => Index > 0;

        /// <summary>
        /// Has next page.
        /// </summary>
        bool HasNext => Index + 1 < TotalPagesCount;
    }
}
