using FinanceControlinator.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments.Domain.Models
{
    public class PaymentItem : Entity<String>
    {
        public String Title { get; private set; }

        public decimal TotalCost { get; private set; }

        public DateTime DueDate { get; private set; }

        public DateTime CloseDate { get; private set; }

        public String DetailsPath { get; private set; }

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
    }
}
