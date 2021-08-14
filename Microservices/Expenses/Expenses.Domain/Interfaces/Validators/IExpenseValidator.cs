using Expenses.Domain.Models.Expenses;
using FluentValidation;

namespace Expenses.Domain.Interfaces.Validators
{
    public interface IExpenseValidator : IValidator<Expense>
    {
    }
}
