using Jotaro.Repository.Tests.Fixtures;
using Xunit;

namespace Jotaro.Repository.Tests.Collections
{
    [CollectionDefinition(Constants.InMemoryEmployeeEmptyTest)]
    public class EmployeeCollection : ICollectionFixture<InMemoryFixture>
    {
    }
}
