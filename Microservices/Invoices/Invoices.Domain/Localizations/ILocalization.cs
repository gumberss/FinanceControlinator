using Invoices.Domain.Enums;
using System.Globalization;

namespace Invoices.Domain.Localizations
{
    public interface ILocalization
    {
        CultureInfo CULTURE { get; }

        string DATE_INCORRECT { get; }
        string PAYMENT_ALREADY_EXISTENT { get; }
        string OVERVIEW_FUTURE_PURCHASE_PERCENT { get; }
        string INVOICE_OVERVIEW_INVESTMENT_PERCENT { get; }
        string INVOICE_OVERVIEW_BILL_PERCENT_INCREASE_COMPARED_LAST_MONTHES { get; }
        string INVOICE_OVERVIEW_BILL_PERCENT_DECREASE_COMPARED_LAST_MONTHES { get; }
        string INVOICE_OVERVIEW_BILL_PERCENT_NOT_CHANGE_COMPARED_WITH_INVOICES { get; }

        string INVOICE_COST_PERCENT_INCREASE_COMPARED_WITH_LAST_INVOICE { get; }
        string INVOICE_COST_PERCENT_NOT_CHANGE_COMPARED_WITH_LAST_INVOICE { get; }
        string INVOICE_COST_PERCENT_DECREASE_COMPARED_WITH_LAST_INVOICE { get; }

        string INVOICE_COST_PERCENT_INCREASE_COMPARED_WITH_LAST_INVOICES { get; }
        string INVOICE_COST_PERCENT_DECREASE_COMPARED_WITH_LAST_INVOICES { get; }
        string INVOICE_COST_PERCENT_NOT_CHANGE_COMPARED_WITH_LAST_INVOICES { get; }
        string OVERDUE { get; }
        string PAID { get; }
        string OPEN { get; }
        string CLOSED { get; }
        string INVOICE_SYNC_NAME { get; }

        string FORMAT_MONEY(decimal value);
        string INVOICE_ITEM_TYPE(InvoiceItemType key);
    }
}
