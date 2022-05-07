using Expenses.Domain.Models.Expenses;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using MediatR;

namespace Expenses.Handler.Domain.Cqrs.Events.Expenses
{
    public class UpdateExpenseCommand : IRequest<Result<Expense, BusinessException>>
    {
        public Expense Expense { get; set; }

        public UpdateExpenseCommand(Expense expense)
        {
            Expense = expense;
        }
    }
}
