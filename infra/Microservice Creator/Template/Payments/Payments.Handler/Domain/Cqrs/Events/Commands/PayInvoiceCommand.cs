using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using MediatR;
using Payments.Domain.Models;
using System;
using System.Collections.Generic;

namespace Payments.Handler.Domain.Cqrs.Events.Commands
{
    public class PayInvoiceCommand : IRequest<Result<Payment, BusinessException>>
    {
        public String Description { get; set; }

        public Guid ItemId { get; set; }

        public List<PaymentMethodDTO> PaymentMethods { get; set; }
    }

    public class PaymentMethodDTO
    {
        public int Method { get; set; }

        public Guid AmountSourceId { get; set; }

        public decimal Amount { get; set; }
    }
}
