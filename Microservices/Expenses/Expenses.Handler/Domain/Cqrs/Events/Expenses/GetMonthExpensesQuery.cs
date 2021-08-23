using Expenses.Domain.Models.Expenses;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using MediatR;
using System.Collections.Generic;


namespace Expenses.Handler.Domain.Cqrs.Events.Expenses
{
    public class GetMonthExpensesQuery : IRequest<Result<List<Expense>, BusinessException>>
    {
    }
}
