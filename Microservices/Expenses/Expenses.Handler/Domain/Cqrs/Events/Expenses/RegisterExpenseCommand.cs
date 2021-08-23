using Expenses.Domain.Models.Expenses;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using MediatR;

namespace Expenses.Handler.Domain.Cqrs.Events.Expenses
{
    public class RegisterExpenseCommand : IRequest<Result<Expense, BusinessException>>
    {
        public Expense Expense { get; set; }
    }
}
