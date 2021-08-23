using FinanceControlinator.Common.Entities;
using System;

namespace Accounts.Domain.Models
{
    public class AccountChange : Entity<String>
    {
        protected AccountChange()
        {

        }

        public AccountChange(String accountId)
        {
            Id = Guid.NewGuid().ToString();
            AccountId = accountId;
        }

        public decimal AmountChanged { get; set; }

        public decimal OldAmount { get; set; }

        public decimal NewAmount { get; set; }

        public String AccountId { get; set; }

        public String PaymentId { get; set; }
    }
}
