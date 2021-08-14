using FinanceControlinator.Common.Entities;
using System;
using System.Collections.Generic;

namespace Payments.Domain.Models
{
    public class Payment : Entity<String>
    {
     public List<PaymentDetails> Items { get; set; }
    }
}
