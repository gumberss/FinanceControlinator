using FluentValidation;
using PiggyBanks.Domain.Interfaces.Validators;
using PiggyBanks.Domain.Localizations;
using PiggyBanks.Domain.Models;
using System;

namespace PiggyBanks.Domain.Validators
{
    public class PiggyBankValidator : AbstractValidator<PiggyBank>, IPiggyBankValidator
    {
        public PiggyBankValidator(ILocalization localization)
        {
            RuleFor(x => x.GoalDate)
                .GreaterThan(DateTime.Now)
                .WithMessage(localization.PIGGY_BANK_INVALID_GOAL_DATE);

            RuleFor(x => x.GoalValue)
                .GreaterThan(0)
                .WithMessage(localization.PIGGY_BANK_INVALID_GOAL_VALUE);

            RuleFor(x => x.SavedValue)
                .GreaterThanOrEqualTo(0)
                .WithMessage(localization.PIGGY_BANK_INVALID_SAVED_VALUE);

            RuleFor(x => x.StartDate)
                .GreaterThan(DateTime.MinValue)
                .WithMessage(localization.PIGGY_BANK_INVALID_START_DATE);

            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage(localization.PIGGY_BANK_WITHOUT_TITLE);

            RuleFor(x => x.Description)
               .MaximumLength(500)
               .WithMessage(localization.PIGGY_BANK_WITH_BIG_DESCRIPTION);
        }
    }
}
