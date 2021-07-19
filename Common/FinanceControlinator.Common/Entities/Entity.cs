using System;

namespace FinanceControlinator.Common.Entities
{
    public interface IEntity<T>
    {
        public T Id { get; set; }

        public DateTime InsertDate { get; set; }

        public DateTime UpdateDate { get; set; }
    }

    public class Entity<T> : IEntity<T>
    {
        public T Id { get; set; }

        public DateTime InsertDate { get; set; }

        public DateTime UpdateDate { get; set; }
    }
}
