using FinanceControlinator.Common.Entities;
using System;

namespace PiggyBanks.Domain.Models
{
    public class Transfer : Entity<Guid>
    {
        public Guid SourceId { get; set; }

        public virtual PiggyBank Source { get; set; }

        public Guid DestinationId { get; set; }

        public virtual PiggyBank Destination { get; set; }

        public decimal Amount { get; set; }

        public void Deconstruct(out Guid destinationId, out Guid sourceId, out decimal amount)
        {
            destinationId = DestinationId;
            sourceId = SourceId;
            amount = Amount;
        }
    }
}
