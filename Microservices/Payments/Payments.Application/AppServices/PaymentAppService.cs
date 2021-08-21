using Payments.Application.Interfaces.AppServices;
using Payments.Data.Repositories;
using Payments.Domain.Interfaces.Validators;
using Payments.Domain.Models;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Raven.Client.Documents.Session;
using Payments.Domain.Localizations;
using Payments.Domain.Enums;
using Raven.Client.Documents.Linq;

namespace Payments.Application.AppServices
{
    public class PaymentAppService : IPaymentAppService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IPaymentItemValidator _paymentItemValidator;
        private readonly ILocalization _localization;
        private readonly ILogger<IPaymentAppService> _logger;
        private readonly IAsyncDocumentSession _documentSession;
        private readonly IPaymentItemRepository _paymentItemRepository;

        public PaymentAppService(
                IAsyncDocumentSession documentSession
                , IPaymentRepository paymentRepository
                , IPaymentItemRepository paymentItemRepository
                , IPaymentItemValidator paymentItemValidator
                , ILocalization localization
                , ILogger<IPaymentAppService> logger
            )
        {
            _documentSession = documentSession;
            _paymentRepository = paymentRepository;
            _paymentItemRepository = paymentItemRepository;
            _paymentItemValidator = paymentItemValidator;
            _localization = localization;
            _logger = logger;

        }

        public async Task<Result<List<PaymentItem>, BusinessException>> GetClosedItems()
        {
            return await _paymentItemRepository.GetAllAsync(null, x => x.CloseDate < DateTime.Now);
        }

        public async Task<Result<Payment, BusinessException>> Pay(string itemId, string description, List<PaymentMethod> paymentMethods)
        {
            var itemToPay = await _paymentItemRepository.GetByIdAsync(itemId, x => x.PaymentIds);

            if (itemToPay.IsFailure) return itemToPay.Error;

            if (itemToPay.Value is null)
            {
                var errorData = new ErrorData(_localization.ITEM_NOT_FOUND);
                return new BusinessException(HttpStatusCode.NotFound, errorData);
            }

            var validationResult = await _paymentItemValidator.ValidateAsync(itemToPay);

            if (!validationResult.IsValid)
            {
                var errorDatas = validationResult.Errors.Select(x => new ErrorData(x.ErrorMessage, x.PropertyName));
                var exception = new BusinessException(HttpStatusCode.BadRequest, errorDatas);

                _logger.LogInformation(exception.Log());

                return exception;
            }

            var paymentsAlreadyRegistered = await _paymentRepository.GetAllAsync(null, x => x.Id.In(itemToPay.Value.PaymentIds));

            if (paymentsAlreadyRegistered.IsFailure)
            {
                return paymentsAlreadyRegistered.Error;
            }

            if (paymentsAlreadyRegistered.Value.Any(x => x.InProcess()))
            {
                var errorData = new ErrorData(_localization.PAYMENT_ALREADY_IN_PROCESS);

                return new BusinessException(HttpStatusCode.BadRequest, errorData);
            }

            if (paymentsAlreadyRegistered.Value.Any(x => x.Paid()))
            {
                var errorData = new ErrorData(_localization.ITEM_ALREADY_WAS_PAID);

                return new BusinessException(HttpStatusCode.BadRequest, errorData);
            }

            var payment =
                   new Payment(DateTime.Now)
                   .For(itemToPay.Value)
                   .PaidWith(paymentMethods)
                   .With(description)
                   .AsRequested();

            return await _paymentRepository.AddAsync(payment);
        }

        public async Task<Result<PaymentItem, BusinessException>> RegisterItem(PaymentItem paymentItem)
        {
            var registeredItem = await _paymentItemRepository.GetByIdAsync(paymentItem.Id);

            if (registeredItem.IsFailure)
            {
                //log
                return registeredItem.Error;
            }

            if (registeredItem.Value is not null)
            {
                if (registeredItem.Value.CanUpdate())
                    registeredItem.Value.UpdateFrom(paymentItem);
                else
                {
                    var errorData = new ErrorData("It's not possible to update the paymentItem", paymentItem.Id);
                    return new BusinessException(HttpStatusCode.BadRequest, errorData);
                }

                return registeredItem.Value;
            }
            else
            {
                return await _paymentItemRepository.AddAsync(paymentItem);
            }
        }
    }
}
