using Payments.Application.Interfaces.AppServices;
using Payments.Data.Repositories;
using Payments.Domain.Interfaces.Validators;
using Payments.Domain.Models;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Localizations;
using FinanceControlinator.Common.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Raven.Client.Documents.Session;

namespace Payments.Application.AppServices
{
    public class PaymentAppService : IPaymentAppService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IPaymentValidator _paymentValidator;
        private readonly ILocalization _localization;
        private readonly ILogger<IPaymentAppService> _logger;
        private readonly IAsyncDocumentSession _documentSession;
        private readonly IPaymentItemRepository _paymentItemRepository;

        public PaymentAppService(
                IAsyncDocumentSession documentSession
                , IPaymentRepository paymentRepository
                , IPaymentItemRepository paymentItemRepository
                , IPaymentValidator paymentValidator
                , ILocalization localization
                , ILogger<IPaymentAppService> logger
            )
        {
            _documentSession = documentSession;
            _paymentRepository = paymentRepository;
            _paymentItemRepository = paymentItemRepository;
            _paymentValidator = paymentValidator;
            _localization = localization;
            _logger = logger;

        }

        public async Task<Result<List<Payment>, BusinessException>> GetAllPayments()
        {
            var result = await _paymentRepository.GetAllAsync(include: x => x.PaymentMethods);

            if (result.IsFailure)
            {
                //log
                return result.Error;
            }

            var payments = result.Value;

            if (!payments.Any())
            {
                return new BusinessException(HttpStatusCode.NotFound, _localization.EXPENSES_NOT_FOUND);
            }

            return payments;
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
