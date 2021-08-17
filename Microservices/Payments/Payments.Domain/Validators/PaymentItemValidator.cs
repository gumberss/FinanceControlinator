using Payments.Domain.Interfaces.Validators;
using Payments.Domain.Models;
using FluentValidation;
using System;
using Payments.Domain.Localizations;

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
