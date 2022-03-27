using Expenses.Domain.Enums;
using Expenses.Domain.Models.Expenses;
using System;
using System.Collections.Generic;

namespace Expenses.Domain.Interfaces.Services
{
    public interface IExpenseOverviewService
    {
        decimal TotalMoneySpent(List<Expense> expenses);
        ExpenseType? MostSpentType(List<Expense> expenses);
        (String local, decimal totalSpendMoney) MostSpentMoneyPlace(List<Expense> expenses);
    }
}
