using Expenses.Application.Interfaces.AppServices;
using Expenses.Data.Repositories;
using Expenses.Domain.Models;
using System;

namespace Expenses.Application.AppServices
{
    public class ExpenseAppService : IExpenseAppService
    {
        private readonly IExpenseRepository _expenseRepository;

        public ExpenseAppService(IExpenseRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        public Expense RegisterExpense(Expense expense)
        {
            return expense;
        }
    }
}
