using Jotaro.Entity.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Jotaro.Repository.Repositories.Interfaces
{
    public interface IDeleteRepository<T, in TId> : IRemoveRepository<T> where T : class, IHasId<TId>
    {
        Task DeleteAsync(TId id, CancellationToken cancellationToken = default);

        ValueTask<int> DeleteRangeAsync(params TId[] ids);

        ValueTask<int> DeleteRangeAsync(IEnumerable<TId> ids, CancellationToken cancellationToken = default);

        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);

        ValueTask<int> DeleteRangeAsync(params T[] entities);

        ValueTask<int> DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    }
}
