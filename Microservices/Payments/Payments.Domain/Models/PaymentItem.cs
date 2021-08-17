using FinanceControlinator.Common.Entities;
using System;
using System.Collections.Generic;

namespace Payments.Domain.Models
{
    public class PaymentItem : Entity<String>
    {
        public PaymentItem()
        {
            PaymentIds = new List<string>();
        }

        public String Title { get; private set; }

        public decimal TotalCost { get; private set; }

        public DateTime DueDate { get; private set; }

        public DateTime CloseDate { get; private set; }

        public String DetailsPath { get; private set; }

        public List<String> PaymentIds { get; private set; }

        public bool CanUpdate() => CloseDate < DateTime.Now;

        public PaymentItem UpdateFrom(PaymentItem paymentItem)
        {
            Title = paymentItem.Title;
            TotalCost = paymentItem.TotalCost;
            DueDate = paymentItem.DueDate;
            CloseDate = paymentItem.CloseDate;
            DetailsPath = paymentItem.DetailsPath;

            return this;
        }

        public bool CanBePaid() => CloseDate < DateTime.Now;
    }
}
