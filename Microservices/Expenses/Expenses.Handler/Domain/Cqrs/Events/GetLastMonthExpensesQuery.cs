using Expenses.Domain.Models;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using MediatR;
using System.Collections.Generic;

namespace Expenses.Handler.Domain.Cqrs.Events
{
    public class GetLastMonthExpensesQuery : IRequest<Result<List<Expense>, BusinessException>>
    {
    }
}
