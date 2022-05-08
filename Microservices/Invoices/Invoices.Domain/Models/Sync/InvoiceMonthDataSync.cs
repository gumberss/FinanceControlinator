namespace Invoices.Domain.Models.Sync
{
    public record InvoiceMonthDataSync
    {
        public InvoiceOverview Overview { get; set; }

        public Invoice Invoice { get; set; }

        public InvoiceMonthDataSync(InvoiceOverview overview, Invoice invoice)
        {
            Overview = overview;
            Invoice = invoice;
        }
    }
}
