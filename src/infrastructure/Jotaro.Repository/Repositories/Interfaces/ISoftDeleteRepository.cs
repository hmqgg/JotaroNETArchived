using Jotaro.Entity.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Jotaro.Repository.Repositories.Interfaces
{
    public interface ISoftDeleteRepository<in T> where T : IHasSoftDelete
    {
        Task SoftDeleteAsync(T entity, CancellationToken cancellationToken = default);

        ValueTask<int> SoftDeleteAsync(params T[] entities);

        ValueTask<int> SoftDeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    }
}
