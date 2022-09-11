using CleanHandling;
using Expenses.Domain.Models.Expenses;
using Expenses.DTO.Expenses;
using MediatR;

namespace Expenses.Handler.Domain.Cqrs.Events.Expenses
{
    public class RegisterExpenseCommand : IRequest<Result<Expense, BusinessException>>
    {
        public ExpenseDTO Expense { get; set; }

        public RegisterExpenseCommand(ExpenseDTO expense)
        {
            Expense = expense;
        }
    }
}
