using AutoFixture.Xunit2;
using DeepEqual.Syntax;
using Jotaro.Repository.Repositories.Interfaces;
using Jotaro.Repository.Tests.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Jotaro.Server.Tests.Repositories
{
    public abstract class RepositoryTests
    {
        protected readonly IRepositoryFactory factory;

        protected RepositoryTests(IRepositoryFactory factory)
        {
            this.factory = factory;
        }

        [Fact]
        public void WithGeneric_ShouldReturnNotNull()
        {
            var actual = factory.Repository<Employee, Guid>();

            Assert.NotNull(actual);
        }

        [Fact]
        public void WithKeyless_ShouldReturnNotNull()
        {
            var actual = factory.Repository<Employee>();

            Assert.NotNull(actual);
        }

        [Fact]
        public async Task QueryAny_ShouldReturnTrue()
        {
            var repository = factory.Repository<Employee, Guid>();

            var actual = await repository.AnyAsync();

            Assert.True(actual);
        }

        [Fact]
        public async Task QueryAnyWithPredicate_ShouldReturnTrue()
        {
            var repository = factory.Repository<Employee, Guid>();

            var actual = await repository.AnyAsync(x => x.Id != Guid.Empty);

            Assert.True(actual);
        }

        [Fact]
        public async Task QueryCount_ShouldReturnTwenty()
        {
            var repository = factory.Repository<Employee, Guid>();

            var actual = await repository.CountAsync();

            Assert.Equal(20, actual);
        }

        [Fact]
        public async Task QueryCountWithPredicate_ShouldReturnTwenty()
        {
            var repository = factory.Repository<Employee, Guid>();

            var actual = await repository.CountAsync(x => x.Id != Guid.Empty);

            Assert.Equal(20, actual);
        }

        [Fact]
        public void FindBy_ShouldReturnNotEmpty()
        {
            var repository = factory.Repository<Employee, Guid>();

            var actual = repository.FindBy();

            Assert.NotEmpty(actual);
        }

        [Fact]
        public void FindByWithPredicate_ShouldReturnNotEmpty()
        {
            var repository = factory.Repository<Employee, Guid>();

            var actual = repository.FindBy(x => x.Id != Guid.Empty);

            Assert.NotEmpty(actual);
        }

        [Fact]
        public async Task FindByAsync_ShouldReturnNotEmpty()
        {
            var repository = factory.Repository<Employee, Guid>();

            var actual = await repository.FindByAsync();

            Assert.NotEmpty(actual);
        }

        [Fact]
        public async Task FindByWithOrderBy_ShouldReturnSorted()
        {
            var repository = factory.Repository<Employee, Guid>();

            var actual = await repository.FindByAsync(x => x.Age > 0, y => y.OrderBy(z => z.Age));

            Employee last = null;
            var sorted = true;
            foreach (var item in actual)
            {
                if (item.Age < (last?.Age ?? 0))
                {
                    sorted = false;
                    break;
                }

                last = item;
            }

            Assert.NotEmpty(actual);
            Assert.All(actual, e => Assert.True(e.Age > 0));
            Assert.True(sorted);
        }

        [Fact]
        public async Task FindByAsyncWithPredicate_ShouldReturnNotEmpty()
        {
            var repository = factory.Repository<Employee, Guid>();

            var actual = await repository.FindByAsync(x => x.Id != Guid.Empty);

            Assert.NotEmpty(actual);
        }

        [Fact]
        public async Task FirstOrDefaultAsync_ShouldReturnNotNull()
        {
            var repository = factory.Repository<Employee, Guid>();

            var actual = await repository.FirstOrDefaultAsync();

            Assert.NotNull(actual);
        }

        [Fact]
        public async Task FirstOrDefaultAsyncWithPredicate_ShouldReturnNotNull()
        {
            var repository = factory.Repository<Employee, Guid>();

            var actual = await repository.FirstOrDefaultAsync(x => x.Id != Guid.Empty);

            Assert.NotNull(actual);
        }

        [Fact]
        public async Task FirstOrDefaultAsyncWithPredicateNever_ShouldReturnNull()
        {
            var repository = factory.Repository<Employee, Guid>();

            var actual = await repository.FirstOrDefaultAsync(x => x.Id == Guid.Empty);

            Assert.Null(actual);
        }

        [Theory, AutoData]
        public async Task GetPageAsync_ShouldReturnNotEmpty([Range(1, 19)] int size, [Range(0, 1)] int index)
        {
            var repository = factory.Repository<Employee, Guid>();

            var actual = await repository.GetPageAsync(size: size, index: index);

            Assert.NotEmpty(actual.Items);
            Assert.Equal(size, actual.Size);
            Assert.Equal(index, actual.Index);
        }

        [Theory, AutoData]
        public async Task GetPageAsyncWithOrderBy_ShouldReturnSorted([Range(1, 19)] int size, [Range(0, 1)] int index)
        {
            var repository = factory.Repository<Employee, Guid>();

            var actual = await repository.GetPageAsync(orderBy: x => x.OrderBy(y => y.Age), size: size, index: index);

            Employee last = null;
            var sorted = true;
            foreach (var item in actual.Items)
            {
                if (item.Age < (last?.Age ?? 0))
                {
                    sorted = false;
                    break;
                }

                last = item;
            }

            Assert.True(sorted);
        }

        [Theory, AutoData]
        public async Task GetPageAsyncWithConversion_ShouldReturnNotEmpty([Range(1, 19)] int size,
            [Range(0, 1)] int index)
        {
            var repository = factory.Repository<Employee, Guid>();

            var actual = await repository.GetPageAsync(x => new Developer
            {
                Age = x.Age,
                Id = x.Id,
                IsDeleted = x.IsDeleted
            }, size: size, index: index);

            Assert.All(actual.Items, d => Assert.NotEqual(Guid.Empty, d.Id));
            Assert.NotEmpty(actual.Items);
            Assert.Equal(size, actual.Size);
            Assert.Equal(index, actual.Index);
        }

        [Theory, AutoData]
        public async Task GetPageAsyncWithConversionAndOrderBy_ShouldReturnSorted([Range(1, 19)] int size,
            [Range(0, 1)] int index)
        {
            var repository = factory.Repository<Employee, Guid>();

            var actual = await repository.GetPageAsync(e => new Developer
            {
                Age = e.Age,
                Id = e.Id,
                IsDeleted = e.IsDeleted,
                Name = e.Name
            }, orderBy: x => x.OrderBy(y => y.Age), size: size, index: index);

            Employee last = null;
            var sorted = true;
            foreach (var item in actual.Items)
            {
                if (item.Age < (last?.Age ?? 0))
                {
                    sorted = false;
                    break;
                }

                last = item;
            }

            Assert.True(sorted);
        }

        [Theory, AutoData]
        public async Task AfterUpdate_ShouldReturnEqual(string name)
        {
            var repository = factory.Repository<Employee, Guid>();

            var a = await repository.FirstOrDefaultAsync();
            if (a != null)
            {
                a.Name = name;
                await repository.UpdateAsync(a);
            }

            var actual = await repository.FirstOrDefaultAsync();

            Assert.Equal(name, actual?.Name);
        }

        [Theory, AutoData]
        public async Task AfterUpdateParams_ShouldReturnEqual(string name)
        {
            var repository = factory.Repository<Employee, Guid>();

            var a = await repository.FirstOrDefaultAsync(x => x.IsDeleted);
            var b = await repository.FirstOrDefaultAsync(x => !x.IsDeleted);

            if (a != null && b != null)
            {
                a.Name = name;
                b.Name = name;

                await repository.UpdateRangeAsync(a, b);
            }

            var actualA = await repository.FirstOrDefaultAsync(x => x.IsDeleted);
            var actualB = await repository.FirstOrDefaultAsync(x => !x.IsDeleted);

            Assert.Equal(name, actualA?.Name);
            Assert.Equal(name, actualB?.Name);
        }

        [Theory, AutoData]
        public async Task AfterUpdateRange_ShouldReturnEqual(string name)
        {
            var repository = factory.Repository<Employee, Guid>();

            var list = await repository.FindByAsync(x => x.IsDeleted);
            foreach (var item in list)
            {
                item.Name = name;
            }

            await repository.UpdateRangeAsync(list);

            var actual = await repository.FindByAsync(x => x.IsDeleted);

            Assert.All(actual, a => Assert.Equal(name, a.Name));
        }

        [Theory, AutoData]
        public async Task AfterUpdateNothing_ShouldReturnEqual(string name)
        {
            var repository = factory.Repository<Employee, Guid>();

            var list = await repository.FindByAsync(x => false);
            foreach (var item in list)
            {
                item.Name = name;
            }

            await repository.UpdateRangeAsync(list);

            var actual = await repository.FindByAsync();

            Assert.All(actual, a => Assert.NotEqual(name, a.Name));
        }

        [Theory, AutoData]
        public async Task AfterUpdateAll_ShouldReturnEqual(string name)
        {
            var repository = factory.Repository<Employee, Guid>();

            await repository.UpdateByAsync(x => new Employee
            {
                Age = x.Age,
                Id = x.Id,
                IsDeleted = x.IsDeleted,
                Name = name,
                QNumber = x.QNumber
            });

            var actual = await repository.FindByAsync();

            Assert.All(actual, a => Assert.Equal(name, a.Name));
        }

        [Theory, AutoData]
        public async Task AfterUpdateById_ShouldReturnEqual(string name)
        {
            var repository = factory.Repository<Employee, Guid>();
            var expected = await repository.FirstOrDefaultAsync();

            await repository.UpdateAsync(expected!.Id, x => new Employee
            {
                Age = x.Age,
                Id = x.Id,
                IsDeleted = x.IsDeleted,
                Name = name,
                QNumber = x.QNumber
            });
            var actual = await repository.FirstOrDefaultAsync(x => x.Name == name);

            expected.Name = name;

            var allUpdated = await repository.FindByAsync(x => x.Name == name);

            Assert.Single(allUpdated);
            Assert.True(expected.IsDeepEqual(actual));
        }

        [Theory, AutoData]
        public async Task AfterUpdateByAction_ShouldReturnEqual(string name)
        {
            var repository = factory.Repository<Employee, Guid>();
            var expected = await repository.CountAsync(x => x.IsDeleted);

            var updated = await repository.UpdateByAsync(x => x.Name = name, x => x.IsDeleted);
            var actual = await repository.CountAsync(x => x.Name == name);
            var allUpdated = await repository.FindByAsync(x => x.Name == name);

            Assert.Equal(updated, allUpdated.Count);
            Assert.Equal(expected, updated);
            Assert.Equal(expected, actual);
        }

        [Theory, AutoData]
        public async Task AfterUpdateByExpression_ShouldReturnEqual(string name)
        {
            var repository = factory.Repository<Employee, Guid>();
            var expected = await repository.CountAsync(x => x.IsDeleted);

            var updated = await repository.UpdateByAsync(x => new Employee
            {
                Age = x.Age,
                Id = x.Id,
                IsDeleted = x.IsDeleted,
                Name = name,
                QNumber = x.QNumber
            }, x => x.IsDeleted);
            var actual = await repository.CountAsync(x => x.Name == name);
            var allUpdated = await repository.FindByAsync(x => x.Name == name);

            Assert.Equal(updated, allUpdated.Count);
            Assert.Equal(expected, updated);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Find_ShouldReturnEqual()
        {
            var repository = factory.Repository<Employee, Guid>();
            var expected = repository.FirstOrDefaultAsync().GetAwaiter().GetResult();

            var actual = repository.Find(expected!.Id);

            Assert.True(expected.IsDeepEqual(actual));
        }

        [Fact]
        public async Task FindAsync_ShouldReturnEqual()
        {
            var repository = factory.Repository<Employee, Guid>();
            var expected = await repository.FirstOrDefaultAsync();

            var actual = await repository.FindAsync(expected!.Id);

            Assert.True(expected.IsDeepEqual(actual));
        }
    }
}
