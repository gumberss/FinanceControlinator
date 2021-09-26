using AutoMapper;
using FinanceControlinator.Common.Messaging;
using FinanceControlinator.Events.Invoices;
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

            await _bus.Publish(invoicesChangedEvent);
            
            // Salvo na base o PiggyBank? Não

            // Converter dto para Objeto de negocio
            // postar command com o objeto de negocio
            // Montar fatura com base no objeto de negocio
            // Salvar novas faturas / Atualizar faturas existentes

        }
    }
}
