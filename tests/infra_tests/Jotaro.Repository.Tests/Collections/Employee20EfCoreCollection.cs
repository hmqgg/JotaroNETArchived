using Jotaro.Repository.Tests.Fixtures;
using Xunit;

namespace Jotaro.Repository.Tests.Collections
{
    [CollectionDefinition(Constants.EfCoreInMemoryEmployee20Test)]
    public class Employee20EfCoreCollection : ICollectionFixture<EfCoreInMemoryWith20Fixture>
    {
        
    }
}
