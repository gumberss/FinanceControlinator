using FinanceControlinator.Common.Entities;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
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
            PaymentMethods = new List<PaymentMethod>();
            
            Status = PaymentStatus.PaymentRequested;
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

            item.PaymentIds.Add(this.Id);
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

        public bool InProcess() => Status == PaymentStatus.PaymentRequested;

        public bool Paid() => Status == PaymentStatus.Paid;
    }
}
