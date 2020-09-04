using Jotaro.Repository.Tests.Fixtures;
using Xunit;

namespace Jotaro.Repository.Tests.Collections
{
    [CollectionDefinition(Constants.InMemoryEmployee20Test)]
    public class Employee20Collection : ICollectionFixture<InMemoryWith20Fixture>
    {
    }
}
