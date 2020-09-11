using Jotaro.Entity.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Jotaro.Repository.Repositories.Interfaces
{
    public interface IUpdateRepository<T, in TId> : IUpdateByRepository<T> where T : class, IHasId<TId>
    {
        Task UpdateAsync(TId id, Expression<Func<T, T>> extend, CancellationToken cancellationToken = default);

        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

        ValueTask<int> UpdateRangeAsync(params T[] entities);

        ValueTask<int> UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    }
}
