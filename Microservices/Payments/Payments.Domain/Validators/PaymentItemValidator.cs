using FluentValidation;
using Payments.Domain.Interfaces.Validators;
using Payments.Domain.Localizations;
using Payments.Domain.Models;
using System;

namespace Payments.Domain.Validators
{
    public class PaymentItemValidator : AbstractValidator<PaymentItem>, IPaymentItemValidator
    {
        public PaymentItemValidator(ILocalization localization)
        {
            RuleFor(x => x.CloseDate)
             .LessThan(DateTime.Now)
             .WithMessage(localization.PAYMENT_ITEM_NOT_CLOSED);
        }
    }
}
