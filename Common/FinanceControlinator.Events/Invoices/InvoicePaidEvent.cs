using FinanceControlinator.Events.Invoices.DTOs;

namespace FinanceControlinator.Events.Invoices
{
    public class InvoicePaidEvent
    {
        public InvoiceDTO Invoice { get; set; }
    }
}
