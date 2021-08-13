using Payments.Domain.Enums;
using FinanceControlinator.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Payments.Domain.Models
{
    public class Payment : Entity<String>
    {
     public List<PaymentItem> Items { get; set; }
    }
}
