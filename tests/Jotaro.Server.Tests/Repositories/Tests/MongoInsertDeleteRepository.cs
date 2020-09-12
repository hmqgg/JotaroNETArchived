using Jotaro.Server.Repositories.Mongo;
using Jotaro.Server.Tests.Fixtures;
using Xunit;

namespace Jotaro.Server.Tests.Repositories.Tests
{
    public class MongoInsertDeleteRepository : InsertDeleteRepositoryTests, IClassFixture<MongoWith20Fixture>
    {
        public MongoInsertDeleteRepository(MongoWith20Fixture fixture) : base(new MongoRepositoryFactory(fixture.Database))
        {
        }
    }
}
