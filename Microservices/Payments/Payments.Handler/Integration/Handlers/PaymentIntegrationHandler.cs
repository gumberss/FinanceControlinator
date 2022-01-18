using AutoMapper;
using FinanceControlinator.Common.Messaging;
using FinanceControlinator.Events.Payments;
using MassTransit;
using MediatR;
using Payments.Domain.Models;
using Payments.Handler.Domain.Cqrs.Events.Commands;
using System.Threading.Tasks;

namespace Payments.Handler.Integration.Handlers
{
    public class PaymentIntegrationHandler
        : IConsumer<RegisterItemToPayEvent>
        , IConsumer<PaymentConfirmedEvent>
        , IConsumer<PaymentRejectedEvent>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IMessageBus _bus;

        public PaymentIntegrationHandler(
            IMapper mapper,
            IMediator mediator,
            IMessageBus bus
            )
        {
            _mapper = mapper;
            _mediator = mediator;
            _bus = bus;
        }
        public async Task Consume(ConsumeContext<RegisterItemToPayEvent> context)
        {
            var command = _mapper.Map<RegisterPaymentItemCommand>(context.Message);

            var result = await _mediator.Send(command);

            if (result.IsFailure)
            {
                //log
                throw result.Error;
            }
        }

        public async Task Consume(ConsumeContext<PaymentConfirmedEvent> context)
        {
            var command = _mapper.Map<PaymentConfirmedEvent, ConfirmPaymentCommand>(context.Message);

            var result = await _mediator.Send(command);

            if (result.IsFailure) throw result.Error;

            var @event = _mapper.Map<Payment, PaymentPerformedEvent>(result.Value);

            await _bus.Publish(@event);
        }

        public async Task Consume(ConsumeContext<PaymentRejectedEvent> context)
        {
            var command = _mapper.Map<PaymentRejectedEvent, PaymentRejectedCommand>(context.Message);

            var result = await _mediator.Send(command);

            if (result.IsFailure) throw result.Error;
        }
    }
}
