using AutoMapper;
using FinanceControlinator.Events.Invoices;
using MassTransit;
using MediatR;
using Payments.Handler.Domain.Cqrs.Events;
using Payments.Handler.Domain.Cqrs.Events.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments.Handler.Integration.Handlers
{
    public class InvoicesIntegrationHandler : IConsumer<InvoicesChangedEvent>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public InvoicesIntegrationHandler(
            IMapper mapper,
            IMediator mediator
            )
        {
            _mapper = mapper;
            _mediator = mediator;
        }
        public async Task Consume(ConsumeContext<InvoicesChangedEvent> context)
        {
            var command = _mapper.Map<AddOrUpdateInvoicesCommand>(context.Message);

            var result = await _mediator.Send(command);

            if (result.IsFailure)
            {
                //log
                throw result.Error;
            }
        }
    }
}
