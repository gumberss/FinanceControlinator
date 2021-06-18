using Expenses.Domain.Models;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using System.Threading.Tasks;

namespace Expenses.Application.Interfaces.AppServices
{
    public interface IExpenseAppService
    {
        public Task<Result<Expense, BusinessException>> RegisterExpense(Expense expense);
    }
}
