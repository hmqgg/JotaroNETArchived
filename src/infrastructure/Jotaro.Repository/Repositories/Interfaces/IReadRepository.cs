using Jotaro.Repository.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Jotaro.Repository.Repositories.Interfaces
{
    public interface IReadRepository<T, in TId> : IQueryRepository<T> where T : class, IHasId<TId>
    {
        T? Find(TId id);

        ValueTask<T?> FindAsync(TId id, CancellationToken cancellationToken = default);
    }
}
