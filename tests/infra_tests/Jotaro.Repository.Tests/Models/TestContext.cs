using Microsoft.EntityFrameworkCore;

namespace Jotaro.Repository.Tests.Models
{
    public class TestContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("test");
        }
    }
}
