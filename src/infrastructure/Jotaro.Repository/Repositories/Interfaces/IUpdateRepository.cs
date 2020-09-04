using Jotaro.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Jotaro.Repository.Repositories.Interfaces
{
    public interface IUpdateRepository<T, in TId> : IUpdateByRepository<T> where T : class, IHasId<TId>
    {
        Task UpdateAsync(TId id, Action<T> action, CancellationToken cancellationToken = default);

        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

        ValueTask<int> UpdateAsync(params T[] entities);

        ValueTask<int> UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    }
}
