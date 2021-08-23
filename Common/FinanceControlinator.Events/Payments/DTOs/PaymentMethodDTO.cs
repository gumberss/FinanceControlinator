using System;

namespace FinanceControlinator.Events.Payments.DTOs
{
    public class PaymentMethodDTO
    {
        public Guid Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int Version { get; private set; }

        public Guid AmountSourceId { get; set; }

        public decimal Amount { get; set; }

        public String Description { get; set; }

        public int Status { get; set; }
    }
}
