using Jotaro.Entity.Interfaces;
using Jotaro.Repository.Repositories.Interfaces;
using MongoDB.Driver;

namespace Jotaro.Server.Repositories.Mongo
{
    public class MongoRepositoryFactory : IRepositoryFactory
    {
        private readonly IMongoDatabase database;

        public MongoRepositoryFactory(IMongoDatabase database)
        {
            this.database = database;
        }

        public IGenericRepository<T, TId> Repository<T, TId>() where T : class, IHasId<TId>
        {
            return new MongoGenericRepository<T, TId>(database.GetCollection<T>(typeof(T).Name));
        }

        public IKeylessRepository<T> Repository<T>() where T : class
        {
            return new MongoKeylessRepository<T>(database.GetCollection<T>(typeof(T).Name));
        }
    }
}
