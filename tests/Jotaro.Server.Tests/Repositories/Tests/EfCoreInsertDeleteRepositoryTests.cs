using Jotaro.Server.Repositories.EfCore;
using Jotaro.Server.Tests.Fixtures;
using Xunit;

namespace Jotaro.Server.Tests.Repositories.Tests
{
    public class EfCoreInsertDeleteRepositoryTests : InsertDeleteRepositoryTests, IClassFixture<EfCoreWith20Fixture>
    {
        public EfCoreInsertDeleteRepositoryTests(EfCoreWith20Fixture fixture) : base(
            new EfCoreRepositoryFactory(fixture.Context))
        {
        }
    }
}
