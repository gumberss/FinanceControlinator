using AutoMapper;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Messaging;
using FinanceControlinator.Common.Utils;
using FinanceControlinator.Events.Payments;
using MediatR;
using Microsoft.Extensions.Logging;
using Payments.Application.Interfaces.AppServices;
using Payments.Domain.Models;
using Payments.Handler.Domain.Cqrs.Events.Commands;
using Payments.Handler.Domain.Cqrs.Events.Queries;
using Raven.Client.Documents.Session;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
namespace Payments.Handler.Domain.Cqrs.Handlers
{
    public class PaymentHandler
          : IRequestHandler<RegisterPaymentItemCommand, Result<PaymentItem, BusinessException>>
          , IRequestHandler<PayItemCommand, Result<Payment, BusinessException>>
          , IRequestHandler<ClosedItemsQuery, Result<List<PaymentItem>, BusinessException>>
    {
        private readonly IPaymentAppService _paymentAppService;
        private readonly IAsyncDocumentSession _documentSession;
        private readonly ILogger<PaymentHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IMessageBus _bus;

        public PaymentHandler(
            IPaymentAppService paymentAppService
            , IAsyncDocumentSession documentSession
            , ILogger<PaymentHandler> logger
            , IMapper mapper
            , IMessageBus bus)
        {
            _paymentAppService = paymentAppService;
            _documentSession = documentSession;
            _logger = logger;
            _mapper = mapper;
            _bus = bus;
        }

        public async Task<Result<PaymentItem, BusinessException>> Handle(RegisterPaymentItemCommand request, CancellationToken cancellationToken)
        {
            var registeredPaymentItem = await _paymentAppService.RegisterItem(request.PaymentItem);

            if (registeredPaymentItem.IsFailure) return registeredPaymentItem.Error;

            var saveResult = await Result.Try(_documentSession.SaveChangesAsync());

            if (saveResult.IsFailure) return saveResult.Error;

            return registeredPaymentItem;
        }

        public async Task<Result<Payment, BusinessException>> Handle(PayItemCommand request, CancellationToken cancellationToken)
        {
            var paymentMethods = _mapper.Map<List<PaymentMethod>>(request.PaymentMethods);

            var paymentCreated = await _paymentAppService.Pay(request.ItemId.ToString(), request.Description, paymentMethods);

            if (paymentCreated.IsFailure) return paymentCreated.Error;

            var saveResult = await Result.Try(_documentSession.SaveChangesAsync());

            if (saveResult.IsFailure) return saveResult.Error;

            var @event = _mapper.Map<Payment, PaymentRequestedEvent>(paymentCreated);

            await _bus.Publish(@event);

            return paymentCreated;
        }

        public async Task<Result<List<PaymentItem>, BusinessException>> Handle(ClosedItemsQuery request, CancellationToken cancellationToken)
        {
            var closedItems = await _paymentAppService.GetClosedItems();

            if (closedItems.IsFailure) return closedItems.Error;

            return closedItems;
        }
    }
}
