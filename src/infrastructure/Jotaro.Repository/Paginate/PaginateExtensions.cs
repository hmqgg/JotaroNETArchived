using System;
using System.Collections.Generic;

namespace Jotaro.Repository.Paginate
{
    /// <summary>
    /// Expose Paginate creation.
    /// </summary>
    public static class PaginateExtensions
    {
        /// <summary>
        /// Create Paginate.
        /// </summary>
        public static IPaginate<T> ToPaginate<T>(this IEnumerable<T> source, int size, int index = 0) =>
            new BasePaginate<T>(source, size, index);

        public static IPaginate<TResult> ToPaginate<TEntity, TResult>(this IEnumerable<TEntity> source,
            Func<TEntity, TResult> conversion, int size, int index = 0) =>
            new ConversionPaginate<TEntity, TResult>(source, conversion, size, index);

        public static IPaginate<T> CreateEmptyPaginate<T>() => new BasePaginate<T>();

        public static IPaginate<TResult> CreateEmptyPaginate<TEntity, TResult>() => new ConversionPaginate<TEntity, TResult>();
    }
}
