using System.Collections.Generic;

namespace Invoices.DTOs.Invoices.Sync
{
    public record InvoiceSyncDTO
    {
        public List<InvoiceMonthDataDTO> MonthDataSyncs { get; set; }
    }
}
