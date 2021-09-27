using FluentValidation;
using PiggyBanks.Domain.Models;

namespace PiggyBanks.Domain.Interfaces.Validators
{
    public interface IPiggyBankValidator : IValidator<PiggyBank>
    {
    }
}
