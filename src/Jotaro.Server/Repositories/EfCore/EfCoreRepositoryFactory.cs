using Jotaro.Entity.Interfaces;
using Jotaro.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Jotaro.Server.Repositories.EfCore
{
    public class EfCoreRepositoryFactory : IRepositoryFactory
    {
        // Dispose by Dependency Injection.
        private readonly DbContext context;

        public EfCoreRepositoryFactory(DbContext context)
        {
            this.context = context;
        }

        public IGenericRepository<T, TId> Repository<T, TId>() where T : class, IHasId<TId>
        {
            return new EfCoreGenericRepository<T, TId>(context);
        }

        public IKeylessRepository<T> Repository<T>() where T : class
        {
            return new EfCoreKeylessRepository<T>(context);
        }
    }
}
