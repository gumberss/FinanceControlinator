using PiggyBanks.Handler.Integration.Events.Invoices.DTOs;

namespace PiggyBanks.Handler.Integration.Events.Invoices
{
    public class PiggyBankPaidInvoiceRegisteredEvent
    {
        public PiggyBankInvoiceDTO Invoice { get; set; }
    }
}
