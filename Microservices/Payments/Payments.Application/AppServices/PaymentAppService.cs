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

        public PaymentAppService(
                IAsyncDocumentSession documentSession
                , IPaymentRepository paymentRepository
                , IPaymentValidator paymentValidator
                , ILocalization localization
                , ILogger<IPaymentAppService> logger
            )
        {
            _documentSession = documentSession;
            _paymentRepository = paymentRepository;
            _paymentValidator = paymentValidator;
            _localization = localization;
            _logger = logger;
        }

        public async Task<Result<List<Payment>, BusinessException>> GetAllPayments()
        {
            var result = await _paymentRepository.GetAllAsync(include: x => x.Items);

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
    
    }
}
