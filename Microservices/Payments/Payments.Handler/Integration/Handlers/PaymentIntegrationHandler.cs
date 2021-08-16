using AutoMapper;
using FinanceControlinator.Events.Payments;
using MassTransit;
using MediatR;
using Payments.Handler.Domain.Cqrs.Events.Commands;
using System.Threading.Tasks;

namespace Payments.Handler.Integration.Handlers
{
    public class PaymentIntegrationHandler : IConsumer<RegisterItemToPayEvent>
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
    }
}
