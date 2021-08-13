﻿using AutoMapper;
using Expenses.Handler.Domain.Cqrs.Events.Invoices;
using FinanceControlinator.Events.Invoices;
using MassTransit;
using MediatR;
using System;
using System.Threading.Tasks;

namespace Expenses.Handler.Integration.Handlers.Invoices
{
    class InvoiceIntegrationHandler : IConsumer<InvoicePaidEvent>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public InvoiceIntegrationHandler(
            IMapper mapper,
            IMediator mediator
            )
        {
            _mapper = mapper;
            _mediator = mediator;
        }
        public async Task Consume(ConsumeContext<InvoicePaidEvent> context)
        {
            var command = _mapper.Map<RegisterPaidInvoiceCommand>(context.Message);

            var result = await _mediator.Send(command);

            if (result.IsFailure)
            {
                //log
                throw result.Error;
            }
        }
    }
}
