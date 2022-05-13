using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using Invoices.Application.Interfaces.AppServices;
using Invoices.Data.Repositories;
using Invoices.Domain.Localizations;
using Invoices.Domain.Models;
using Invoices.Domain.Services;
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
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IExpenseRepository _expenseRepositorty;
        private readonly ILocalization _localization;
        private readonly ILogger<IInvoiceAppService> _logger;
        private readonly IInvoiceService _invoiceService;
        private readonly IPaymentRepository _paymentRepository;

        public InvoiceAppService(
                IInvoiceRepository invoiceRepository
                , IExpenseRepository expenseRepositorty
                , ILocalization localization
                , ILogger<IInvoiceAppService> logger
                , IInvoiceService invoiceService
                , IPaymentRepository paymentRepository
            )
        {
            _invoiceRepository = invoiceRepository;
            _expenseRepositorty = expenseRepositorty;
            _localization = localization;
            _logger = logger;
            _invoiceService = invoiceService;
            _paymentRepository = paymentRepository;
        }

        public async Task<Result<List<Invoice>, BusinessException>> RegisterInvoiceItems(Expense expense)
        {
            var registerExpenseResult = await RegisterExpense(expense);

            if (registerExpenseResult.IsFailure) return registerExpenseResult.Error;

            var now = DateTime.Now;

            var currentInvoiceCloseDate = _invoiceService.GetInvoiceCloseDateBy(now);

            var (invoiceStartSearchDate, lastInvoiceCloseDate) = _invoiceService.GetInvoiceDateRangeByInstallments(expense.InstallmentsCount, now);

            var registeredInvoices =
                await _invoiceRepository.GetAllAsync(x => x.Items,
                    x => x.CloseDate >= invoiceStartSearchDate
                      && x.CloseDate <= lastInvoiceCloseDate);

            if (registeredInvoices.IsFailure) return registeredInvoices.Error;

            var existentInvoices = registeredInvoices.Value;

            var changedInvoices = _invoiceService.RegisterExpense(expense, existentInvoices, currentInvoiceCloseDate);

            var newInvoicesResult = changedInvoices.Except(existentInvoices);

            foreach (var newInvoice in newInvoicesResult)
            {
                var addResult = await _invoiceRepository.AddAsync(newInvoice);

                if (addResult.IsFailure)
                {
                    return new BusinessException(HttpStatusCode.InternalServerError, new ErrorData(addResult.Error.Message));
                }
            }

            return changedInvoices;
        }

        private async Task<Result<Expense, BusinessException>> RegisterExpense(Expense expense)
        {
            var registeredExpense = await _expenseRepositorty.GetByIdAsync(expense.Id);

            if (registeredExpense.IsFailure) return registeredExpense.Error;

            if (registeredExpense.Value is not null)
            {
                //existent expense
                //when we accept changes, this method will be refactored
                return (Expense)null;
            }
            else
            {
                return await _expenseRepositorty.AddAsync(expense);
            }
        }

        public async Task<Result<Invoice, BusinessException>> RegisterPayment(Payment payment)
        {
            var paymentRegistered = await _paymentRepository.GetByIdAsync(payment.Id);

            if (paymentRegistered.IsFailure) return paymentRegistered.Error;

            if (paymentRegistered.Value is not null)
            {
                var errorData = new ErrorData(_localization.PAYMENT_ALREADY_EXISTENT, "PaymentId", payment.Id);

                return new BusinessException(HttpStatusCode.BadRequest, errorData);
            }

            var invoice = await _invoiceRepository.GetByIdAsync(payment.ItemId);

            if (invoice.IsFailure) return invoice.Error;

            if (invoice.Value is null)
            {
                _logger.LogInformation($"The item with id {payment.ItemId} from the payment {payment.Id} was not found. This payment will be skipped");

                return invoice;
            }

            invoice.Value.WasPaidIn(payment.Date);

            await _paymentRepository.AddAsync(payment);

            return invoice;
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
    }
}
