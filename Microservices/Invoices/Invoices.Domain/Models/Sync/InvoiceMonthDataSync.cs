namespace Invoices.Domain.Models.Sync
{
    public record InvoiceMonthDataSync
    {
        public InvoiceOverviewSync Overview { get; set; }

        public InvoiceSync Invoice { get; set; }

        public InvoiceMonthDataSync(InvoiceOverviewSync overview, InvoiceSync invoice)
        {
            Overview = overview;
            Invoice = invoice;
        }
    }
}
