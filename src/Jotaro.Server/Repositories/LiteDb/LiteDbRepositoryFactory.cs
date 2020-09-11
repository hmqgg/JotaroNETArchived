using Jotaro.Entity.Interfaces;
using Jotaro.Repository.Repositories.Interfaces;
using LiteDB;

namespace Jotaro.Server.Repositories.LiteDb
{
    public class LiteDbRepositoryFactory : IRepositoryFactory
    {
        // Dispose by Dependency Injection.
        private readonly ILiteDatabase database;

        public LiteDbRepositoryFactory(ILiteDatabase database)
        {
            this.database = database;
        }

        public IGenericRepository<T, TId> Repository<T, TId>() where T : class, IHasId<TId>
        {
            return new LiteDbGenericRepository<T, TId>(database.GetCollection<T>());
        }

        public IKeylessRepository<T> Repository<T>() where T : class
        {
            return new LiteDbKeylessRepository<T>(database.GetCollection<T>());
        }
    }
}
