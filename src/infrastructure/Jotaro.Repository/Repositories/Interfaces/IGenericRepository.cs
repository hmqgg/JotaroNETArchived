using Jotaro.Entity.Interfaces;

namespace Jotaro.Repository.Repositories.Interfaces
{
    public interface IGenericRepository<T, in TId> : ICreateRepository<T>,
        IReadRepository<T, TId>,
        IUpdateRepository<T, TId>,
        IDeleteRepository<T, TId>,
        IKeylessRepository<T>
        where T : class, IHasId<TId>
    {
    }
}
