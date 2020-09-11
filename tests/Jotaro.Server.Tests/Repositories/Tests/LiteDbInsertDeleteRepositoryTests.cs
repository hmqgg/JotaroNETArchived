using Jotaro.Server.Repositories.LiteDb;
using Jotaro.Server.Tests.Fixtures;
using Xunit;

namespace Jotaro.Server.Tests.Repositories.Tests
{
    public class LiteDbInsertDeleteRepositoryTests : InsertDeleteRepositoryTests, IClassFixture<LiteDbWith20Fixture>
    {
        public LiteDbInsertDeleteRepositoryTests(LiteDbWith20Fixture fixture) : base(
            new LiteDbRepositoryFactory(fixture.Database))
        {
        }
    }
}
