using Invoices.Application.Interfaces.AppServices;
using Invoices.Data.Repositories;
using Invoices.Domain.Models;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Raven.Client.Documents.Session;
using Raven.Client.Documents;
using Invoices.Domain.Services;
using System.Linq;
using Invoices.Domain.Localizations;

namespace Invoices.Application.AppServices
{
    public class InvoiceAppService : IInvoiceAppService
    {
        private readonly IDocumentStore _documentStore;
        private readonly IAsyncDocumentSession _documentSession;

        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IExpenseRepository _expenseRepositorty;
        private readonly ILocalization _localization;
        private readonly ILogger<IInvoiceAppService> _logger;
        private readonly IInvoiceService _invoiceService;

        public InvoiceAppService(
                IDocumentStore documentStore
                , IAsyncDocumentSession documentSession
                , IInvoiceRepository invoiceRepository
                , IExpenseRepository expenseRepositorty
                , ILocalization localization
                , ILogger<IInvoiceAppService> logger
                , IInvoiceService invoiceService
            )
        {
            _documentStore = documentStore;
            _documentSession = documentSession;
            _invoiceRepository = invoiceRepository;
            _expenseRepositorty = expenseRepositorty;
            _localization = localization;
            _logger = logger;
            _invoiceService = invoiceService;
        }

        public async Task<Result<List<Invoice>, BusinessException>> GetAllInvoices()
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
                return new BusinessException(HttpStatusCode.NotFound, "");
            }

            return invoices;
        }

        public async Task<Result<List<Invoice>, BusinessException>> GetMonthInvoice()
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
                return new BusinessException(HttpStatusCode.NotFound, "");
            }

            return invoices;
        }

        public async Task<Result<List<Invoice>, BusinessException>> GetLastMonthInvoice()
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
                return new BusinessException(HttpStatusCode.NotFound, "");//_localization.EXPENSES_NOT_FOUND
            }

            return invoices;
        }

        public async Task<Result<List<Invoice>, BusinessException>> RegisterInvoiceItems(Expense expense)
        {
            var registeredExpense = await _expenseRepositorty.GetByIdAsync(expense.Id);

            if (registeredExpense.IsFailure)
            {
                return registeredExpense.Error;
            }

            if (registeredExpense.Value is not null)
            {
                //existent expense
            }
            else
            {
                var addExpenseResult = await _expenseRepositorty.AddAsync(expense);

                if (addExpenseResult.IsFailure)
                {
                    return addExpenseResult.Error;
                }
            }

            var firstInvoiceCloseDate = _invoiceService.GetCurrentInvoiceCloseDate();

            var lastInvoiceCloseDate = firstInvoiceCloseDate.AddMonths(expense.InstallmentsCount);

            var invoiceStartSearchDate = new DateTime(firstInvoiceCloseDate.Year, firstInvoiceCloseDate.Month, DateTime.Now.Day);

            var registeredInvoices =
                await _invoiceRepository.GetAllAsync(x => x.Items,
                    x => x.CloseDate >= invoiceStartSearchDate
                      && x.CloseDate <= lastInvoiceCloseDate
                );

            if (registeredInvoices.IsFailure)
            {
                return registeredInvoices.Error;
                //log
            }

            var existentInvoices = registeredInvoices.Value;

            var changedInvoices = _invoiceService.RegisterExpense(expense, existentInvoices, firstInvoiceCloseDate);

            var newInvoicesResult = changedInvoices.Except(existentInvoices);

            foreach (var newInvoice in newInvoicesResult)
            {
                var addResult = await _invoiceRepository.AddAsync(newInvoice);

                if (addResult.IsFailure)
                {
                    //log
                    return new BusinessException(HttpStatusCode.InternalServerError, new ErrorData(addResult.Error.Message));
                }
            }


            var saveResult = await Result.Try(_documentSession.SaveChangesAsync());

            if (saveResult.IsFailure)
            {
                return saveResult.Error;
                //log
            }

            return changedInvoices;
        }

        public Task<Result<Invoice, BusinessException>> Pay(Invoice invoice)
        {
            //invoice.WasPaidIn();
            //invoice.update
            return null;
        }
    }
}
