using System;
using AutoFixture;
using Jotaro.Repository.Tests.Models;
using Microsoft.EntityFrameworkCore;

namespace Jotaro.Server.Tests.Models
{
    public class ServerTestContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var fixture = new Fixture();
            var data = fixture.CreateMany<Employee>(20);
            modelBuilder.Entity<Employee>().HasData(data);
        }
    }
}
