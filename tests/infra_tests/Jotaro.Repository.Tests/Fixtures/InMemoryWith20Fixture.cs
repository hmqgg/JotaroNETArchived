using AutoFixture;
using Jotaro.Repository.Tests.Models;
using System;
using System.Collections.Concurrent;

namespace Jotaro.Repository.Tests.Fixtures
{
    public class InMemoryWith20Fixture
    {
        public ConcurrentDictionary<Guid, Employee> Employees { get; }

        public InMemoryWith20Fixture()
        {
            Employees = new ConcurrentDictionary<Guid, Employee>();

            var fixture = new Fixture();
            var data = fixture.CreateMany<Employee>(20);
            foreach (var employee in data)
            {
                Employees.TryAdd(employee.Id, employee);
            }
        }
    }
}
