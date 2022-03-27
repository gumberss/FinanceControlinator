using System;

namespace Expenses.Domain.Models.Expenses.Overviews
{
    public class ExpensePartition
    {
        public ExpensePartition(string type, float percent, decimal totalValue)
        {
            Type = type;
            Percent = percent;
            TotalValue = totalValue;
        }

        public String Type { get; set; }

        public float Percent { get; set; }

        public decimal TotalValue { get; set; }
    }
}
