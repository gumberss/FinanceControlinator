namespace Invoices.DTOs.Invoices.Sync
{
    public record InvoiceMonthDataDTO
    {
        public InvoiceOverviewDTO Overview { get; set; }

        public InvoiceDTO Invoice { get; set; }
    }
}
