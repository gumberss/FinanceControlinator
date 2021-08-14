using Expenses.Domain.Models;
using FluentValidation;

namespace Expenses.Domain.Interfaces.Validators
{
    public interface IExpenseValidator : IValidator<Expense>
    {
    }
}
