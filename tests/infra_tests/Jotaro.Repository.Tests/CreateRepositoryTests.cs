using AutoFixture.Xunit2;
using Jotaro.Repository.Repositories.InMemory;
using Jotaro.Repository.Repositories.Interfaces;
using Jotaro.Repository.Tests.Fixtures;
using Jotaro.Repository.Tests.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Jotaro.Repository.Tests
{
    public class CreateRepositoryTests : IClassFixture<InMemoryFixture>
    {
        private readonly InMemoryFixture fixture;

        public CreateRepositoryTests(InMemoryFixture fixture)
        {
            this.fixture = fixture;
        }

        [Theory, AutoData]
        public async Task WithEmptyAfterInserting_CountShouldReturnOne(Employee tester)
        {
            var conDict = new ConcurrentDictionary<Guid, Employee>();
            ICreateRepository<Employee> repository = new InMemoryRepository<Employee, Guid>(conDict);

            await repository.InsertAsync(tester);

            var count = conDict.Count;

            Assert.Equal(1, count);
        }

        [Theory, AutoData]
        public async Task WithEmptyAfterInsertingMany_CountShouldReturnEqual(IEnumerable<Employee> testers)
        {
            var conDict = new ConcurrentDictionary<Guid, Employee>();
            ICreateRepository<Employee> repository = new InMemoryRepository<Employee, Guid>(conDict);
            var testerList = testers.ToList();

            var inserted = await repository.InsertRangeAsync(testerList);

            Assert.Equal(testerList.Count, inserted);
        }

        [Theory, AutoData]
        public async Task AfterInsertingParams_CountShouldReturnTwo(Employee tester1, Employee tester2)
        {
            ICreateRepository<Employee> repository = new InMemoryRepository<Employee, Guid>(fixture.Employees);

            var number = await repository.InsertAsync(tester1, tester2);

            Assert.Equal(2, number);
        }

        [Theory, AutoData]
        public async Task AfterInserting_ObjectShouldReturnEqual(Employee tester)
        {
            ICreateRepository<Employee> repository = new InMemoryRepository<Employee, Guid>(fixture.Employees);
            var expected = JsonSerializer.Serialize(tester);

            await repository.InsertAsync(tester);

            var actual = JsonSerializer.Serialize(fixture.Employees[tester.Id]);

            Assert.Equal(expected, actual);
        }
    }
}
