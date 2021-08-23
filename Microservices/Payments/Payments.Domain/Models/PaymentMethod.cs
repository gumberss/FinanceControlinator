using FinanceControlinator.Common.Entities;
using Payments.Domain.Enums;
using System;

namespace Payments.Domain.Models
{
    public class PaymentMethod : Entity<String>
    {
        public PaymentMethod()
        {
            Id = Guid.NewGuid().ToString();
        }

        public String AmountSourceId { get; set; }

        public decimal Amount { get; set; }

        public String Description { get; set; }

        public PaymentStatus Status { get; set; }

        public void ConfirmPayment() => Status = PaymentStatus.Paid;

        public PaymentMethod AsRequested()
        {
            Status = PaymentStatus.PaymentRequested;

            return this;
        }
    }
}
