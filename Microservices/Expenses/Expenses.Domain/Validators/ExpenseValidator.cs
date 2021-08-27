using Expenses.Domain.Interfaces.Validators;
using Expenses.Domain.Localizations;
using Expenses.Domain.Models.Expenses;
using FluentValidation;
using System;

namespace Expenses.Domain.Validators
{
    public class ExpenseValidator : AbstractValidator<Expense>, IExpenseValidator
    {
        public ExpenseValidator(ILocalization localization)
        {
            RuleFor(x => x.PurchaseDay)
                .GreaterThan(DateTime.MinValue)
                .WithMessage(localization.DATE_INCORRECT);

            RuleFor(x => x.Location)
                .NotEmpty()
                .WithMessage(localization.PURCHASE_LOCATION_MUST_HAVE_VALUE);

            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage(localization.TITLE_MUST_HAVE_VALUE);

            RuleFor(x => x.Type)
                .IsInEnum()
                .WithMessage(localization.EXPENSE_TYPE_MUST_BE_VALID);

            RuleFor(x => x.TotalCostIsValid())
                .Equal(true)
                .WithMessage(localization.TOTAL_COST_DOES_NOT_MATCH_WITH_ITEMS)
                .WithName("TotalCost");

            RuleFor(x => x.Items)
                .ForEach(x => x.SetValidator(new ExpenseItemValidator(localization)));
        }
    }

    public class ExpenseItemValidator : AbstractValidator<ExpenseItem>
    {
        public ExpenseItemValidator(ILocalization localization)
        {
            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage(localization.ITEM_AMOUNT_MUST_BE_GREATER_THAN_ZERO);

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(localization.ITEM_NAME_MUST_BE_VALID);

            RuleFor(x => x.Cost)
               .NotEmpty()
               .WithMessage(localization.ITEM_COST_MUST_BE_GREATER_THAN_ZERO);
        }
    }
}
