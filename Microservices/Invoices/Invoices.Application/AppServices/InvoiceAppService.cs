using Invoices.Application.Interfaces.AppServices;
using Invoices.Data.Contexts;
using Invoices.Data.Interfaces.Contexts;
using Invoices.Data.Repositories;
using Invoices.Domain.Models;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Localizations;
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

        public async Task<Result<List<Invoice>, BusinessException>> RegisterExpense(Expense expense)
        {
            var result = await _expenseRepositorty.AddAsync(expense);

            if (result.IsFailure)
            {
                return result.Error;
                //log
            }

            var firstInvoiceDate = _invoiceService.GetCurrentInvoiceDate();

            var lastInvoiceDate = firstInvoiceDate.AddMonths(expense.InstallmentsCount);

            var invoicesResult =
                await _invoiceRepository.GetAllAsync(x => x.Items,
                 x => x.DueDate.Month >= firstInvoiceDate.Month
                   && x.DueDate.Month < lastInvoiceDate.Month
                );

            if (invoicesResult.IsFailure)
            {
                return invoicesResult.Error;
                //log
            }

            var existentInvoices = invoicesResult.Value;

            var changedInvoices = _invoiceService.RegisterExpense(expense, existentInvoices, firstInvoiceDate);

            var newInvoicesResult = changedInvoices.Except(existentInvoices);

            foreach (var newInvoice in newInvoicesResult)
            {
               var addResult =  await _invoiceRepository.AddAsync(newInvoice);

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
    }
}
