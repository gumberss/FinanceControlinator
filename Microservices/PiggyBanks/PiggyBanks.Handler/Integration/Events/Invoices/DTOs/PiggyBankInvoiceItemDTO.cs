using System;

namespace PiggyBanks.Handler.Integration.Events.Invoices.DTOs
{
    public class PiggyBankInvoiceItemDTO
    {
        public Guid ExpenseId { get; set; }

        public decimal InstallmentCost { get; set; }

        public Guid InvoiceId { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int Version { get; }
    }
}
