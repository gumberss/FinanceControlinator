using FinanceControlinator.Common.Entities;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using Accounts.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Accounts.Domain.Models
{
    public class Account : Entity<String>
    {
        protected Account()
        {

        }

        public decimal TotalAmount { get; set; }
    }
}
