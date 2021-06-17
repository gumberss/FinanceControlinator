using Expenses.Domain.Models;

namespace Expenses.Application.Interfaces.AppServices
{
    public interface IExpenseAppService
    {
        public Expense RegisterExpense(Expense expense);
    }
}
