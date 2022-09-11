using CleanHandling;
using MediatR;
using Payments.Domain.Models;
using System;

namespace Payments.Handler.Domain.Cqrs.Events.Commands
{
    public class PaymentRejectedCommand
           : IRequest<Result<Payment, BusinessException>>
    {
        public String Id { get; set; }

        public String Reason { get; set; }
    }
}
