using Accounts.Domain.Models;
using FluentValidation;

namespace Accounts.Domain.Interfaces.Validators
{
    public interface IAccountItemValidator : IValidator<AccountItem>
    {
    }
}
