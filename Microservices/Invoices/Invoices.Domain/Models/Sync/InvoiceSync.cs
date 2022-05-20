using System.Collections.Generic;

namespace Invoices.Domain.Models.Sync
{
    public record InvoiceSync
    {
        public List<InvoiceMonthDataSync> MonthDataSyncs { get; set; }
        public long SyncDate { get; set; }

        public InvoiceSync(long syncDate, List<InvoiceMonthDataSync> monthDataSyncs)
        {
            SyncDate = syncDate;
            MonthDataSyncs = monthDataSyncs;
        }
    }
}
