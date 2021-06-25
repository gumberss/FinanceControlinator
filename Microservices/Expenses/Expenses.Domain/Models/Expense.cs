using Expenses.Domain.Enums;
using FinanceControlinator.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Expenses.Domain.Models
{
    public class Expense : Entity
    {
        public String Title { get; init; }

        public String Description { get; init; }

        public DateTime Date { get; init; }

        public ExpenseType Type { get; init; }

        public bool IsRecurrent { get; init; } //Monthly only yet

        public String Location { get; init; }

        public String Observation { get; init; }

        public decimal TotalCost { get; set; }

        public List<ExpenseItem> Items { get; init; }

        public bool TotalCostIsValid()
            => TotalCost == Items.Sum(x => x.Cost * x.Amount);
    }
}
