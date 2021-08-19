using FinanceControlinator.Common.Entities;
using System;

namespace Accounts.Domain.Models
{
    public class Account : Entity<String>
    {
        public Account(Guid? id = null)
        {
            Id = (id ?? Guid.NewGuid()).ToString();
        }

        public decimal TotalAmount { get;  set; }

        public Account Receive(decimal amount)
        {
            TotalAmount += amount;

            return this;
        }
    }
}
