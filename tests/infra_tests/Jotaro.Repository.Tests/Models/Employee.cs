using Jotaro.Repository.Entities;
using System;

namespace Jotaro.Repository.Tests.Models
{
    public class Employee : IHasId<Guid>, IHasSoftDelete
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public long QNumber { get; set; }

        public bool IsDeleted { get; set; }
    }
}
