using AutoFixture.Xunit2;
using Jotaro.Repository.Paginate;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Jotaro.Repository.Tests
{
    public class PaginateTests
    {
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
        public async Task AfterAsync_ShouldReturnSame(IEnumerable<int> data)
        {
            var dataList = data.ToList();
            var size = dataList.Count;

            var page = await dataList.AsQueryable().ToPaginateAsync(size);

            Assert.Equal(size, page.Size);
            Assert.Equal(size, page.Count);
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
        public async Task AfterConversionAsync_ShouldReturnSame(IEnumerable<int> data)
        {
            var dataList = data.ToList();
            var size = dataList.Count;

            var page = await dataList.AsQueryable().ToPaginateAsync(x => x.ToString(), size);

            Assert.Equal(size, page.Size);
            Assert.Equal(size, page.Count);
        }
    }
}
