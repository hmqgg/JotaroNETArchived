using AutoFixture;
using Jotaro.Repository.Tests.Models;
using Mongo2Go;
using MongoDB.Driver;
using System;

namespace Jotaro.Server.Tests.Fixtures
{
    public class MongoWith20Fixture : IDisposable
    {
        private readonly MongoDbRunner runner;
        public IMongoDatabase Database { get; }

        public MongoWith20Fixture()
        {
            runner = MongoDbRunner.Start();
            var client = new MongoClient(runner.ConnectionString);
            Database = client.GetDatabase("server-test");

            var fixture = new Fixture();
            var data = fixture.CreateMany<Employee>(20);

            var col = Database.GetCollection<Employee>(nameof(Employee));
            col.InsertMany(data);
        }

        public void Dispose()
        {
            runner?.Dispose();
        }
    }
}
