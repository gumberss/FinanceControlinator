using Expenses.Application.Interfaces.AppServices;
using Expenses.Data.Repositories;
using Expenses.Domain.Enums;
using Expenses.Domain.Interfaces.Services;
using Expenses.Domain.Localizations;
using Expenses.Domain.Models.Expenses;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Parsers.TextParsers;
using FinanceControlinator.Common.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Expenses.Application.AppServices
{
    public class ExpenseOverviewAppService : IExpenseOverviewAppService
    {
        private readonly IDateService _dateService;
        private readonly IExpenseRepository _expenseRepository;
        private readonly IExpenseOverviewService _expenseService;
        private readonly ILocalization _localization;
        private readonly ITextParser _textParser;

        public ExpenseOverviewAppService(IDateService dateService,
            IExpenseRepository expenseRepository,
            ITextParser textParser,
            ILocalization localization,
            IExpenseOverviewService expenseService)
        {
            _dateService = dateService;
            _expenseRepository = expenseRepository;
            _expenseService = expenseService;
            _localization = localization;
            _textParser = textParser;
        }

        public async Task<Result<List<ExpenseOverview>, BusinessException>> GetExpensesOverview()
        {
            var (startDate, endDate) = _dateService.StartAndEndMonthDate(DateTime.Now);

            var expenses = await _expenseRepository.GetAllAsync(null, x => x.PurchaseDate >= startDate && x.PurchaseDate <= endDate);

            if (expenses.IsFailure) return expenses.Error;

            List<ExpenseOverview> overviews = new List<ExpenseOverview>();

            ExpenseType? mostSpentType = _expenseService.MostSpentType(expenses.Value);

            if (mostSpentType.HasValue)
            {
                var parsers = new List<(String key, String value)>
                {
                    ("MOST_SPENT_TYPE", _localization.EXPENSE_TYPE(mostSpentType.Value))
                };

                overviews.Add(new ExpenseOverview(_textParser.Parse(_localization.MOST_EXPENT_TYPE_TEMPLATE, parsers)));
            }

            (String mostSpentMoneyPlace, decimal totalSpentMoneyInThePlace) = _expenseService.MostSpentMoneyPlace(expenses.Value);

            if (totalSpentMoneyInThePlace > 0)
            {
                var parsers = new List<(String key, String value)>
                {
                    ("MOST_SPENT_PLACE", mostSpentMoneyPlace),
                    ("TOTAL_VALUE", totalSpentMoneyInThePlace.ToString())
                };

                overviews.Add(new ExpenseOverview(_textParser.Parse(_localization.TOTAL_SPENT_MONEY_IN_THE_PLACE_TEMPLATE, parsers)));
            }

            decimal totalMonthExpense = _expenseService.TotalMoneySpent(expenses.Value);

            overviews.Add(new ExpenseOverview(_textParser.Parse(_localization.TOTAL_SPENT_IN_THE_MONTH_TEMPLATE, new List<(String key, String value)>
                {
                    ("TOTAL_SPENT_IN_THE_MONTH", totalMonthExpense.ToString())
                })));

            return overviews;
        }
    }
}
