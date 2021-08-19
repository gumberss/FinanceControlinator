using FinanceControlinator.Common.Entities;
using Accounts.Domain.Enums;
using System;

namespace Accounts.Domain.Models
{
    public class InvoiceItem : Entity<String>
    {
        protected InvoiceItem() { }

        public String ExpenseId { get; set; }

        public int InstallmentNumber { get; set; }

        public decimal InstallmentCost { get; set; }

        public InvoiceItemType Type { get; set; }

        public DateTime PurchaseDay { get; set; }

        public String Location { get; set; }

        public String Title { get; set; }
    }
}
