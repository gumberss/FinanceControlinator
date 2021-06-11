using Expenses.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Domain.Models
{
    public record Expense
    {
        public Guid Id { get; init; }

        public String Title { get; init; }

        public String Description { get; init; }

        public DateTime Date { get; init; }

        public ExpenseType Type { get; init; }

        public bool IsRecurrent { get; init; }

        public String Location { get; init; }

        public String Observation { get; init; }

        public List<ExpenseItem> Items { get; init; }
    }
}
