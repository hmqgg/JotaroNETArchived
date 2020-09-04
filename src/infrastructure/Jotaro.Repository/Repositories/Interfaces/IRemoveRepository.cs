using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Jotaro.Repository.Repositories.Interfaces
{
    public interface IRemoveRepository<T> where T : class
    {
        ValueTask<int> RemoveByAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default);
    }
}
