using Invoices.Application.Interfaces.AppServices;
using Invoices.Data.Contexts;
using Invoices.Data.Interfaces.Contexts;
using Invoices.Data.Repositories;
using Invoices.Domain.Interfaces.Validators;
using Invoices.Domain.Models;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Localizations;
using FinanceControlinator.Common.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Invoices.Application.AppServices
{
    public class InvoiceAppService : IInvoiceAppService
    {
        private readonly IInvoiceDbContext _invoiceDbContext;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IInvoiceValidator _invoiceValidator;
        private readonly ILocalization _localization;
        private readonly ILogger<IInvoiceAppService> _logger;

        public InvoiceAppService(
                InvoiceDbContext invoiceDbContext
                , IInvoiceRepository invoiceRepository
                , IInvoiceValidator invoiceValidator
                , ILocalization localization
                , ILogger<IInvoiceAppService> logger
            )
        {
            _invoiceDbContext = invoiceDbContext;
            _invoiceRepository = invoiceRepository;
            _invoiceValidator = invoiceValidator;
            _localization = localization;
            _logger = logger;
        }

        public async Task<Result<List<Invoice>, BusinessException>> GetAllInvoices()
        {
            var result = await _invoiceRepository.GetAllAsync(include: x => x.Items);

            if (result.IsFailure)
            {
                //log
                return result.Error;
            }

            var invoices = result.Value;

            if (!invoices.Any())
            {
                return new BusinessException(HttpStatusCode.NotFound, _localization.EXPENSES_NOT_FOUND);
            }

            return invoices;
        }
        
        public async Task<Result<List<Invoice>, BusinessException>> GetMonthInvoices()
        {
            var month = DateTime.Now.Month;
            var year = DateTime.Now.Year;

            var result = await _invoiceRepository.GetAllAsync(
                include: e => e.Items
                , e => e.Date.Month == month
                , e => e.Date.Year == year);

            if (result.IsFailure)
            {
                //log
                return result.Error;
            }

            var invoices = result.Value;

            if (!invoices.Any())
            {
                return new BusinessException(HttpStatusCode.NotFound, _localization.EXPENSES_NOT_FOUND);
            }

            return invoices;
        }

        public async Task<Result<List<Invoice>, BusinessException>> GetLastMonthInvoices()
        {
            var lastMonth = DateTime.Now.AddMonths(-1);
            var month = lastMonth.Month;
            var year = lastMonth.Year;

            var result = await _invoiceRepository.GetAllAsync(
                include: e => e.Items
                , e => e.Date.Month == month
                , e => e.Date.Year == year);

            if (result.IsFailure)
            {
                //log
                return result.Error;
            }

            var invoices = result.Value;

            if (!invoices.Any())
            {
                return new BusinessException(HttpStatusCode.NotFound, _localization.EXPENSES_NOT_FOUND);
            }

            return invoices;
        }

        public async Task<Result<Invoice, BusinessException>> RegisterInvoice(Invoice invoice)
        {
            var validationResult = await _invoiceValidator.ValidateAsync(invoice);

            if (!validationResult.IsValid)
            {
                var errorDatas = validationResult.Errors.Select(x => new ErrorData(x.ErrorMessage, x.PropertyName));
                var exception = new BusinessException(HttpStatusCode.BadRequest, errorDatas);

                _logger.LogInformation(exception.Log());

                return exception;
            }

            if (!invoice.TotalCostIsValid())
            {
                var errorData = new ErrorData(_localization.TOTAL_COST_DOES_NOT_MATCH_WITH_ITEMS, "TotalCost");
                var exception = new BusinessException(HttpStatusCode.BadRequest, errorData);

                _logger.LogInformation(exception.Log());

                return exception;
            }

            var addResult = await _invoiceRepository.AddAsync(invoice);

            if (addResult.IsFailure)
            {
                var errorData = new ErrorData(_localization.AN_ERROR_OCCURRED_ON_THE_SERVER);
                var exception = new BusinessException(HttpStatusCode.InternalServerError, errorData);

                _logger.LogError(exception.Log());

                return exception;
            }

            var saveResult = await Result.Try<int, Exception>(_invoiceDbContext.Commit());

            if (saveResult.IsFailure)
            {
                var errorData = new ErrorData(_localization.AN_ERROR_OCCURRED_ON_THE_SERVER);
                var exception = new BusinessException(HttpStatusCode.InternalServerError, errorData);

                _logger.LogError(exception.Log());

                return exception;
            }

            return addResult;
        }
    }
}
