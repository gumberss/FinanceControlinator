using Expenses.Domain.Models.Expenses;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using MediatR;

namespace Expenses.Handler.Domain.Cqrs.ExpenseOverviews
{
    public class ExpenseOverviewQuery : IRequest<Result<ExpenseOverviewDTO, BusinessException>>
    {
    }
}
