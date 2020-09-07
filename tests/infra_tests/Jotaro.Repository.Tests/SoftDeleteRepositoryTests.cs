using Jotaro.Repository.Repositories.InMemory;
using Jotaro.Repository.Repositories.Interfaces;
using Jotaro.Repository.Tests.Fixtures;
using Jotaro.Repository.Tests.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Jotaro.Repository.Tests
{
    [Collection(Constants.InMemoryEmployee20Test)]
    public class SoftDeleteRepositoryTests
    {
        private readonly InMemoryWith20Fixture fixture;
        private readonly ISoftDeleteRepository<Employee> softRepo;

        public SoftDeleteRepositoryTests(InMemoryWith20Fixture fixture)
        {
            this.fixture = fixture;
            softRepo = new InMemoryRepository<Employee, Guid>(fixture.Employees);
        }

        [Fact]
        public async Task AfterRemoveNever_CountShouldReturnSame()
        {
            await softRepo.SoftDeleteAsync(new Employee());

            Assert.Equal(20, fixture.Employees.Count);
        }

        [Fact]
        public async Task AfterRemoveOne_CountShouldReturnSame()
        {
            await softRepo.SoftDeleteAsync(fixture.Employees.Values.FirstOrDefault());

            Assert.Equal(20, fixture.Employees.Count);
        }

        [Fact]
        public async Task AfterRemoveOne_ShouldReturnTrue()
        {
            var entity = fixture.Employees.Values.FirstOrDefault(e => !e.IsDeleted);

            await softRepo.SoftDeleteAsync(entity);

            // To ensure tests work as intended.
            Assert.True(entity?.IsDeleted ?? true);
        }

        [Fact]
        public async Task AfterRemoveParams_ShouldReturnTrue()
        {
            var entity1 = fixture.Employees.Values.FirstOrDefault(e => !e.IsDeleted);
            var entity2 = fixture.Employees.Values.LastOrDefault(e => !e.IsDeleted);

            await softRepo.SoftDeleteAsync(entity1, entity2);

            // To ensure tests work as intended.
            Assert.True(entity1?.IsDeleted ?? true);
            Assert.True(entity2?.IsDeleted ?? true);
        }

        [Fact]
        public async Task AfterRemoveAll_ShouldReturnTrue()
        {
            var entities = fixture.Employees.Values;

            await softRepo.SoftDeleteRangeAsync(entities);

            Assert.All(fixture.Employees.Values, e => Assert.True(e.IsDeleted));
        }
    }
}
