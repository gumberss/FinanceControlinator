using CleanHandling;
using MediatR;
using Payments.Domain.Models;
using System;
using System.Collections.Generic;

namespace Payments.Handler.Domain.Cqrs.Events.Commands
{
    public class PayItemCommand : IRequest<Result<Payment, BusinessException>>
    {
        public String Description { get; set; }

        public Guid ItemId { get; set; }

        public List<PaymentMethodCommandDTO> PaymentMethods { get; set; }
    }

    public class PaymentMethodCommandDTO
    {
        public Guid AmountSourceId { get; set; }

        public decimal Amount { get; set; }
    }
}
