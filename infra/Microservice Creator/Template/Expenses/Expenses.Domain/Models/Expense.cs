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
        public String Title { get; set; }

        public String Description { get; set; }

        public DateTime Date { get; set; }

        public ExpenseType Type { get; set; }

        public bool IsRecurrent { get; set; } //Monthly only yet

        public String Location { get; set; }

        public String Observation { get; set; }

        public decimal TotalCost { get; set; }

        public List<ExpenseItem> Items { get; set; }

        public bool TotalCostIsValid()
            => TotalCost == Items.Sum(x => x.Cost * x.Amount);
    }
}
