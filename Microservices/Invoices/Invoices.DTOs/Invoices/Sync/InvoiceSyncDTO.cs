using System.Collections.Generic;

namespace Invoices.DTOs.Invoices.Sync
{
    public record InvoiceSyncDTO
    {
        public string SyncName { get; set; }

        public long SyncDate { get; set; }

        public List<InvoiceMonthDataDTO> MonthDataSyncs { get; set; }
    }
}
