using Invoices.DTOs.Invoices.Enum;
using System.Collections.Generic;

namespace Invoices.DTOs.Invoices.Sync
{
    public record InvoiceSyncDTO
    {
        public string Id { get; set; }

        public string TotalCost { get; set; }

        public string CloseDate { get; set; }

        public List<InvoiceItemSyncDTO> Items { get; set; }

        public string DueDate { get; set; }

        public string PaymentDate { get; set; }

        public PaymentStatus PaymentStatus { get; set; }
    }
}
