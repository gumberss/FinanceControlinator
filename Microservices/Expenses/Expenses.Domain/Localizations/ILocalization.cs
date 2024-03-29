﻿using Expenses.Domain.Enums;
using System;
using System.Globalization;

namespace Expenses.Domain.Localizations
{
    public interface ILocalization
    {
        public String DATE_INCORRECT { get; }
        string PURCHASE_LOCATION_MUST_HAVE_VALUE { get; }
        string TITLE_MUST_HAVE_VALUE { get; }
        string EXPENSE_TYPE_MUST_BE_VALID { get; }
        string ITEM_NAME_MUST_BE_VALID { get; }
        string ITEM_AMOUNT_MUST_BE_GREATER_THAN_ZERO { get; }
        string ITEM_COST_MUST_BE_GREATER_THAN_ZERO { get; }
        string TOTAL_COST_DOES_NOT_MATCH_WITH_ITEMS { get; }
        string AN_ERROR_OCCURRED_ON_THE_SERVER { get; }
        string EXPENSES_NOT_FOUND { get; }
        string EXPENSE_NOT_FOUND { get; }
        string EXPENSE_COST_IS_LESS_THAN_WHAT_WAS_PAID { get; }
        string EXPENSE_INSTALLMENTS_IS_LESS_THAN_TIMES_PAID { get; }
        string INSTALLMENTS_QUANTITY_IS_NOT_VALID { get; }
        string MOST_EXPENT_TYPE_TEMPLATE { get; }

        string EXPENSE_TYPE(ExpenseType expenseType);
        string TOTAL_SPENT_MONEY_IN_THE_PLACE_TEMPLATE { get; }
        string TOTAL_SPENT_IN_THE_MONTH_TEMPLATE { get; }

        CultureInfo CULTURE { get; }
    }
}
