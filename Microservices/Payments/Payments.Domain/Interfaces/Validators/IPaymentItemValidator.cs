using FluentValidation;
using Payments.Domain.Models;

namespace Payments.Domain.Interfaces.Validators
{
    public interface IPaymentItemValidator : IValidator<PaymentItem>
    {
    }
}
