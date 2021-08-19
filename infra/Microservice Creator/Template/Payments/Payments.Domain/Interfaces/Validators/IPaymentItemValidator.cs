using Payments.Domain.Models;
using FluentValidation;

namespace Payments.Domain.Interfaces.Validators
{
    public interface IPaymentItemValidator : IValidator<PaymentItem>
    {
    }
}
