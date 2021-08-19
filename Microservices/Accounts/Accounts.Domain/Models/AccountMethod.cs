using FinanceControlinator.Common.Entities;
using System;

namespace Accounts.Domain.Models
{
    public class AccountMethod : Entity<String>
    {
        public AccountMethod()
        {
            Id = Guid.NewGuid().ToString();
        }

        public int Method { get; set; }

        public String AmountSourceId { get; set; }

        public decimal Amount { get; set; }
    }
}
