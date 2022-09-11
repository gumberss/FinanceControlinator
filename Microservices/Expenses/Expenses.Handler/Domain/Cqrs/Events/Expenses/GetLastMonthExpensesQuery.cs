using CleanHandling;
using Expenses.Domain.Models.Expenses;
using MediatR;
using System.Collections.Generic;

namespace Expenses.Handler.Domain.Cqrs.Events.Expenses
{
    public class GetLastMonthExpensesQuery : IRequest<Result<List<Expense>, BusinessException>>
    {
    }
}
