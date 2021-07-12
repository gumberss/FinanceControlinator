using Invoices.Domain.Interfaces.Validators;
using Invoices.Domain.Models;
using FinanceControlinator.Common.Localizations;
using FluentValidation;
using System;

namespace Invoices.Domain.Validators
{
    public class InvoiceValidator : AbstractValidator<Invoice>, IInvoiceValidator
    {
        public InvoiceValidator(ILocalization localization)
        {
            RuleFor(x => x.Date)
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

            RuleFor(x => x.Items)
                .ForEach(x => x.SetValidator(new InvoiceItemValidator(localization)));
        }
    }

    public class InvoiceItemValidator : AbstractValidator<InvoiceItem>
    {
        public InvoiceItemValidator(ILocalization localization)
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
