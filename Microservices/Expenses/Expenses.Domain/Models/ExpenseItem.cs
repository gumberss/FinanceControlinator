using FinanceControlinator.Common.Entities;
using System;

namespace Expenses.Domain.Models
{
    public class ExpenseItem : Entity<Guid>
    {
        public String Name { get; set; }

        public String Description { get; set; }

        public decimal Cost { get; set; }

        public int Amount { get; set; }

        public Guid ExpenseId { get; set; }

        public Expense Expense { get; set; }
    }
}
