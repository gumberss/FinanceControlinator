using Expenses.Domain.Models.Expenses;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Expenses.Application.Interfaces.AppServices
{
    public interface IExpenseAppService
    {
        Task<Result<Expense, BusinessException>> RegisterExpense(Expense expense);

        Task<Result<Expense, BusinessException>> UpdateExpense(Expense expense);

        Task<Result<List<Expense>, BusinessException>> GetAllExpenses();

        Task<Result<List<Expense>, BusinessException>> GetMonthExpenses();

        Task<Result<List<Expense>, BusinessException>> GetLastMonthExpenses();

        Task<Result<List<Expense>, BusinessException>> GetByPagination(int page, int count, Guid userId);
    }
}
