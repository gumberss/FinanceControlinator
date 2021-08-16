using FinanceControlinator.Common.Entities;
using System;

namespace Payments.Domain.Models
{
    public class PaymentMethod : Entity<String>
    {
        public int Method { get; set; }

        public String AmountSourceId { get; set; }

        public decimal Amount { get; set; }
    }
}
