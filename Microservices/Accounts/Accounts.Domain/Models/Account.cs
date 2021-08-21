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

        public bool AbleToPay(decimal amount)
        {
            throw new NotImplementedException();
        }

        public void Retrieve(decimal amount)
        {
            throw new NotImplementedException();
        }

        public AccountChange Withdraw(decimal amount, String paymentId)
        {
            TotalAmount -= amount;

            return new AccountChange(this.Id)
            {
                AmountChanged = amount,
                NewAmount = TotalAmount,
                OldAmount = TotalAmount + amount,
                PaymentId = paymentId
            };
        }

        public AccountChange RejectWithDraw(decimal amount)
        {
            return new AccountChange(this.Id)
            {
                AmountChanged = amount,
                NewAmount = TotalAmount,
                OldAmount = TotalAmount + amount,
            };
        }
    }
}
