using AutoMapper;
using FinanceControlinator.Events.Expenses;
using FinanceControlinator.Events.Invoices;
using Invoices.Application.Interfaces.AppServices;
using Invoices.Domain.Models;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Invoices.Handler.Integration.Handlers.Expenses
{
    public class ExpenseIntegrationHandler : IConsumer<ExpenseCreatedEvent>
    {
        private readonly IInvoiceAppService _invoiceAppService;
        private readonly IMapper _mapper;

        public ExpenseIntegrationHandler(
            IInvoiceAppService invoiceAppService,
            IMapper mapper
            )
        {
            _invoiceAppService = invoiceAppService;
            _mapper = mapper;
        }
        public async Task Consume(ConsumeContext<ExpenseCreatedEvent> context)
        {
            var expense = _mapper.Map<Expense>(context.Message.Expense);

            var result =  await _invoiceAppService.RegisterExpense(expense);

            if (result.IsFailure) throw result.Error;

            //log

            if (result.IsSuccess)
            {
                var invoices = result.Value;

                var @event = _mapper.Map<InvoicesChangedEvent>(invoices);

                await context.Publish(@event);
            }
        }
    }
}
