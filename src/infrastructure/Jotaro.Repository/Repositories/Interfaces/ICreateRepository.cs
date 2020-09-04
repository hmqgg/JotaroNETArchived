using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Jotaro.Repository.Repositories.Interfaces
{
    public interface ICreateRepository<in T> where T : class
    {
        Task InsertAsync(T entity, CancellationToken cancellationToken = default);

        ValueTask<int> InsertAsync(params T[] entities);

        ValueTask<int> InsertRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    }
}
