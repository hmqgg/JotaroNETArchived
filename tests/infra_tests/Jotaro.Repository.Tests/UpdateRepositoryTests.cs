using AutoFixture.Xunit2;
using Jotaro.Repository.Repositories.InMemory;
using Jotaro.Repository.Repositories.Interfaces;
using Jotaro.Repository.Tests.Fixtures;
using Jotaro.Repository.Tests.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Jotaro.Repository.Tests
{
    public class UpdateRepositoryTests : IClassFixture<InMemoryWith20Fixture>
    {
        private readonly InMemoryWith20Fixture fixture;
        private readonly IUpdateRepository<Employee, Guid> updateRepo;

        public UpdateRepositoryTests(InMemoryWith20Fixture fixture)
        {
            this.fixture = fixture;
            updateRepo = new InMemoryRepository<Employee, Guid>(fixture.Employees);
        }

        [Fact]
        public async Task AfterUpdateEmptyId_AllShouldReturnNotEqual()
        {
            var expected = Guid.NewGuid().ToString();

            await updateRepo.UpdateAsync(Guid.Empty, e => new Employee
            {
                Id = e.Id,
                Age = e.Age,
                IsDeleted = e.IsDeleted,
                Name = e.Name,
                QNumber = e.QNumber
            });

            Assert.All(fixture.Employees.Values, e => Assert.NotEqual(expected, e.Name));
        }

        [Theory, AutoData]
        public async Task AfterUpdateOne_AllShouldReturnNotEqual([Range(0, 19)] int index)
        {
            var entity = fixture.Employees.Values.ToList()[index];
            var expected = Guid.NewGuid().ToString();

            await updateRepo.UpdateAsync(entity.Id, e => new Employee
            {
                Id = e.Id,
                Age = e.Age,
                IsDeleted = e.IsDeleted,
                Name = expected,
                QNumber = e.QNumber
            });

            Assert.Contains(fixture.Employees.Values, e => e.Name == expected);
        }

        [Theory, AutoData]
        public async Task AfterUpdateNotExisting_ListShouldReturnNotContainsKey(Employee tester)
        {
            // Not existing.
            await updateRepo.UpdateAsync(tester);

            Assert.DoesNotContain(fixture.Employees.Keys.ToList(), x => x == tester.Id);
        }

        [Theory, AutoData]
        public async Task AfterUpdateOne_ValueShouldReturnEqual([Range(0, 19)] int index)
        {
            var entity = fixture.Employees.Values.ToList()[index];
            var expected = Guid.NewGuid().ToString();

            entity.Name = expected;
            await updateRepo.UpdateAsync(entity);
            var actual = fixture.Employees[entity.Id];

            Assert.Equal(expected, actual.Name);
        }

        [Theory, AutoData]
        public async Task AfterUpdateParams_ValueShouldReturnEqual([Range(0, 19)] int index1, [Range(0, 19)] int index2)
        {
            var values = fixture.Employees.Values.ToList();
            var entity1 = values[index1];
            var entity2 = values[index2];
            var expected = Guid.NewGuid().ToString();

            entity1.Name = expected;
            entity2.Name = expected;
            var updated = await updateRepo.UpdateRangeAsync(entity1, entity2);
            var actual1 = fixture.Employees[entity1.Id];
            var actual2 = fixture.Employees[entity2.Id];

            Assert.Equal(2, updated);
            Assert.Equal(expected, actual1.Name);
            Assert.Equal(expected, actual2.Name);
        }

        [Theory, AutoData]
        public async Task AfterUpdateRange_ValueShouldReturnEqual([Range(0, 19)] int index)
        {
            var values = fixture.Employees.Values.ToList();
            var range = values.GetRange(index, 20 - index);
            var expected = Guid.NewGuid().ToString();

            range.ForEach(e => e.Name = expected);
            var updated = await updateRepo.UpdateRangeAsync(range);

            Assert.Equal(range.Count, updated);
            Assert.Equal(range.Count, fixture.Employees.Values.Count(e => e.Name == expected));
        }
    }
}
