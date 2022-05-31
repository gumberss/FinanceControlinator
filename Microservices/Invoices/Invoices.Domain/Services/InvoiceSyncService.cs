using FinanceControlinator.Common.Parsers.TextParsers;
using Invoices.Domain.Localizations;
using Invoices.Domain.Models;
using Invoices.Domain.Models.Sync;
using System.Linq;

namespace Invoices.Domain.Services
{
    public interface IInvoiceSyncService
    {
        InvoiceSync BuildInvoiceSync(Invoice invoice, ILocalization localization, ITextParser parser);

        InvoiceItemSync BuildInvoiceItemSync(InvoiceItem item, ILocalization localization, ITextParser parser);
    }
    public class InvoiceSyncService : IInvoiceSyncService
    {
        public InvoiceSync BuildInvoiceSync(Invoice invoice, ILocalization localization, ITextParser parser)
            => new InvoiceSync(invoice.Id,
                localization.FORMAT_MONEY(invoice.TotalCost),
                invoice.CloseDate.ToString(localization.CULTURE),
                invoice.DueDate.ToString(localization.CULTURE),
                invoice.PaymentDate?.ToString(localization.CULTURE) ?? string.Empty,
                invoice.PaymentStatus,
                invoice.Items.Select(x => BuildInvoiceItemSync(x, localization, parser)).ToList());

        public InvoiceItemSync BuildInvoiceItemSync(InvoiceItem item, ILocalization localization, ITextParser parser)
            => new InvoiceItemSync(item.Id,
                item.Title,
                parser.Parse(localization.INVOICE_INSTALLMENT_NUMBER, ("INSTALLMENT_NUMBER", item.InstallmentNumber.ToString())),
                localization.FORMAT_MONEY(item.InstallmentCost),
                item.Type,
                $"{item.PurchaseDay.ToString("m", localization.CULTURE)} - {item.PurchaseDay.ToString("t", localization.CULTURE)}");
    }
}
