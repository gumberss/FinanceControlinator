using CleanHandling;
using Expenses.Application.Interfaces.AppServices;
using Expenses.Data.Repositories;
using Expenses.Domain.Interfaces.Services;
using Expenses.Domain.Interfaces.Validators;
using Expenses.Domain.Localizations;
using Expenses.Domain.Models.Expenses;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Expenses.Application.AppServices
{
    public class ExpenseAppService : IExpenseAppService
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IExpenseValidator _expenseValidator;
        private readonly ILocalization _localization;
        private readonly ILogger<IExpenseAppService> _logger;
        private readonly IExpenseService _expenseService;

        public ExpenseAppService(
                IExpenseRepository expenseRepository
                , IInvoiceRepository invoiceRepository
                , IExpenseValidator expenseValidator
                , ILocalization localization
                , ILogger<IExpenseAppService> logger
                , IExpenseService expenseService
            )
        {
            _expenseRepository = expenseRepository;
            _invoiceRepository = invoiceRepository;
            _expenseValidator = expenseValidator;
            _localization = localization;
            _logger = logger;
            _expenseService = expenseService;
        }

        public async Task<Result<List<Expense>, BusinessException>> GetAllExpenses()
        {
            var result = await _expenseRepository.GetAllAsync(include: x => x.Items);

            if (result.IsFailure)
            {
                //log
                return result.Error;
            }

            var expenses = result.Value;

            if (!expenses.Any())
            {
                return new BusinessException(HttpStatusCode.NotFound, _localization.EXPENSES_NOT_FOUND);
            }

            return expenses;
        }

        public async Task<Result<List<Expense>, BusinessException>> GetMonthExpenses()
        {
            var month = DateTime.Now.Month;
            var year = DateTime.Now.Year;

            var result = await _expenseRepository.GetAllAsync(
                include: e => e.Items
                , e => e.PurchaseDate.Month == month
                , e => e.PurchaseDate.Year == year);

            if (result.IsFailure)
            {
                //log
                return result.Error;
            }

            var expenses = result.Value;

            if (!expenses.Any())
            {
                return new BusinessException(HttpStatusCode.NotFound, _localization.EXPENSES_NOT_FOUND);
            }

            return expenses;
        }

        public async Task<Result<List<Expense>, BusinessException>> GetLastMonthExpenses()
        {
            var lastMonth = DateTime.Now.AddMonths(-1);
            var month = lastMonth.Month;
            var year = lastMonth.Year;

            var result = await _expenseRepository.GetAllAsync(
                include: e => e.Items
                , e => e.PurchaseDate.Month == month
                , e => e.PurchaseDate.Year == year);

            if (result.IsFailure)
            {
                //log
                return result.Error;
            }

            var expenses = result.Value;

            if (!expenses.Any())
            {
                return new BusinessException(HttpStatusCode.NotFound, _localization.EXPENSES_NOT_FOUND);
            }

            return expenses;
        }

        public async Task<Result<Expense, BusinessException>> RegisterExpense(Expense expense)
        {
            var validationResult = await _expenseValidator.ValidateAsync(expense);

            if (!validationResult.IsValid)
            {
                var errorDatas = validationResult.Errors.Select(x => new ErrorData(x.ErrorMessage, x.PropertyName));
                var exception = new BusinessException(HttpStatusCode.BadRequest, errorDatas);
                _logger.LogInformation(exception.Log());

                return exception;
            }

            var addResult = await _expenseRepository.AddAsync(expense);

            if (addResult.IsFailure)
            {
                var errorData = new ErrorData(_localization.AN_ERROR_OCCURRED_ON_THE_SERVER);
                var exception = new BusinessException(HttpStatusCode.InternalServerError, errorData, addResult.Error);
                _logger.LogError(addResult.Error, exception.Log());

                return exception;
            }

            return addResult;
        }

        public async Task<Result<Expense, BusinessException>> UpdateExpense(Expense expense)
        {
            var validationResult = await _expenseValidator.ValidateAsync(expense);

            if (!validationResult.IsValid)
            {
                var errorDatas = validationResult.Errors.Select(x => new ErrorData(x.ErrorMessage, x.PropertyName));
                var exception = new BusinessException(HttpStatusCode.BadRequest, errorDatas);
                _logger.LogInformation(exception.Log());
                return exception;
            }

            //TODO: filter by user when this functionality is added
            var registeredExpense = await _expenseRepository.GetByIdAsync(expense.Id, exp => exp.Items);

            if (registeredExpense.IsFailure) return registeredExpense.Error;

            if (registeredExpense.Value is null)
            {
                var errorData = new ErrorData(_localization.EXPENSE_NOT_FOUND, "ExpenseId", expense.Id.ToString());
                var exception = new BusinessException(HttpStatusCode.BadRequest, errorData);
                _logger.LogInformation(exception.Log());
                return exception;
            }

            var invoicesWithExpenseCosts = await _invoiceRepository.GetAllAsync(
                    where: inv => inv.Items.Any(i => i.ExpenseId == expense.Id),
                    include: inv => inv.Items
                );

            if (invoicesWithExpenseCosts.IsFailure) return invoicesWithExpenseCosts.Error;

            bool newTotalCostIsValid = _expenseService.IsTotalCostValid(expense, invoicesWithExpenseCosts);

            if (!newTotalCostIsValid)
            {
                var errorData = new ErrorData(_localization.EXPENSE_COST_IS_LESS_THAN_WHAT_WAS_PAID, "ExpenseId", expense.Id.ToString());
                var exception = new BusinessException(HttpStatusCode.BadRequest, errorData);

                _logger.LogInformation(exception.Log());

                return exception;
            }

            if (!_expenseService.InstallmentsCountIsValid(expense, invoicesWithExpenseCosts))
            {
                var errorData = new ErrorData(_localization.EXPENSE_INSTALLMENTS_IS_LESS_THAN_TIMES_PAID, "ExpenseId", expense.Id.ToString());
                var exception = new BusinessException(HttpStatusCode.BadRequest, errorData);

                _logger.LogInformation(exception.Log());

                return exception;
            }

            var (toAdd, toUpdate, toDelete) = _expenseService.SegregateItems(expense, registeredExpense);

            registeredExpense.Value
                .ChangeTotalCost(expense.TotalCost)
                .UpdateItems(toUpdate)
                .AddItems(toAdd)
                .RemoveItems(toDelete);

            return registeredExpense;
        }

        public Task<Result<List<Expense>, BusinessException>> GetByPagination(int page, int count, Guid userId)
        {
            return _expenseRepository.GetPaginationAsync(page, count, userId);
        }
    }
}
