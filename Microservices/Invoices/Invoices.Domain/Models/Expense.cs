using Invoices.Domain.Enums;
using FinanceControlinator.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Invoices.Domain.Models
{
    public class Expense : Entity<String>
    {
        public String Title { get; set; }

        public String Description { get; set; }

        public DateTime PurchaseDay { get; set; }

        public ExpenseType Type { get; set; }

        public int InstallmentsCount { get; set; }

        public String Location { get; set; }

        public String Observation { get; set; }

        public decimal TotalCost { get; set; }

        public List<ExpenseItem> Items { get; set; }

        public bool TotalCostIsValid()
            => TotalCost == Items.Sum(x => x.Cost * x.Amount);
    }
}