using FinanceControlinator.Common.Contexts;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using Invoices.Application.Interfaces.AppServices;
using Invoices.Domain.Models;
using Invoices.Handler.Domain.Cqrs.Events;
using MediatR;
using Microsoft.Extensions.Logging;
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
        private readonly IContext _context;

        public InvoiceHandler(
            IInvoiceAppService invoiceAppService
            , ILogger<InvoiceHandler> logger
            , IContext context
        )
        {
            _invoiceAppService = invoiceAppService;
            _logger = logger;
            _context = context;
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
            => await _invoiceAppService.RegisterPayment(request.Payment)
                .Then(invoice => _context.Commit().Then(_ => invoice));
      
        public async Task<Result<List<Invoice>, BusinessException>> Handle(RegisterExpenseCommand request, CancellationToken cancellationToken)
            => await _invoiceAppService.RegisterInvoiceItems(request.Expense)
                .Then(invoices => _context.Commit().Then(_ => invoices));

    }
}
