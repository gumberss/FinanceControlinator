using Expenses.Domain.Models;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Expenses.Application.Interfaces.AppServices
{
    public interface IExpenseAppService
    {
        public Task<Result<Expense, BusinessException>> RegisterExpense(Expense expense);

        Task<Result<List<Expense>, BusinessException>> GetAllExpenses();

        Task<Result<List<Expense>, BusinessException>> GetMonthExpenses();

        Task<Result<List<Expense>, BusinessException>> GetLastMonthExpenses();
    }
}
