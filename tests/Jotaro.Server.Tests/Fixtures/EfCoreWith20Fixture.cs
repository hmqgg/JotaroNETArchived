using Jotaro.Server.Tests.Models;
using System;
using System.Threading.Tasks;

namespace Jotaro.Server.Tests.Fixtures
{
    public class EfCoreWith20Fixture : IDisposable, IAsyncDisposable
    {
        public ServerTestContext Context { get; }

        public EfCoreWith20Fixture()
        {
            Context = new ServerTestContext();
            Context.Database.EnsureCreated();
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
