using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Jotaro.Repository.Repositories.Interfaces
{
    public interface IUpdateByRepository<T> where T : class
    {
        ValueTask<int> UpdateByAsync(Action<T> action,
            Expression<Func<T, bool>>? predicate = null,
            CancellationToken cancellationToken = default);

        ValueTask<int> UpdateByAsync(Expression<Func<T, T>> extend,
            Expression<Func<T, bool>>? predicate = null,
            CancellationToken cancellationToken = default);
    }
}
