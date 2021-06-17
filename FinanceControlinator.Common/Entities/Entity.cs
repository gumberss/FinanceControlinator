using System;

namespace FinanceControlinator.Common.Entities
{
    public interface IEntity
    {
        public Guid Id { get; set; }
    }

    public class Entity : IEntity
    {
        public Guid Id { get; set; }
    }
}
