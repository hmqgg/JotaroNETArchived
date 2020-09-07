using AutoFixture.Xunit2;
using Jotaro.Repository.Paginate;
using Jotaro.Repository.Tests.Fixtures;
using Jotaro.Repository.Tests.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Jotaro.Repository.Tests
{
    [Collection(Constants.EfCoreInMemoryEmployee20Test)]
    public class PaginateTests
    {
        private readonly EfCoreInMemoryWith20Fixture fixture;

        public PaginateTests(EfCoreInMemoryWith20Fixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void AfterCreateEmpty_ShouldReturnEmpty()
        {
            var page = PaginateExtensions.CreateEmptyPaginate<int>();

            Assert.Empty(page.Items);
            Assert.Equal(0, page.Size);
            Assert.Equal(0, page.Count);
        }

        [Fact]
        public void AfterCreateConversionEmpty_ShouldReturnEmpty()
        {
            var page = PaginateExtensions.CreateEmptyPaginate<int, string>();

            Assert.Empty(page.Items);
            Assert.Equal(0, page.Size);
            Assert.Equal(0, page.Count);
        }

        [Fact]
        public void WithEmptyAfterSync_ShouldReturnEmpty()
        {
            const int size = 5;
            var list = new List<int>().AsQueryable();

            var page = list.ToPaginate(size);

            Assert.Empty(page.Items);
            Assert.Equal(size, page.Size);
            Assert.Equal(0, page.Count);
        }

        [Fact]
        public async Task WithEmptyAfterAsync_ShouldReturnEmpty()
        {
            const int size = 5;
            var list = new List<int>().AsQueryable();

            var page = await list.ToPaginateAsync(size);

            Assert.Empty(page.Items);
            Assert.Equal(size, page.Size);
            Assert.Equal(0, page.Count);
        }

        [Theory, AutoData]
        public void WithIEnumerableAfterSync_ShouldReturnSame(IEnumerable<int> data)
        {
            var dataList = data.ToList();
            var size = dataList.Count;

            var page = dataList.ToPaginate(size);

            Assert.Equal(size, page.Size);
            Assert.Equal(size, page.Count);
        }

        [Theory, AutoData]
        public void WithIQueryableAfterSync_ShouldReturnSame(IEnumerable<int> data)
        {
            var dataList = data.ToList();
            var size = dataList.Count;

            var page = dataList.AsQueryable().ToPaginate(size);

            Assert.Equal(size, page.Size);
            Assert.Equal(size, page.Count);
        }

        [Theory, AutoData]
        public async Task AfterAsync_ShouldReturnSame(IEnumerable<int> data)
        {
            var dataList = data.ToList();
            var size = dataList.Count;

            var page = await dataList.AsQueryable().ToPaginateAsync(size);

            Assert.Equal(size, page.Size);
            Assert.Equal(size, page.Count);
        }

        [Fact]
        public void WithEmptyAfterConversionSync_ShouldReturnEmpty()
        {
            const int size = 5;
            var list = new List<int>().AsQueryable();

            var page = list.ToPaginate(x => x.ToString(), size);

            Assert.Empty(page.Items);
            Assert.Equal(size, page.Size);
            Assert.Equal(0, page.Count);
        }

        [Fact]
        public async Task WithEmptyAfterConversionAsync_ShouldReturnEmpty()
        {
            const int size = 5;
            var list = new List<int>().AsQueryable();

            var page = await list.ToPaginateAsync(x => x.ToString(), size);

            Assert.Empty(page.Items);
            Assert.Equal(size, page.Size);
            Assert.Equal(0, page.Count);
        }

        [Theory, AutoData]
        public void WithIEnumerableAfterConversionSync_ShouldReturnSame(IEnumerable<int> data)
        {
            var dataList = data.ToList();
            var size = dataList.Count;

            var page = dataList.ToPaginate(x => x.ToString(), size);

            Assert.Equal(size, page.Size);
            Assert.Equal(size, page.Count);
        }

        [Theory, AutoData]
        public void WithIQueryableAfterConversionSync_ShouldReturnSame(IEnumerable<int> data)
        {
            var dataList = data.ToList();
            var size = dataList.Count;

            var page = dataList.AsQueryable().ToPaginate(x => x.ToString(), size);

            Assert.Equal(size, page.Size);
            Assert.Equal(size, page.Count);
        }

        [Theory, AutoData]
        public async Task AfterConversionAsync_ShouldReturnSame(IEnumerable<int> data)
        {
            var dataList = data.ToList();
            var size = dataList.Count;

            var page = await dataList.AsQueryable().ToPaginateAsync(x => x.ToString(), size);

            Assert.Equal(size, page.Size);
            Assert.Equal(size, page.Count);
        }

        [Fact]
        public async Task WithIAsyncEnumerableAfterAsync_ShouldReturnSame()
        {
            const int size = 5;
            var page = await fixture.Context.Employees.ToPaginateAsync(size);

            Assert.Equal(size, page.Size);
            Assert.Equal(size, page.Count);
        }

        [Fact]
        public async Task WithIAsyncEnumerableAfterConversionAsync_ShouldReturnSame()
        {
            const int size = 5;
            var page = await fixture.Context.Employees.ToPaginateAsync(x => new Developer
            {
                Id = x.Id
            }, size);

            Assert.Equal(size, page.Size);
            Assert.Equal(size, page.Count);
        }
    }
}
