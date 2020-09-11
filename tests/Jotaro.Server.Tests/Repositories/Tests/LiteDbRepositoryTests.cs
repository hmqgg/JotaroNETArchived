using Jotaro.Server.Repositories.LiteDb;
using Jotaro.Server.Tests.Fixtures;
using Xunit;

namespace Jotaro.Server.Tests.Repositories.Tests
{
    public class LiteDbRepositoryTests : RepositoryTests, IClassFixture<LiteDbWith20Fixture>
    {
        public LiteDbRepositoryTests(LiteDbWith20Fixture fixture) : base(new LiteDbRepositoryFactory(fixture.Database))
        {
        }
    }
}
