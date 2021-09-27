using AutoMapper;
using FinanceControlinator.Events.PiggyBanks;
using MassTransit;
using MediatR;
using PiggyBanks.Handler.Domain.Cqrs.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiggyBanks.Handler.Integration.Handlers
{
    public class PiggyBanksIntegrationHandler : IConsumer<SaveMoneyEvent>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public PiggyBanksIntegrationHandler(
            IMediator mediator,
            IMapper mapper
            )
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<SaveMoneyEvent> context)
        {
            var command = _mapper.Map<SaveMoneyEvent, SaveMoneyCommand>(context.Message);

            var result = await _mediator.Send(command);

            if (result.IsFailure) throw result.Error;
        }
    }
}
