using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using MediatR;
using Microsoft.Extensions.Logging;
using Payments.Application.Interfaces.AppServices;
using Payments.Domain.Models;
using Payments.Handler.Domain.Cqrs.Events.Commands;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Payments.Handler.Domain.Cqrs.Handlers
{
    public class InvoiceHandler
        : IRequestHandler<AddOrUpdateInvoicesCommand, Result<List<Invoice>, BusinessException>>
    {
        private readonly IInvoiceAppService _invoiceAppService;
        private readonly ILogger<PaymentHandler> _logger;
        private readonly IAsyncDocumentSession _documentSession;

        public InvoiceHandler(
            IInvoiceAppService invoiceAppService
            , IAsyncDocumentSession documentSession
            , ILogger<PaymentHandler> logger)
        {
            _invoiceAppService = invoiceAppService;
            _logger = logger;
            _documentSession = documentSession;
        }

        public async Task<Result<List<Invoice>, BusinessException>> Handle(AddOrUpdateInvoicesCommand request, CancellationToken cancellationToken)
        {
           var changedInvoices =  await _invoiceAppService.Change(request.Invoices);

            var result = await Result.Try(_documentSession.SaveChangesAsync());

            if (result.IsFailure)
            {
                //log

                return result.Error;
            }

            return changedInvoices;
        }
    }
}
