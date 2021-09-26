using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using Invoices.Application.Interfaces.AppServices;
using Invoices.Domain.Models;
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
        private readonly ILogger<InvoiceHandler> _logger;
        private readonly IAsyncDocumentSession _documentSession;

        public PiggyBankHandler(
            IInvoiceAppService invoiceAppService
            , ILogger<InvoiceHandler> logger
            , IAsyncDocumentSession documentSession
        )
        {
            _invoiceAppService = invoiceAppService;
            _logger = logger;
            _documentSession = documentSession;
        }

        public Task<Result<List<Invoice>, BusinessException>> Handle(RegisterPiggyBankExpenseCommand request, CancellationToken cancellationToken)
        {
            return default;
            //var result = await _invoiceAppService.RegisterInvoiceItems(request.Expense);

            //if (result.IsFailure) return result.Error;

            //var saveResult = await Result.Try(_documentSession.SaveChangesAsync());

            //if (saveResult.IsFailure) return saveResult.Error;

            //return result;
        }
    }
}
