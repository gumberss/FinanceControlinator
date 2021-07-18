using FinanceControlinator.Common.Entities;
using System;

namespace Invoices.Domain.Models
{
    public class ExpenseItem : Entity<String>
    {
        public String Name { get; set; }

        public String Description { get; set; }

        public decimal Cost { get; set; }

        public int Amount { get; set; }

        public Guid InvoiceId { get; set; }

        public Expense Invoice { get; set; }
    }
}
