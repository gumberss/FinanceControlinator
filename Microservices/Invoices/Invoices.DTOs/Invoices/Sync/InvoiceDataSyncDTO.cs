using System.Collections.Generic;

namespace Invoices.DTOs.Invoices.Sync;

public record InvoiceDataSyncDTO
{
    public string SyncName { get; set; }

    public long SyncDate { get; set; }

    public List<InvoiceMonthDataDTO> MonthDataSyncs { get; set; }
}
