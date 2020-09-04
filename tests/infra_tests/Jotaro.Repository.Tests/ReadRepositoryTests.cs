using AutoFixture.Xunit2;
using Jotaro.Repository.Repositories.InMemory;
using Jotaro.Repository.Repositories.Interfaces;
using Jotaro.Repository.Tests.Fixtures;
using Jotaro.Repository.Tests.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Jotaro.Repository.Tests
{
    [Collection(Constants.InMemoryEmployee20Test)]
    public class ReadRepositoryTests
    {
        private readonly InMemoryWith20Fixture fixture;
        private readonly IReadRepository<Employee, Guid> readRepo;

        public ReadRepositoryTests(InMemoryWith20Fixture fixture)
        {
            this.fixture = fixture;
            readRepo = new InMemoryRepository<Employee, Guid>(fixture.Employees);
        }

        [Fact]
        public void ReadFindEmpty_ResultShouldReturnNull()
        {
            var actual = readRepo.Find(Guid.Empty);

            Assert.Null(actual);
        }

        [Theory, AutoData]
        public void ReadFind_ItemShouldReturnEqual([Range(0, 19)] int index)
        {
            var employee = fixture.Employees.Values.ToList()[index];
            var expected = JsonSerializer.Serialize(employee);

            var result = readRepo.Find(employee.Id);
            var actual = JsonSerializer.Serialize(result);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task ReadFindEmpty_ItemShouldReturnNull()
        {
            var actual = await readRepo.FindAsync(Guid.Empty);

            Assert.Null(actual);
        }

        [Theory, AutoData]
        public async Task ReadFindRandom_ItemShouldReturnEqual([Range(0, 19)] int index)
        {
            var employee = fixture.Employees.Values.ToList()[index];
            var expected = JsonSerializer.Serialize(employee);

            var result = await readRepo.FindAsync(employee.Id);
            var actual = JsonSerializer.Serialize(result);

            Assert.Equal(expected, actual);
        }
    }
}
