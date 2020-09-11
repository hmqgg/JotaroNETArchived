using AutoFixture;
using Jotaro.Repository.Tests.Models;
using LiteDB;
using System;
using System.IO;

namespace Jotaro.Server.Tests.Fixtures
{
    public class LiteDbWith20Fixture : IDisposable
    {
        private readonly MemoryStream memoryStream;
        public ILiteDatabase Database { get; }

        public LiteDbWith20Fixture()
        {
            memoryStream = new MemoryStream();
            Database = new LiteDatabase(memoryStream); 

            var fixture = new Fixture();
            var data = fixture.CreateMany<Employee>(20);

            var col = Database.GetCollection<Employee>();
            col.InsertBulk(data);
        }

        public void Dispose()
        {
            Database.Dispose();
            memoryStream.Dispose();
        }
    }
}
