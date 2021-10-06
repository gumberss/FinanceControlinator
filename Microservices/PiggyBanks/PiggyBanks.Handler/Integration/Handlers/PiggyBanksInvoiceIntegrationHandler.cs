using AutoMapper;
using FinanceControlinator.Events.Invoices;
using MassTransit;
using MediatR;
using PiggyBanks.Domain.Models;
using PiggyBanks.Handler.Domain.Cqrs.Events;
using PiggyBanks.Handler.Domain.Cqrs.Events.Invoices;
using PiggyBanks.Handler.Integration.Events.Invoices;
using System.Threading.Tasks;

namespace PiggyBanks.Handler.Integration.Handlers
{
    public class PiggyBanksInvoiceIntegrationHandler :
        IConsumer<InvoicePaidEvent>,
        IConsumer<PiggyBankPaidInvoiceRegisteredEvent>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IBus _bus;

        public PiggyBanksInvoiceIntegrationHandler(
            IMediator mediator,
            IMapper mapper,
            IBus bus
            )
        {
            _mediator = mediator;
            _mapper = mapper;
            _bus = bus;
        }

        public async Task Consume(ConsumeContext<InvoicePaidEvent> context)
        {
            var command = _mapper.Map<InvoicePaidEvent, InvoicePaidCommand>(context.Message);

            var invoice = await _mediator.Send(command);

            if (invoice.IsFailure) throw invoice.Error;

            var paidInvoiceRegisteredEvent = _mapper.Map<Invoice, PiggyBankPaidInvoiceRegisteredEvent>(invoice.Value);

            await _bus.Publish(paidInvoiceRegisteredEvent);
        }

        public async Task Consume(ConsumeContext<PiggyBankPaidInvoiceRegisteredEvent> context)
        {
            var command = _mapper.Map<PiggyBankPaidInvoiceRegisteredEvent, RegisterPiggyBanksPaymentCommand>(context.Message);

            var invoice = await _mediator.Send(command);

            if (invoice.IsFailure) throw invoice.Error;

            // PiggyBanks Instlalment Paid
        }
    }
}
