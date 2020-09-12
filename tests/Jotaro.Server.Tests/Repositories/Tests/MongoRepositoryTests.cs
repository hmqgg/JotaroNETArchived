using Jotaro.Server.Repositories.Mongo;
using Jotaro.Server.Tests.Fixtures;
using Xunit;

namespace Jotaro.Server.Tests.Repositories.Tests
{
    public class MongoRepositoryTests : RepositoryTests, IClassFixture<MongoWith20Fixture>
    {
        public MongoRepositoryTests(MongoWith20Fixture fixture) : base(new MongoRepositoryFactory(fixture.Database))
        {
        }
    }
}
