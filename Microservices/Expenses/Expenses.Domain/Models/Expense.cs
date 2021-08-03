using Expenses.Domain.Enums;
using FinanceControlinator.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Expenses.Domain.Models
{
    public class Expense : Entity<Guid>
    {
        public String Title { get; set; }

        public String Description { get; set; }

        public DateTime PurchaseDay { get; set; }

        public ExpenseType Type { get; set; }

        public bool InstallmentsCount { get; set; }

        public String Location { get; set; }

        public String Observation { get; set; }

        public decimal TotalCost { get; set; }

        public List<ExpenseItem> Items { get; set; }

        public bool TotalCostIsValid()
            => TotalCost == Items.Sum(x => x.Cost * x.Amount);
    }
}
