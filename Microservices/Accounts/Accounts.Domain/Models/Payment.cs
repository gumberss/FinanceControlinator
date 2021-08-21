using FinanceControlinator.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Accounts.Domain.Models
{
    public class Payment : Entity<String>
    {
        public decimal TotalAmount { get; set; }

        public String ItemId { get; set; }

        public List<PaymentMethod> PaymentMethods { get; set; }

        public IEnumerable<String> SourceIds()
        {
            return PaymentMethods
                .Select(x => x.AmountSourceId)
                .Distinct();
        }
    }
}
