using AutoMapper;
using FinanceControlinator.Events.Payments;
using Invoices.Handler.Domain.Cqrs.Events;
using MassTransit;
using MediatR;
using System;
using System.Threading.Tasks;

namespace Invoices.Handler.Integration.Handlers
{
    public class PaymentIntegrationHandler : IConsumer<PaymentPerformedEvent>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public PaymentIntegrationHandler(
            IMapper mapper,
            IMediator mediator
            )
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<PaymentPerformedEvent> context)
        {
            var command = _mapper.Map<PaymentPerformedEvent, RegisterInvoicePaymentCommand>(context.Message);

            var result = await _mediator.Send(command);

            if (result.IsFailure) throw result.Error;

            // create a InvoicePaidEvent and publish it :)


        }
    }
}
