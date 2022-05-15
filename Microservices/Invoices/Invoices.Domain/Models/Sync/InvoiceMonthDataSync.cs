namespace Invoices.Domain.Models.Sync
{
    public record InvoiceMonthDataSync
    {
        public InvoiceOverviewSync Overview { get; set; }

        public Invoice Invoice { get; set; }

        public InvoiceMonthDataSync(InvoiceOverviewSync overview, Invoice invoice)
        {
            Overview = overview;
            Invoice = invoice;
        }
    }
}
