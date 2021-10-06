using AutoMapper;
using FinanceControlinator.Common.Messaging;
using FinanceControlinator.Events.Invoices;
using FinanceControlinator.Events.Payments;
using FinanceControlinator.Events.PiggyBanks;
using Invoices.Handler.Domain.Cqrs.Events;
using MassTransit;
using MediatR;
using System.Threading.Tasks;

namespace Invoices.Handler.Integration.Handlers
{
    public class InvoicesPiggyBankIntegrationHandler :
        IConsumer<PiggyBankCreatedEvent>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IMessageBus _bus;

        public InvoicesPiggyBankIntegrationHandler(
            IMapper mapper,
            IMediator mediator,
            IMessageBus bus
            )
        {
            _mapper = mapper;
            _mediator = mediator;
            _bus = bus;
        }

        public async Task Consume(ConsumeContext<PiggyBankCreatedEvent> context)
        {
            var registerExpenseCommand = _mapper.Map<PiggyBankCreatedEvent, RegisterPiggyBankExpenseCommand>(context.Message);

            var result = await _mediator.Send(registerExpenseCommand);

            if (result.IsFailure) throw result.Error;

            var invoices = result.Value;

            var invoicesChangedEvent = _mapper.Map<InvoicesChangedEvent>(invoices);

            foreach (var invoice in invoices)
            {
                var paymentEvent = _mapper.Map<RegisterItemToPayEvent>(invoice);
                await context.Publish(paymentEvent);
            }

            await _bus.Publish(invoicesChangedEvent);
        }
    }
}
