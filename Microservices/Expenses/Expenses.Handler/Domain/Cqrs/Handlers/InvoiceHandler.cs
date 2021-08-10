using Expenses.Application.Interfaces.AppServices;
using Expenses.Data.Contexts;
using Expenses.Domain.Models.Expenses;
using Expenses.Domain.Models.Invoices;
using Expenses.Handler.Domain.Cqrs.Events.Invoices;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Handler.Domain.Cqrs.Handlers
{
    public class InvoiceHandler
        : IRequestHandler<ChangeInvoicesCommand, Result<List<Invoice>, BusinessException>>
    {
        private readonly IInvoiceAppService _invoiceAppService;
        private readonly ExpenseDbContext _expenseDbContext;

        public InvoiceHandler(
            IInvoiceAppService invoiceAppService,
            ExpenseDbContext expenseDbContext
            )
        {
            _invoiceAppService = invoiceAppService;
            _expenseDbContext = expenseDbContext;
        }

        public async Task<Result<List<Invoice>, BusinessException>> Handle(ChangeInvoicesCommand request, CancellationToken cancellationToken)
        {
            var changedInvoices = await _invoiceAppService.Change(request.Invoices);

            var saveResult = await Result.Try(_expenseDbContext.SaveChangesAsync());

            if (saveResult.IsFailure)
            {
                //log
                return saveResult.Error;
            }

            return changedInvoices;
        }
    }
}
