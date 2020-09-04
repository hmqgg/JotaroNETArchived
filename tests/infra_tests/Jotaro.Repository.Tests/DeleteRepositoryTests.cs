using AutoFixture.Xunit2;
using Jotaro.Repository.Repositories.InMemory;
using Jotaro.Repository.Repositories.Interfaces;
using Jotaro.Repository.Tests.Fixtures;
using Jotaro.Repository.Tests.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Jotaro.Repository.Tests
{
    public class DeleteRepositoryTests : IClassFixture<InMemoryWith20Fixture>
    {
        private readonly InMemoryWith20Fixture fixture;
        private readonly IDeleteRepository<Employee, Guid> deleteRepo;

        public DeleteRepositoryTests(InMemoryWith20Fixture fixture)
        {
            this.fixture = fixture;
            deleteRepo = new InMemoryRepository<Employee, Guid>(fixture.Employees);
        }

        [Fact]
        public async Task AfterDeleteEmptyId_CountShouldReturnSame()
        {
            var expected = fixture.Employees.Count;

            await deleteRepo.DeleteAsync(Guid.Empty);
            var actual = fixture.Employees.Count;

            Assert.Equal(expected, actual);
        }

        [Theory, AutoData]
        public async Task AfterDeleteOne_CountShouldReturnLess([Range(0, 19)] int index)
        {
            var id = fixture.Employees.Keys.ToList()[index];
            var expected = fixture.Employees.Count - 1;

            await deleteRepo.DeleteAsync(id);
            var actual = fixture.Employees.Count;

            Assert.Equal(expected, actual);
        }

        [Theory, AutoData]
        public async Task AfterDeleteIdParams_CountShouldReturnLess([Range(0, 10)] int index1, [Range(0, 10)] int index2)
        {
            var values = fixture.Employees.Keys.ToList();
            var entity1 = values[index1];
            var entity2 = values[index2];
            var expected = index1 == index2 ? 1 : 2;

            var deleted = await deleteRepo.DeleteAsync(entity1, entity2);
            var actual = fixture.Employees.Count;

            Assert.Equal(expected, deleted);
            Assert.Equal(expected, values.Count - actual);
        }

        [Theory, AutoData]
        public async Task AfterDeleteParams_CountShouldReturnLess([Range(0, 10)] int index1, [Range(0, 10)] int index2)
        {
            var values = fixture.Employees.Values.ToList();
            var entity1 = values[index1];
            var entity2 = values[index2];
            var expected = index1 == index2 ? 1 : 2;

            var deleted = await deleteRepo.DeleteAsync(entity1, entity2);
            var actual = fixture.Employees.Count;

            Assert.Equal(expected, deleted);
            Assert.Equal(expected, values.Count - actual);
        }

        [Theory, AutoData]
        public async Task AfterDeleteRangeAll_CountShouldReturnZero(IEnumerable<Employee> testers)
        {
            var conDict = new ConcurrentDictionary<Guid, Employee>();
            var testerList = testers.ToList();
            var expected = testerList.Count;
            testerList.ForEach(e => conDict.GetOrAdd(e.Id, e));
            IDeleteRepository<Employee, Guid> repository = new InMemoryRepository<Employee, Guid>(conDict);

            var deleted = await repository.DeleteRangeAsync(testerList);

            Assert.Equal(expected, deleted);
        }

        [Theory, AutoData]
        public async Task AfterDeleteRangeAllId_CountShouldReturnZero(IEnumerable<Employee> testers)
        {
            var conDict = new ConcurrentDictionary<Guid, Employee>();
            var testerList = testers.ToList();
            var expected = testerList.Count;
            testerList.ForEach(e => conDict.GetOrAdd(e.Id, e));
            IDeleteRepository<Employee, Guid> repository = new InMemoryRepository<Employee, Guid>(conDict);

            var deleted = await repository.DeleteRangeAsync(testerList.Select(e => e.Id));

            Assert.Equal(expected, deleted);
        }
    }
}
