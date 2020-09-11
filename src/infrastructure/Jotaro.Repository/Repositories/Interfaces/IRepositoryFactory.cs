using Jotaro.Entity.Interfaces;

namespace Jotaro.Repository.Repositories.Interfaces
{
    public interface IRepositoryFactory
    {
        IGenericRepository<T, TId> Repository<T, TId>() where T : class, IHasId<TId>;

        IKeylessRepository<T> Repository<T>() where T : class;
    }
}
