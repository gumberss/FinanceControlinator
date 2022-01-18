using AutoMapper;
using MediatR;

namespace Accounts.Handler.Integration.Handlers
{
    public class AccountIntegrationHandler //: IConsumer<PaymentRequested>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public AccountIntegrationHandler(
            IMapper mapper,
            IMediator mediator
            )
        {
            _mapper = mapper;
            _mediator = mediator;
        }
        //public async Task Consume(ConsumeContext<RegisterItemToPayEvent> context)
        //{
        //    var command = _mapper.Map<RegisterAccountItemCommand>(context.Message);

        //    var result = await _mediator.Send(command);

        //    if (result.IsFailure)
        //    {
        //        //log
        //        throw result.Error;
        //    }
        //}
    }
}
