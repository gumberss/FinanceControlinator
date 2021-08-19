using Newtonsoft.Json;
using System;

namespace FinanceControlinator.Common.Entities
{
    public interface IEntity<T>
    {
        [JsonProperty(PropertyName = "id")]
        public T Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int Version { get; }
    }

    public class Entity<T> : IEntity<T>
    {
        public Entity()
        {
            CreatedDate = DateTime.Now;
        }

        public T Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int Version { get; private set; }

    }
}
