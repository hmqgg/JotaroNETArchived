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
            var updated = await updateRepo.UpdateByAsync(e => new Employee
            {
                Id = e.Id,
                Age = e.Age,
                IsDeleted = e.IsDeleted,
                Name = e.Name,
                QNumber = e.QNumber
            }, x => x.IsDeleted && !x.IsDeleted);

            Assert.Equal(0, updated);
            Assert.All(fixture.Employees.Values, e => Assert.NotEqual(Guid.Empty, e.Id));
        }

        [Theory, AutoData]
        public async Task AfterUpdateAllByAction_CountShouldReturnEqual([Range(0, 100)] int age)
        {
            var count = fixture.Employees.Values.Count(e => e.Age != age);
            var updated = await updateRepo.UpdateByAsync(e => e.Age = age, x => x.Age != age);

            var actual = fixture.Employees.Values.Count(e => e.Age == age);

            Assert.Equal(count, updated);
            Assert.Equal(20, actual);
        }

        [Fact]
        public async Task AfterUpdateAllByExpression_CountShouldReturnEqual()
        {
            var count = fixture.Employees.Values.Count(e => e.IsDeleted);
            var updated = await updateRepo.UpdateByAsync(e => new Employee
            {
                Id = e.Id,
                Age = e.Age,
                IsDeleted = false,
                Name = e.Name,
                QNumber = e.QNumber
            }, x => x.IsDeleted);

            var flipCount = fixture.Employees.Values.Count(e => !e.IsDeleted);

            Assert.Equal(count, updated);
            Assert.Equal(20, flipCount);
        }
    }
}
