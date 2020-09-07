using AutoFixture;
using Jotaro.Repository.Tests.Models;
using System;
using System.Threading.Tasks;

namespace Jotaro.Repository.Tests.Fixtures
{
    public class EfCoreInMemoryWith20Fixture : IDisposable, IAsyncDisposable
    {
        public TestContext Context { get; }

        public EfCoreInMemoryWith20Fixture()
        {
            Context = new TestContext();

            var fixture = new Fixture();
            var data = fixture.CreateMany<Employee>(20);
            Context.Employees.AddRange(data);
            Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Dispose();
        }

        public ValueTask DisposeAsync()
        {
            return Context.DisposeAsync();
        }
    }
}
