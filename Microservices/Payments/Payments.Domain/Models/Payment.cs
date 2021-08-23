using FinanceControlinator.Common.Entities;
using Payments.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Payments.Domain.Models
{
    public class Payment : Entity<String>
    {
        protected Payment()
        {

        }

        public Payment(DateTime paymentDate)
        {
            Id = Guid.NewGuid().ToString();
            Date = paymentDate;
            PaymentMethods = new List<PaymentMethod>();
        }

        public String Description { get; set; }

        public DateTime? Date { get; set; }

        public PaymentStatus Status { get; set; }

        public String ItemId { get; set; }

        public decimal TotalAmount => PaymentMethods.Sum(x => x.Amount);

        public List<PaymentMethod> PaymentMethods { get; set; }

        public Payment For(PaymentItem item)
        {
            ItemId = item.Id;
            item.AddPaymentRequest(this);

            return this;
        }

        public Payment PaidWith(List<PaymentMethod> paymentMethods)
        {
            PaymentMethods = paymentMethods;

            return this;
        }

        public Payment With(String description)
        {
            Description = description;
            return this;
        }

        public Payment AsRequested()
        {
            Status = PaymentStatus.PaymentRequested;

            PaymentMethods.ForEach(pm => pm.AsRequested());

            return this;
        }

        public bool InProcess() => Status == PaymentStatus.PaymentRequested;

        public bool Paid() => Status == PaymentStatus.Paid;

        public bool WaitingForConfirmation() => Status == PaymentStatus.PaymentRequested;

        public Payment Confirm()
        {
            Status = PaymentStatus.Paid;

            PaymentMethods.ForEach(pm => pm.ConfirmPayment());

            Updated();

            return this;
        }
    }
}
