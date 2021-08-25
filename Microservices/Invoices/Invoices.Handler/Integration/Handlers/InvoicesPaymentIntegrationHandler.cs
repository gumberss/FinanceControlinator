using AutoMapper;
using FinanceControlinator.Common.Messaging;
using FinanceControlinator.Events.Invoices;
using FinanceControlinator.Events.Payments;
using Invoices.Domain.Models;
using Invoices.Handler.Domain.Cqrs.Events;
using MassTransit;
using MediatR;
using System.Threading.Tasks;

namespace Invoices.Handler.Integration.Handlers
{
    public class InvoicesPaymentIntegrationHandler : IConsumer<PaymentPerformedEvent>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IMessageBus _bus;

        public InvoicesPaymentIntegrationHandler(
            IMapper mapper,
            IMediator mediator,
            IMessageBus bus
            )
        {
            _mapper = mapper;
            _mediator = mediator;
            _bus = bus;
        }

        public async Task Consume(ConsumeContext<PaymentPerformedEvent> context)
        {
            var command = _mapper.Map<PaymentPerformedEvent, RegisterInvoicePaymentCommand>(context.Message);

            var result = await _mediator.Send(command);

            if (result.IsFailure) throw result.Error;

            var @event = _mapper.Map<Invoice, InvoicePaidEvent>(result.Value);

            await _bus.Publish(@event);
        }
    }
}
