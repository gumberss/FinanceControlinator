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
using Raven.Client.Documents.Session;
using Raven.Client.Documents;

namespace Invoices.Application.AppServices
{
    public class InvoiceAppService : IInvoiceAppService
    {
        private readonly IDocumentStore _documentStore;
        private readonly IAsyncDocumentSession _documentSession;

        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IInvoiceValidator _invoiceValidator;
        private readonly ILocalization _localization;
        private readonly ILogger<IInvoiceAppService> _logger;

        public InvoiceAppService(
                IDocumentStore documentStore
                , IAsyncDocumentSession documentSession
                , IInvoiceRepository invoiceRepository
                , IInvoiceValidator invoiceValidator
                , ILocalization localization
                , ILogger<IInvoiceAppService> logger
            )
        {
            _documentStore = documentStore;
            _documentSession = documentSession;
            _invoiceRepository = invoiceRepository;
            _invoiceValidator = invoiceValidator;
            _localization = localization;
            _logger = logger;
        }

        public async Task<Result<List<Expense>, BusinessException>> RegisterNewExpense()
        {
            using (IDocumentSession session = _documentStore.OpenSession())
            {
                session.Query<Expense>();
            }

            dynamic result = null;// await _invoiceRepository.GetAllAsync(include: x => x.Items);

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


        public async Task<Result<List<Expense>, BusinessException>> GetAllInvoices()
        {
            dynamic result = null; //var result = await _invoiceRepository.GetAllAsync(include: x => x.Items);

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

        public async Task<Result<List<Expense>, BusinessException>> GetMonthInvoices()
        {
            var month = DateTime.Now.Month;
            var year = DateTime.Now.Year;

            dynamic result = null; //var result = await _invoiceRepository.GetAllAsync(
                                   //include: e => e.Items
                                   //, e => e.Date.Month == month
                                   //, e => e.Date.Year == year);

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

        public async Task<Result<List<Expense>, BusinessException>> GetLastMonthInvoices()
        {
            var lastMonth = DateTime.Now.AddMonths(-1);
            var month = lastMonth.Month;
            var year = lastMonth.Year;

            dynamic result = null; // //var result = await _invoiceRepository.GetAllAsync(
            //    include: e => e.Items
            //    , e => e.Date.Month == month
            //    , e => e.Date.Year == year);

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

        public async Task<Result<Expense, BusinessException>> RegisterExpense(Expense expense)
        {
            var result = await _invoiceRepository.AddAsync(expense);

            if (result.IsFailure)
            {
                //error
            }

            var saveResult = await Result.Try(_documentSession.SaveChangesAsync());

            if (saveResult.IsFailure)
            {
                //error
            }

            return result.Value;
        }
    }
}
