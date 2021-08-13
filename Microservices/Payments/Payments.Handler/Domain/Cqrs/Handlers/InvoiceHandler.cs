using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using MediatR;
using Microsoft.Extensions.Logging;
using Payments.Application.Interfaces.AppServices;
using Payments.Domain.Models;
using Payments.Handler.Domain.Cqrs.Events.Commands;
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

        public InvoiceHandler(
            IInvoiceAppService invoiceAppService
            , ILogger<PaymentHandler> logger)
        {
            _invoiceAppService = invoiceAppService;
            _logger = logger;
        }

        async Task<Result<List<Invoice>, BusinessException>> IRequestHandler<AddOrUpdateInvoicesCommand, Result<List<Invoice>, BusinessException>>.Handle(AddOrUpdateInvoicesCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
