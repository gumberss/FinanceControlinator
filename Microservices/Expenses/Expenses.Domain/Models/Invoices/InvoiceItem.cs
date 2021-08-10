﻿using FinanceControlinator.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.Domain.Models.Invoices
{
    public class InvoiceItem : Entity<Guid>
    {
        public Guid ExpenseId { get; set; }

        public decimal InstallmentCost { get; set; }

        public Guid InvoiceId { get; set; }

        public virtual Invoice Invoice { get; set; }

        internal void LinkTo(Invoice invoice)
        {
            this.InvoiceId = invoice.Id;
            this.Invoice = invoice;
        }
    }
}
