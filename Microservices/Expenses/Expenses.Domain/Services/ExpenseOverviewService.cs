using Expenses.Domain.Enums;
using Expenses.Domain.Interfaces.Services;
using Expenses.Domain.Models.Expenses;
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
    }
}
