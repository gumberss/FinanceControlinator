using System;

namespace Expenses.Domain.Models.Expenses.Overviews
{
    public class ExpensePartition
    {
        public String Type { get; set; }

        public float Percent { get; set; }

        public decimal TotalValue { get; set; }
    }
}
