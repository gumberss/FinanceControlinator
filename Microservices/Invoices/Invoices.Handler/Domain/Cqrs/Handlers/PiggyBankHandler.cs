using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using Invoices.Application.Interfaces.AppServices;
using Invoices.Domain.Models;
using Invoices.Domain.Services;
using Invoices.Handler.Domain.Cqrs.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Invoices.Handler.Domain.Cqrs.Handlers
{
    public class PiggyBankHandler :
         IRequestHandler<RegisterPiggyBankExpenseCommand, Result<List<Invoice>, BusinessException>>
    {
        private readonly IInvoiceAppService _invoiceAppService;
        private readonly IInvoiceService _invoiceService;
        private readonly ILogger<InvoiceHandler> _logger;
        private readonly IAsyncDocumentSession _documentSession;

        public PiggyBankHandler(
            IInvoiceAppService invoiceAppService,
            IInvoiceService invoiceService
            , ILogger<InvoiceHandler> logger
            , IAsyncDocumentSession documentSession
        )
        {
            _invoiceAppService = invoiceAppService;
            _invoiceService = invoiceService;
            _logger = logger;
            _documentSession = documentSession;
        }

        public async Task<Result<List<Invoice>, BusinessException>> Handle(RegisterPiggyBankExpenseCommand request, CancellationToken cancellationToken)
        {
            var piggyBank = request.Expense;

            var installmentCount = _invoiceService.GetInvoiceInstallmentsByDateRange(piggyBank.StartDate, piggyBank.GoalDate);

            var expense = new Expense()
                .From(piggyBank)
                .WithInstallmentCount(installmentCount);

            return await _invoiceAppService.RegisterInvoiceItems(expense);
        }
    }
}
