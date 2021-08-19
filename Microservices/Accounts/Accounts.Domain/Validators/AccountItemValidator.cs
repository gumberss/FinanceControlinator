using Accounts.Domain.Interfaces.Validators;
using Accounts.Domain.Models;
using FluentValidation;
using System;
using Accounts.Domain.Localizations;

namespace Accounts.Domain.Validators
{
    public class AccountItemValidator : AbstractValidator<AccountItem>, IAccountItemValidator
    {
        public AccountItemValidator(ILocalization localization)
        {
            RuleFor(x => x.CloseDate)
             .LessThan(DateTime.Now)
             .WithMessage(localization.PAYMENT_ITEM_NOT_CLOSED);
        }
    }
}
