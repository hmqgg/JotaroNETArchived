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
    public class QueryRepositoryTests
    {
        private readonly InMemoryWith20Fixture fixture;
        private readonly IQueryRepository<Employee> queryRepo;

        public QueryRepositoryTests(InMemoryWith20Fixture fixture)
        {
            this.fixture = fixture;
            queryRepo = new InMemoryRepository<Employee, Guid>(fixture.Employees);
        }

        [Fact]
        public async Task Query_AnyShouldReturnTrue()
        {
            var actual = await queryRepo.AnyAsync();

            Assert.True(actual);
        }

        [Fact]
        public async Task QueryCount_CountShouldReturnEqual()
        {
            var actual = await queryRepo.CountAsync();

            Assert.Equal(fixture.Employees.Count, actual);
        }

        [Fact]
        public async Task QueryFirstOrDefault_ItemShouldReturnNotNull()
        {
            var item = await queryRepo.FirstOrDefaultAsync();

            Assert.NotNull(item);
        }

        [Fact]
        public async Task QueryFirstOrDefaultNever_ItemShouldReturnNull()
        {
            // Never happen.
            var item = await queryRepo.FirstOrDefaultAsync(x => x.IsDeleted && !x.IsDeleted);

            Assert.Null(item);
        }

        [Fact]
        public void QueryFindBy_ShouldQueryable()
        {
            // Maybe empty.
            var queryable = queryRepo.FindBy(x => x.IsDeleted);

            Assert.NotNull(queryable);
            Assert.All(queryable, e => Assert.True(e.IsDeleted));
        }

        [Fact]
        public async Task QueryFindBy_CountShouldReturnEqual()
        {
            // All
            var all = await queryRepo.FindByAsync();

            Assert.Equal(20, all.Count);
        }

        [Fact]
        public async Task QueryFindByAlways_ShouldReturnNotEmpty()
        {
            // Predicate all.
            var queryable = await queryRepo.FindByAsync(x => true, q => q.OrderBy(e => e.Age));

            Assert.NotEmpty(queryable);
        }

        [Fact]
        public async Task QueryFindByNever_ShouldReturnEmpty()
        {
            // Never happen.
            var queryable = await queryRepo.FindByAsync(x => x.IsDeleted && !x.IsDeleted);

            Assert.Empty(queryable);
        }

        [Theory, AutoData]
        public async Task QueryGetPage_CountShouldReturnEqual([Range(1, 20)] int size)
        {
            var page = await queryRepo.GetPageAsync(x => true,
                q => q.OrderBy(e => e.Age),
                size: size,
                index: 0);

            // First page.
            Assert.False(page.HasPrevious);
            Assert.Equal(size, page.Size);
            Assert.Equal(size, page.Count);
        }

        [Theory, AutoData]
        public async Task QueryGetPageOverHalf_CountShouldReturnNotEqual([Range(11, 19)] int size)
        {
            // Second page.
            var page = await queryRepo.GetPageAsync(x => true,
                q => q.OrderBy(e => e.Age),
                size: size,
                index: 1);

            // Last page.
            Assert.False(page.HasNext);
            Assert.Equal(size, page.Size);
            Assert.NotEqual(size, page.Count);
        }

        [Theory, AutoData]
        public async Task ConvertGetPage_CountShouldReturnEqual([Range(1, 20)] int size)
        {
            // First page, take from 0.
            var expected = JsonSerializer.Serialize(fixture.Employees.Values.OrderBy(e => e.Age).Take(size));
            var page = await queryRepo.GetPageAsync(x => new Employee
                {
                    Age = x.Age,
                    Id = x.Id,
                    IsDeleted = x.IsDeleted,
                    Name = x.Name,
                    QNumber =  x.QNumber
                }, 
                x => true,
                q => q.OrderBy(e => e.Age),
                size: size,
                index: 0);

            var actual = JsonSerializer.Serialize(page.Items);

            // First page.
            Assert.False(page.HasPrevious);
            Assert.Equal(size, page.Size);
            Assert.Equal(size, page.Count);

            Assert.Equal(expected, actual);
        }
    }
}
