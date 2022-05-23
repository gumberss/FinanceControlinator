using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using Invoices.Application.Interfaces.AppServices;
using Invoices.Domain.Models;
using Invoices.Handler.Domain.Cqrs.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using Raven.Client.Documents.Session;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Invoices.Handler.Domain.Cqrs.Handlers
{
    public class InvoiceHandler
        : IRequestHandler<GetAllInvoicesQuery, Result<List<Invoice>, BusinessException>>
        , IRequestHandler<GetMonthInvoicesQuery, Result<List<Invoice>, BusinessException>>
        , IRequestHandler<GetLastMonthInvoicesQuery, Result<List<Invoice>, BusinessException>>
        , IRequestHandler<RegisterInvoicePaymentCommand, Result<Invoice, BusinessException>>
        , IRequestHandler<RegisterExpenseCommand, Result<List<Invoice>, BusinessException>>

    {
        private readonly IInvoiceAppService _invoiceAppService;
        private readonly ILogger<InvoiceHandler> _logger;
        private readonly IAsyncDocumentSession _documentSession;

        public InvoiceHandler(
            IInvoiceAppService invoiceAppService
            , ILogger<InvoiceHandler> logger
            , IAsyncDocumentSession documentSession
        )
        {
            _invoiceAppService = invoiceAppService;
            _logger = logger;
            _documentSession = documentSession;
        }

        public async Task<Result<List<Invoice>, BusinessException>> Handle(GetAllInvoicesQuery request, CancellationToken cancellationToken)
        {
            return await _invoiceAppService.GetAllInvoices();
        }

        public async Task<Result<List<Invoice>, BusinessException>> Handle(GetMonthInvoicesQuery request, CancellationToken cancellationToken)
        {
            return await _invoiceAppService.GetMonthInvoice();
        }

        public async Task<Result<List<Invoice>, BusinessException>> Handle(GetLastMonthInvoicesQuery request, CancellationToken cancellationToken)
        {
            return await _invoiceAppService.GetLastMonthInvoice();
        }

        public async Task<Result<Invoice, BusinessException>> Handle(RegisterInvoicePaymentCommand request, CancellationToken cancellationToken)
        {
            var result = await _invoiceAppService.RegisterPayment(request.Payment);

            if (result.IsFailure) return result.Error;

            var saveResult = await Result.Try(_documentSession.SaveChangesAsync());

            if (saveResult.IsFailure) return saveResult.Error;

            return result;
        }

        public async Task<Result<List<Invoice>, BusinessException>> Handle(RegisterExpenseCommand request, CancellationToken cancellationToken)
        {
            var result = await _invoiceAppService.RegisterInvoiceItems(request.Expense);

            if (result.IsFailure) return result.Error;

            var saveResult = await Result.Try(_documentSession.SaveChangesAsync());

            if (saveResult.IsFailure) return saveResult.Error;

            return result;
        }
    }
}
