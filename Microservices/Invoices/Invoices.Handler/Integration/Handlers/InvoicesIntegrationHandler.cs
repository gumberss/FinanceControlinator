using AutoMapper;
using FinanceControlinator.Events.Invoices;
using FinanceControlinator.Events.Payments;
using Invoices.Application.Interfaces.AppServices;
using Invoices.Handler.Domain.Cqrs.Events;
using MassTransit;
using MediatR;
using System.Threading.Tasks;

namespace Invoices.Handler.Integration.Handlers
{
    public class InvoicesIntegrationHandler : IConsumer<GenerateInvoicesEvent>
    {
        private readonly IInvoiceAppService _invoiceAppService;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public InvoicesIntegrationHandler(
            IInvoiceAppService invoiceAppService,
            IMapper mapper,
            IMediator mediator
            )
        {
            _invoiceAppService = invoiceAppService;
            _mapper = mapper;
            _mediator = mediator;
        }
        public async Task Consume(ConsumeContext<GenerateInvoicesEvent> context)
        {
            var registerExpenseCommand = _mapper.Map<GenerateInvoicesEvent, RegisterExpenseCommand>(context.Message);

            var result = await _mediator.Send(registerExpenseCommand);

            if (result.IsFailure) throw result.Error;

            var invoices = result.Value;

            var invoicesChangedEvent = _mapper.Map<InvoicesChangedEvent>(invoices);

            foreach (var invoice in invoices)
            {
                var paymentEvent = _mapper.Map<RegisterItemToPayEvent>(invoice);
                await context.Publish(paymentEvent);
            }

            await context.Publish(invoicesChangedEvent);
        }
    }
}
