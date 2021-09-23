using FluentValidation;
using PiggyBanks.Domain.Interfaces.Validators;
using PiggyBanks.Domain.Localizations;
using PiggyBanks.Domain.Models;

namespace PiggyBanks.Domain.Validators
{
    public class PiggyBankValidator : AbstractValidator<PiggyBank>, IPiggyBankValidator
    {
        public PiggyBankValidator(ILocalization localization)
        {
            //RuleFor(x => x.PurchaseDay)
            //    .GreaterThan(DateTime.MinValue)
            //    .WithMessage(localization.DATE_INCORRECT);

        
        }
    }
}
