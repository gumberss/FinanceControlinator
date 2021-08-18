using FinanceControlinator.Common.Entities;
using System;

namespace Payments.Domain.Models
{
    public class PaymentMethod : Entity<String>
    {
        public PaymentMethod()
        {
            Id = Guid.NewGuid().ToString();
        }

        public int Method { get; set; }

        public String AmountSourceId { get; set; }

        public decimal Amount { get; set; }
    }
}
