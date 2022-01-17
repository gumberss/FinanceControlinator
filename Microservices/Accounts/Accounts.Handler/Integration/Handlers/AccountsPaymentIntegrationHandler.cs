using Accounts.Domain.Models;
using Accounts.Handler.Domain.Cqrs.Events.Commands.Payments;
using AutoMapper;
using FinanceControlinator.Common.Messaging;
using FinanceControlinator.Events.Accounts;
using FinanceControlinator.Events.Payments;
using MassTransit;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Accounts.Handler.Integration.Handlers
{
    public class AccountsPaymentIntegrationHandler
        : IConsumer<PaymentRequestedEvent>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IMessageBus _bus;

        public AccountsPaymentIntegrationHandler(
            IMapper mapper,
            IMediator mediator,
            IMessageBus bus)
        {
            _mapper = mapper;
            _mediator = mediator;
            _bus = bus;
        }

        public async Task Consume(ConsumeContext<PaymentRequestedEvent> context)
        {
            var command = _mapper.Map<PaymentRequestedEvent, PayCommand>(context.Message);

            var accountChanges = await _mediator.Send(command);

            if (accountChanges.IsFailure) throw accountChanges.Error; //If a rollback occurs here, is can be a problem

            var @event = _mapper.Map<List<AccountChange>, AccountChangedEvent>(accountChanges.Value);

            await _bus.Publish(@event);
        }
    }
}
