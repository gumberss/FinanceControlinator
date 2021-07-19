using Expenses.Domain.Models;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using MediatR;

namespace Expenses.Handler.Domain.Cqrs.Events
{
    public class RegisterExpenseCommand : IRequest<Result<Expense, BusinessException>>
    {
        public Expense Expense { get; set; }
    }
}
