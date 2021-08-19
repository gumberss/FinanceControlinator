using FinanceControlinator.Common.Entities;
using System;

namespace Accounts.Domain.Models
{
    public class Account : Entity<String>
    {
        public Account()
        {
            Id = Guid.NewGuid().ToString();
        }

        public decimal TotalAmount { get; private set; }

        public Account Receive(decimal amount)
        {
            TotalAmount += amount;

            return this;
        }
    }
}
