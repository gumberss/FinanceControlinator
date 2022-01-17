using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using Microsoft.Extensions.Logging;
using Payments.Application.Interfaces.AppServices;
using Payments.Data.Repositories;
using Payments.Domain.Interfaces.Validators;
using Payments.Domain.Localizations;
using Payments.Domain.Models;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Payments.Application.AppServices
{
    public class PaymentAppService : IPaymentAppService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IPaymentItemValidator _paymentItemValidator;
        private readonly ILocalization _localization;
        private readonly ILogger<IPaymentAppService> _logger;
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
            _paymentRepository = paymentRepository;
            _paymentItemRepository = paymentItemRepository;
            _paymentItemValidator = paymentItemValidator;
            _localization = localization;
            _logger = logger;
        }

        public async Task<Result<Payment, BusinessException>> ConfirmPayment(String paymentId)
        {
            var payment = await _paymentRepository.GetByIdAsync(paymentId, p => p.ItemId);

            if (payment.IsFailure) return payment.Error;

            if (payment.Value is null)
            {
                var errorData = new ErrorData(_localization.PAYMENT_NOT_FOUND, nameof(paymentId), paymentId);
                return new BusinessException(HttpStatusCode.BadRequest, errorData);
            }

            if (!payment.Value.WaitingForConfirmation())
            {
                var errorData = new ErrorData(_localization.PAYMENT_IS_NOT_WAITING_FOR_CONFIRMATION, nameof(paymentId), paymentId);
                return new BusinessException(HttpStatusCode.BadRequest, errorData);
            }

            var itemPaid = await _paymentItemRepository.GetByIdAsync(payment.Value.ItemId);

            if (itemPaid.IsFailure) return itemPaid.Error;

            payment.Value.Confirm();
            itemPaid.Value.Confirm();

            return payment;
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

            if (itemToPay.Value.Paid())
            {
                var errorData = new ErrorData(_localization.ITEM_ALREADY_WAS_PAID);

                return new BusinessException(HttpStatusCode.BadRequest, errorData);
            }

            if (itemToPay.Value.PaymentAlreadyRequested())
            {
                var errorData = new ErrorData(_localization.PAYMENT_ALREADY_IN_PROCESS);

                return new BusinessException(HttpStatusCode.BadRequest, errorData);
            }

            var payment =
                   new Payment(DateTime.Now)
                   .PaidWith(paymentMethods)
                   .AsRequested()
                   .For(itemToPay.Value)
                   .With(description);

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
