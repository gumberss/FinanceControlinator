using System;

namespace FinanceControlinator.Common.Entities
{
    public interface IEntity<T>
    {
        public T Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }

    public class Entity<T> : IEntity<T>
    {
        public T Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
