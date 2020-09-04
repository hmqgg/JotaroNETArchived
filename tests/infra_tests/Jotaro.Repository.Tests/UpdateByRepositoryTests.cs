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
    public class UpdateByRepositoryTests : IClassFixture<InMemoryWith20Fixture>
    {
        private readonly InMemoryWith20Fixture fixture;
        private readonly IUpdateByRepository<Employee> updateRepo;

        public UpdateByRepositoryTests(InMemoryWith20Fixture fixture)
        {
            this.fixture = fixture;
            updateRepo = new InMemoryRepository<Employee, Guid>(fixture.Employees);
        }

        [Fact]
        public async Task AfterUpdateNever_CountShouldReturnZero()
        {
            var updated = await updateRepo.UpdateByAsync(e => e.Id = Guid.Empty, x => x.IsDeleted && !x.IsDeleted);

            Assert.Equal(0, updated);
            Assert.All(fixture.Employees.Values, e => Assert.NotEqual(Guid.Empty, e.Id));
        }

        [Fact]
        public async Task AfterUpdateAll_CountShouldReturnEqual()
        {
            var count = fixture.Employees.Values.Count(e => e.IsDeleted);
            var updated = await updateRepo.UpdateByAsync(e => e.IsDeleted = !e.IsDeleted, x => x.IsDeleted);

            var flipCount = fixture.Employees.Values.Count(e => !e.IsDeleted);

            Assert.Equal(count, updated);
            Assert.Equal(20, flipCount);
        }
    }
}
