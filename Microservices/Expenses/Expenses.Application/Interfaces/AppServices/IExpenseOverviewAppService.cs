using CleanHandling;
using Expenses.Domain.Models.Expenses.Overviews;
using System;
using System.Threading.Tasks;

namespace Expenses.Application.Interfaces.AppServices
{
    public interface IExpenseOverviewAppService
    {
        Task<Result<ExpenseOverview, BusinessException>> GetExpensesOverview(Guid userId);
    }
}
