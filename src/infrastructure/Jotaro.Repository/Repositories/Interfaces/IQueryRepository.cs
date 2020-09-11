using Jotaro.Repository.Paginate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Jotaro.Repository.Repositories.Interfaces
{
    public interface IQueryRepository<T> where T : class
    {
        Task<bool> AnyAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default);

        ValueTask<int> CountAsync(Expression<Func<T, bool>>? predicate = null,
            CancellationToken cancellationToken = default);

        IQueryable<T> FindBy(Expression<Func<T, bool>>? predicate = null);

        Task<List<T>> FindByAsync(Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            CancellationToken cancellationToken = default);

        ValueTask<T?> FirstOrDefaultAsync(Expression<Func<T, bool>>? predicate = null,
            CancellationToken cancellationToken = default);

        Task<IPaginate<T>> GetPageAsync(Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            int size = 50,
            int index = 0,
            CancellationToken cancellationToken = default);

        Task<IPaginate<TResult>> GetPageAsync<TResult>(Func<T, TResult> conversion,
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            int size = 50,
            int index = 0,
            CancellationToken cancellationToken = default) where TResult : class;
    }
}
