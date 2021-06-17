using Expenses.Domain.Models;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using MediatR;

namespace Expenses.Application.Domain.Cqrs.Events
{
    public class RegisterExpenseCommand : IRequest<Result<Expense, BusinessException>>
    {
        public Expense Expense { get; set; }
    }
}
