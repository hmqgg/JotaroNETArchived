using AutoFixture.Xunit2;
using Jotaro.Repository.Repositories.Interfaces;
using Jotaro.Repository.Tests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Jotaro.Server.Tests.Repositories
{
    public abstract class InsertDeleteRepositoryTests
    {
        protected readonly IRepositoryFactory factory;

        protected InsertDeleteRepositoryTests(IRepositoryFactory factory)
        {
            this.factory = factory;
        }

        [Theory, AutoData]
        public async Task AfterInsertOne_ShouldReturnEqual(Employee tester)
        {
            var repository = factory.Repository<Employee, Guid>();
            var expected = 1 + await repository.CountAsync();

            await repository.InsertAsync(tester);

            var actual = await repository.CountAsync();
            var all = await repository.FindByAsync();

            Assert.Equal(expected, actual);
            Assert.Contains(tester.Id, all.Select(x => x.Id));
        }

        [Theory, AutoData]
        public async Task AfterInsertParams_ShouldReturnEqual(Employee tester1, Employee tester2)
        {
            var repository = factory.Repository<Employee, Guid>();
            var expected = 2 + await repository.CountAsync();

            await repository.InsertRangeAsync(tester1, tester2);

            var actual = await repository.CountAsync();
            var all = await repository.FindByAsync();

            Assert.Equal(expected, actual);
            Assert.Contains(tester1.Id, all.Select(x => x.Id));
            Assert.Contains(tester2.Id, all.Select(x => x.Id));
        }

        [Theory, AutoData]
        public async Task AfterInsertRange_ShouldReturnEqual(List<Employee> testers)
        {
            var repository = factory.Repository<Employee, Guid>();
            var expected = testers.Count + await repository.CountAsync();

            await repository.InsertRangeAsync(testers);

            var actual = await repository.CountAsync();
            var all = await repository.FindByAsync();

            Assert.Equal(expected, actual);
            Assert.All(testers, t => Assert.Contains(t.Id, all.Select(x => x.Id)));
        }

        [Fact]
        public async Task AfterDeleteOneById_ShouldReturnEqual()
        {
            var repository = factory.Repository<Employee, Guid>();
            var expected = await repository.CountAsync() - 1;
            var entity = await repository.FirstOrDefaultAsync();

            await repository.DeleteAsync(entity!.Id);

            var actual = await repository.CountAsync();
            var all = await repository.FindByAsync();

            Assert.Equal(expected, actual);
            Assert.DoesNotContain(entity.Id, all.Select(x => x.Id));
        }

        [Fact]
        public async Task AfterDeleteOne_ShouldReturnEqual()
        {
            var repository = factory.Repository<Employee, Guid>();
            var expected = await repository.CountAsync() - 1;
            var entity = await repository.FirstOrDefaultAsync();

            await repository.DeleteAsync(entity!);

            var actual = await repository.CountAsync();
            var all = await repository.FindByAsync();

            Assert.Equal(expected, actual);
            Assert.DoesNotContain(entity.Id, all.Select(x => x.Id));
        }

        [Theory, AutoData]
        public async Task AfterDeleteParamsById_ShouldReturnEqual(Employee tester1, Employee tester2)
        {
            var repository = factory.Repository<Employee, Guid>();
            await repository.InsertRangeAsync(tester1, tester2);

            var expected = await repository.CountAsync() - 2;

            await repository.DeleteRangeAsync(tester1.Id, tester2.Id);
            
            var actual = await repository.CountAsync();
            var all = await repository.FindByAsync();

            Assert.Equal(expected, actual);
            Assert.DoesNotContain(tester1.Id, all.Select(x => x.Id));
            Assert.DoesNotContain(tester2.Id, all.Select(x => x.Id));
        }

        [Theory, AutoData]
        public async Task AfterDeleteParams_ShouldReturnEqual(Employee tester1, Employee tester2)
        {
            var repository = factory.Repository<Employee, Guid>();
            await repository.InsertRangeAsync(tester1, tester2);

            var expected = await repository.CountAsync() - 2;

            await repository.DeleteRangeAsync(tester1, tester2);
            
            var actual = await repository.CountAsync();
            var all = await repository.FindByAsync();

            Assert.Equal(expected, actual);
            Assert.DoesNotContain(tester1.Id, all.Select(x => x.Id));
            Assert.DoesNotContain(tester2.Id, all.Select(x => x.Id));
        }

        [Theory, AutoData]
        public async Task AfterDeleteRangeById_ShouldReturnEqual(List<Employee> testers)
        {
            var repository = factory.Repository<Employee, Guid>();
            await repository.InsertRangeAsync(testers);

            var expected = await repository.CountAsync() - testers.Count;

            await repository.DeleteRangeAsync(testers.Select(x => x.Id));
            
            var actual = await repository.CountAsync();
            var all = await repository.FindByAsync();

            Assert.Equal(expected, actual);
            Assert.All(testers, t => Assert.DoesNotContain(t.Id, all.Select(x => x.Id)));
        }

        [Theory, AutoData]
        public async Task AfterDeleteRange_ShouldReturnEqual(List<Employee> testers)
        {
            var repository = factory.Repository<Employee, Guid>();
            await repository.InsertRangeAsync(testers);

            var expected = await repository.CountAsync() - testers.Count;

            await repository.DeleteRangeAsync(testers);
            
            var actual = await repository.CountAsync();
            var all = await repository.FindByAsync();

            Assert.Equal(expected, actual);
            Assert.All(testers, t => Assert.DoesNotContain(t.Id, all.Select(x => x.Id)));
        }

        [Fact]
        public async Task AfterRemoveBy_ShouldReturnZero()
        {
            var repository = factory.Repository<Employee, Guid>();

            await repository.RemoveByAsync(x => x.IsDeleted);

            var actual = await repository.CountAsync(x => x.IsDeleted);

            Assert.Equal(0, actual);
        }
    }
}
