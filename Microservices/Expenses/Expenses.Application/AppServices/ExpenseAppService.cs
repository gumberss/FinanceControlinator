using Expenses.Application.Interfaces.AppServices;
using Expenses.Data.Contexts;
using Expenses.Data.Interfaces.Contexts;
using Expenses.Data.Repositories;
using Expenses.Domain.Interfaces.Validators;
using Expenses.Domain.Models;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Localizations;
using FinanceControlinator.Common.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Expenses.Application.AppServices
{
    public class ExpenseAppService : IExpenseAppService
    {
        private readonly IExpenseDbContext _expenseDbContext;
        private readonly IExpenseRepository _expenseRepository;
        private readonly IExpenseValidator _expenseValidator;
        private readonly ILocalization _localization;
        private readonly ILogger<IExpenseAppService> _logger;

        public ExpenseAppService(
                ExpenseDbContext expenseDbContext
                , IExpenseRepository expenseRepository
                , IExpenseValidator expenseValidator
                , ILocalization localization
                , ILogger<IExpenseAppService> logger
            )
        {
            _expenseDbContext = expenseDbContext;
            _expenseRepository = expenseRepository;
            _expenseValidator = expenseValidator;
            _localization = localization;
            _logger = logger;
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

            if (!expense.TotalCostIsValid())
            {
                var errorData = new ErrorData(_localization.TOTAL_COST_DOES_NOT_MATCH_WITH_ITEMS, "TotalCost");
                var exception = new BusinessException(HttpStatusCode.BadRequest, errorData);

                _logger.LogInformation(exception.Log());

                return exception;
            }

            var addResult = await _expenseRepository.AddAsync(expense);

            if (addResult.IsFailure)
            {
                var errorData = new ErrorData(_localization.AN_ERROR_OCCURRED_ON_THE_SERVER);
                var exception = new BusinessException(HttpStatusCode.InternalServerError, errorData);

                _logger.LogError(exception.Log());

                return exception;
            }

            var saveResult = await Result.Try<int, Exception>(_expenseDbContext.Commit());

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
