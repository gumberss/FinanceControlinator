using CleanHandling;
using MediatR;
using Payments.Domain.Models;

namespace Payments.Handler.Domain.Cqrs.Events.Commands
{
    public class RegisterPaymentItemCommand : IRequest<Result<PaymentItem, BusinessException>>
    {
        public PaymentItem PaymentItem { get; set; }
    }
}
