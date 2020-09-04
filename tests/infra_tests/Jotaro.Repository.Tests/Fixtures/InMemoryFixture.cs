using Jotaro.Repository.Tests.Models;
using System;
using System.Collections.Concurrent;

namespace Jotaro.Repository.Tests.Fixtures
{
    public class InMemoryFixture
    {
        public ConcurrentDictionary<Guid, Employee> Employees { get; }

        public InMemoryFixture()
        {
            Employees = new ConcurrentDictionary<Guid, Employee>();
        }
    }
}
