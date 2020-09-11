using Jotaro.Server.Repositories.EfCore;
using Jotaro.Server.Tests.Fixtures;
using Xunit;

namespace Jotaro.Server.Tests.Repositories.Tests
{
    public class EfCoreRepositoryTests : RepositoryTests, IClassFixture<EfCoreWith20Fixture>
    {
        public EfCoreRepositoryTests(EfCoreWith20Fixture fixture) : base(new EfCoreRepositoryFactory(fixture.Context))
        {
        }
    }
}
