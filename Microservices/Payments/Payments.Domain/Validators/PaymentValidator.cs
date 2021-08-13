using Payments.Domain.Interfaces.Validators;
using Payments.Domain.Models;
using FinanceControlinator.Common.Localizations;
using FluentValidation;
using System;

namespace Payments.Domain.Validators
{
    public class PaymentValidator : AbstractValidator<Payment>, IPaymentValidator
    {
        public PaymentValidator(ILocalization localization)
        {
           
        }
    }

    public class PaymentItemValidator : AbstractValidator<PaymentItem>
    {
        public PaymentItemValidator(ILocalization localization)
        {
         
        }
    }
}
