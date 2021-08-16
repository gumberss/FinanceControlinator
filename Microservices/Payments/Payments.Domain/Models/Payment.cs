using FinanceControlinator.Common.Entities;
using Payments.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Payments.Domain.Models
{
    public class Payment : Entity<String>
    {
        public Payment()
        {
            PaymentMethods = new List<PaymentMethod>();
        }

        public String Description { get; set; }

        public DateTime Date { get; set; }

        public PaymentStatus Status { get; set; }

        public String ItemId { get; set; }

        public decimal TotalAmount => PaymentMethods.Sum(x => x.Amount);

        public List<PaymentMethod> PaymentMethods { get; set; }
    }
}
