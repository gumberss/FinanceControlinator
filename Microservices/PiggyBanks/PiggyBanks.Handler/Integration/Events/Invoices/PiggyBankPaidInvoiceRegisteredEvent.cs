using PiggyBanks.Domain.Models;

namespace PiggyBanks.Handler.Integration.Events.Invoices
{
    public class PiggyBankPaidInvoiceRegisteredEvent
    {
        public Invoice Invoice { get; set; }
    }
}
