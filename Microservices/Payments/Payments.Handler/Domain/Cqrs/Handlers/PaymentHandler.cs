using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using MediatR;
using Microsoft.Extensions.Logging;
using Payments.Application.Interfaces.AppServices;
using Payments.Domain.Models;
using Payments.Handler.Domain.Cqrs.Events.Commands;
using Raven.Client.Documents.Session;
using System.Threading;
using System.Threading.Tasks;
namespace Payments.Handler.Domain.Cqrs.Handlers
{
    public class PaymentHandler
          : IRequestHandler<RegisterPaymentItemCommand, Result<PaymentItem, BusinessException>>
    {
        private readonly IPaymentAppService _paymentAppService;
        private readonly IAsyncDocumentSession _documentSession;
        private readonly ILogger<PaymentHandler> _logger;

        public PaymentHandler(
            IPaymentAppService paymentAppService
            , IAsyncDocumentSession documentSession
            , ILogger<PaymentHandler> logger)
        {
            _paymentAppService = paymentAppService;
            _documentSession = documentSession;
            _logger = logger;
        }

        public async Task<Result<PaymentItem, BusinessException>> Handle(RegisterPaymentItemCommand request, CancellationToken cancellationToken)
        {
            var registeredPaymentItem = await _paymentAppService.RegisterItem(request.PaymentItem);

            if (registeredPaymentItem.IsFailure) return registeredPaymentItem.Error;

            var saveResult = await Result.Try(_documentSession.SaveChangesAsync());

            if (saveResult.IsFailure) return saveResult.Error;

            return registeredPaymentItem;
        }
    }
}
