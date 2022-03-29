using Expenses.Domain.Enums;
using Expenses.Domain.Interfaces.Services;
using Expenses.Domain.Models.Expenses;
using Expenses.Domain.Models.Expenses.Overviews;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Expenses.Domain.Services
{
    public class ExpenseOverviewService : IExpenseOverviewService
    {
        public (String local, decimal totalSpendMoney) MostSpentMoneyPlace(List<Expense> expenses)
            => expenses
                   .GroupBy(x => x.Location)
                   .OrderByDescending(x => x.Sum(y => y.TotalCost))
                   .Select(x => (x.Key, x.Sum(y => y.TotalCost)))
                   .FirstOrDefault();

        public ExpenseType? MostSpentType(List<Expense> expenses)
            => expenses
                .GroupBy(x => x.Type)
                .OrderByDescending(x => x.Sum(x => x.TotalCost))
                .FirstOrDefault()?
                .Key;

        public decimal TotalMoneySpent(List<Expense> expenses)
            => expenses.Sum(x => x.TotalCost);

        public List<ExpensePartition> GroupByType(List<Expense> expenses)
        {
            var totalSpent = expenses.Sum(x => x.TotalCost);

            Func<decimal, float> getPercent = current => ((float)(current / totalSpent) * 100);

            var partitionsSpent = expenses
                .GroupBy(x => x.Type)
                .Select(x => new ExpensePartition(x.Key, getPercent(x.Sum(y => y.TotalCost)), x.Sum(y => y.TotalCost)))
                .ToList();

            var partitionsNotSpent =
                Enum.GetValues(typeof(ExpenseType))
                .Cast<ExpenseType>()
                .Except(partitionsSpent.Select(x => x.Type))
                .Select(x => new ExpensePartition(x, 0, 0));

            return partitionsSpent.Concat(partitionsNotSpent).ToList();
        }
    }
}
