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
    public class RemoveRepositoryTests : IClassFixture<InMemoryWith20Fixture>
    {
        private readonly InMemoryWith20Fixture fixture;
        private readonly IRemoveRepository<Employee> removeRepo;

        public RemoveRepositoryTests(InMemoryWith20Fixture fixture)
        {
            this.fixture = fixture;
            removeRepo = new InMemoryRepository<Employee, Guid>(fixture.Employees);
        }

        [Fact]
        public async Task AfterRemoveNever_CountShouldReturnSame()
        {
            var number = await removeRepo.RemoveByAsync(x => false);

            Assert.Equal(0, number);
        }

        [Fact]
        public async Task AfterRemove_CountShouldReturnRemoved()
        {
            var expected = fixture.Employees.Values.Count(e => e.IsDeleted);

            var number = await removeRepo.RemoveByAsync(e => e.IsDeleted);

            Assert.Equal(expected, number);
            Assert.Equal(20 - expected, fixture.Employees.Count);
        }
    }
}
