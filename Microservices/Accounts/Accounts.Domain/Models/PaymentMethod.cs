using FinanceControlinator.Common.Entities;
using System;

namespace Accounts.Domain.Models
{
    public class PaymentMethod : Entity<String>
    {
        public String AmountSourceId { get; set; }

        public decimal Amount { get; set; }
    }
}
