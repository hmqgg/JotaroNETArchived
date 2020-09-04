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
        IQueryable<T> FindBy(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default);

        IQueryable<T> FindAll();

        Task<IList<T>> FindByAsync(Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            CancellationToken cancellationToken = default);

        Task<IList<T>> FindAllAsync(CancellationToken cancellationToken = default);

        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default);

        IPaginate<T> GetPageAsync(Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            int size = 50,
            int index = 0,
            CancellationToken cancellationToken = default);

        IPaginate<TResult> GetPageAsync<TResult>(Expression<Func<T, TResult>> conversion,
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            int size = 50,
            int index = 0,
            CancellationToken cancellationToken = default) where TResult : class;
    }
}
