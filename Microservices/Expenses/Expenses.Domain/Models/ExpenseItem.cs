using System;

namespace Expenses.Domain.Models
{
    public class ExpenseItem
    {
        public Guid Id { get; init; }

        public String Name { get; init; }

        public String Description { get; init; }

        public decimal Amount { get; init; }

        public Guid ExpenseId { get; set; }

        public Expense Expense { get; init; }
    }
}
