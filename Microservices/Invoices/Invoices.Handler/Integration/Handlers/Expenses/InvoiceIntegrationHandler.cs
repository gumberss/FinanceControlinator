using AutoMapper;
using FinanceControlinator.Events.Invoices;
using FinanceControlinator.Events.Invoices;
using FinanceControlinator.Events.Payments;
using Invoices.Application.Interfaces.AppServices;
using Invoices.Domain.Models;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Invoices.Handler.Integration.Handlers.Expenses
{
    public class InvoiceIntegrationHandler : IConsumer<GenerateInvoicesEvent>
    {
        private readonly IInvoiceAppService _invoiceAppService;
        private readonly IMapper _mapper;

        public InvoiceIntegrationHandler(
            IInvoiceAppService invoiceAppService,
            IMapper mapper
            )
        {
            _invoiceAppService = invoiceAppService;
            _mapper = mapper;
        }
        public async Task Consume(ConsumeContext<GenerateInvoicesEvent> context)
        {
            var expense = _mapper.Map<Expense>(context.Message.InvoiceExpense);

            var result =  await _invoiceAppService.RegisterInvoiceItems(expense);

            if (result.IsFailure) throw result.Error;

            //log

            if (result.IsSuccess)
            {
                var invoices = result.Value;

                var @event = _mapper.Map<InvoicesChangedEvent>(invoices);

                foreach (var invoice in invoices)
                {
                    var paymentEvent = _mapper.Map<RegisterItemToPayEvent>(invoice);
                    await context.Publish(paymentEvent);
                }

                await context.Publish(@event);
            }
        }
    }
}
