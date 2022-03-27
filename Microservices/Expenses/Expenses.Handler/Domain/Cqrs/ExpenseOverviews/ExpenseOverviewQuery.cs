using Expenses.Domain.Models.Expenses;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using MediatR;
using System.Collections.Generic;

namespace Expenses.Handler.Domain.Cqrs.ExpenseOverviews
{
    public class ExpenseOverviewQuery : IRequest<Result<List<ExpenseOverviewDTO>, BusinessException>>
    {
    }
}
