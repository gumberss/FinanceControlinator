using AutoMapper;
using FinanceControlinator.Events.Expenses;
using Invoices.Application.Interfaces.AppServices;
using Invoices.Domain.Models;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Invoices.Handler.Integration.Handlers.Expenses
{
    public class ExpenseHandler : IConsumer<ExpenseCreatedEvent>
    {
        private readonly IInvoiceAppService _invoiceAppService;
        private readonly IMapper _mapper;

        public ExpenseHandler(
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
        }
    }
}
