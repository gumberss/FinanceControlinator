using Expenses.Domain.Models.Expenses;
using Expenses.Domain.Models.Expenses.Overviews;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using System.Threading.Tasks;

namespace Expenses.Application.Interfaces.AppServices
{
    public interface IExpenseOverviewAppService
    {
        Task<Result<ExpenseOverview, BusinessException>> GetExpensesOverview();
    }
}
