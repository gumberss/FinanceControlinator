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
     
    {
        private readonly IPaymentAppService _paymentAppService;
        private readonly ILogger<PaymentHandler> _logger;
        private readonly IAsyncDocumentSession _documentSession;

        public InvoiceHandler(
            IPaymentAppService paymentAppService
            , IAsyncDocumentSession documentSession
            , ILogger<PaymentHandler> logger)
        {
            _paymentAppService = paymentAppService;
            _logger = logger;
            _documentSession = documentSession;
        }

   
    }
}
