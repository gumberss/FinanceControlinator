using Expenses.Domain.Models.Expenses;
using Expenses.DTO.Expenses;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using MediatR;

namespace Expenses.Handler.Domain.Cqrs.Events.Expenses
{
    public class UpdateExpenseCommand : IRequest<Result<Expense, BusinessException>>
    {
        public ExpenseDTO Expense { get; set; }

        public UpdateExpenseCommand(ExpenseDTO expense)
        {
            Expense = expense;
        }
    }
}
