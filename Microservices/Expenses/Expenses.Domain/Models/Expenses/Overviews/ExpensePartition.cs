using Expenses.Domain.Enums;

namespace Expenses.Domain.Models.Expenses.Overviews
{
    public class ExpensePartition
    {
        public ExpensePartition(ExpenseType type, float percent, decimal totalValue)
        {
            Type = type;
            Percent = percent;
            TotalValue = totalValue;
        }

        public ExpenseType Type { get; set; }

        public float Percent { get; set; }

        public decimal TotalValue { get; set; }
    }
}
